using System.Text;
using Context;
using Entity;

namespace Models;

public class Client
{
    public int Id { get; set; }

    public int Limit { get; set; }

    public int Balance { get; set; }

    public int GetBalance()
    {
        return Balance;
    }

    public void Deposit(int amount)
    {
        Balance += amount;
    }

    public void Withdraw(int amount, Db db)
    {
        lock (this)
        {
            Decrease(amount);
            Console.WriteLine($"Thead {Environment.ProcessId} try to withdraw! At: {Environment.TickCount}");
            db.MakeTransaction(new Transaction(amount, 'd', "debit", Id));
            db.UpdateClientBalance(this);
            Console.WriteLine(this);
        }
    }

    public Task WithdrawAsync(int amount, Db db)
    {
        return Task.Run(async () =>
        {
            Decrease(amount);
            Console.WriteLine($"Thead {Environment.ProcessId} try to withdraw ASYNC! At: {Environment.TickCount}");
            await db.MakeTransactionAsync(new Transaction(amount, 'c', "credit", Id));
            await db.UpdateClientBalanceAsync(this);
            Console.WriteLine(this);
        });
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.AppendLine("======== Statement =======");
        builder.AppendLine("Id: " + Id);
        builder.AppendLine("Limit: " + Limit);
        builder.AppendLine("Balance: " + Balance);
        builder.AppendLine("===== Final Statement =====");
        return builder.ToString();
    }

    private void Decrease(int qtd)
    {
        Balance -= qtd;
    }
}