using System.Text;
using Org.BouncyCastle.Security;

namespace Entity;

public class Customer
{
    public int Id { get; set; }

    public int Limit { get; set; }

    public int Balance { get; set; }

    public Customer(int id, int limit, int balance)
    {
        Id = id;
        Limit = limit;
        Balance = balance;
    }

    public void Withdraw(int amount)
    {
        var backUpBalance = Balance;
        Balance -= amount;
        int realBalance = Balance * -1;

        if (realBalance > Limit)
        {
            Balance = backUpBalance;
            throw new InvalidParameterException("The balance could't be greater than limit!");
        }
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
}