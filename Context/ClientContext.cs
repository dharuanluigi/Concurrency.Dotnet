using Entity;
using Models;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Security;

namespace Context;

public class ClientContext : IClientContext
{
    private readonly string _str;

    public ClientContext()
    {
        var conn = Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? "server=localhost;uid=root;pwd=Brasil123;database=tft;sslmode=none;MaxPoolSize=1000;CacheServerProperties=true;";
        _str = conn;
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        Console.WriteLine($"Process starts for id: {id}");
        try
        {
            int id_result = 0;
            int limit_result = 0;
            int balance_result = 0;

            using (var reader = await MySqlHelper.ExecuteReaderAsync(_str, $"SELECT id, c_limit, balance FROM clients WHERE id={id};"))
            {
                while (await reader.ReadAsync())
                {
                    id_result = await reader.GetFieldValueAsync<int>(reader.GetOrdinal("id"));
                    limit_result = await reader.GetFieldValueAsync<int>(reader.GetOrdinal("c_limit"));
                    balance_result = await reader.GetFieldValueAsync<int>(reader.GetOrdinal("balance"));
                }
            };

            return new Customer(id_result, limit_result, balance_result);
        }
        catch (MySqlException ex)
        {
            Console.WriteLine("[ERROR] Trying to make query to get customer by id async, reason: " + ex.Message + " link: " + ex.InnerException);
        }
        finally
        {
            Console.WriteLine($"Process finishes for id: {id}");
        }

        return default;
    }

    public async Task DoTransactionAsync(Transaction transaction)
    {
        try
        {
            if (transaction.Customer is not null)
            {
                var query = $"INSERT INTO transactions VALUES(NULL, {transaction.Value}, '{transaction.Type}', '{transaction.Description}', '{DateTime.UtcNow.ToString("yyyy/MM/dd hh:mm:ss.f")}', {transaction.Customer.Id});";
                await MySqlHelper.ExecuteNonQueryAsync(_str, query);
            }
            else
            {
                throw new InvalidParameterException("[ERROR] On transactions, customer is required!!");
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine("[ERROR] Trying to make transaction at db reason: " + ex.Message);
        }
    }

    public async Task UpdateCustomerBalanceAsync(Customer customer)
    {
        try
        {
            await MySqlHelper.ExecuteNonQueryAsync(_str, $"UPDATE clients SET balance = {customer.Balance} WHERE id = {customer.Id};");
        }
        catch (MySqlException ex)
        {
            Console.WriteLine("[ERROR] Trying to update client balance, reason: " + ex.Message);
        }
    }

    public async Task<Extract?> GetLastExtractsByClientIdAsync(int id)
    {
        try
        {
            int i = 0;
            var total = 0;
            var limit = 0;

            var listOfTransactions = new List<Transacao>();

            using (var reader = await MySqlHelper.ExecuteReaderAsync(_str, $"SELECT c.c_limit, c.balance, t.t_value, t.t_type, t.t_desc, t.process_at FROM clients c LEFT JOIN transactions t ON c.id = t.client_id WHERE c.id = {id} ORDER BY t.process_at DESC LIMIT 10;"))
            {
                while (await reader.ReadAsync())
                {
                    if (i != 0)
                    {
                        if (await reader.IsDBNullAsync(2) == false)
                        {
                            listOfTransactions.Add(new Transacao(
                                await reader.GetFieldValueAsync<int>(2),
                                await reader.GetFieldValueAsync<char>(3),
                                await reader.GetFieldValueAsync<string>(4),
                                await reader.GetFieldValueAsync<DateTime>(5)));
                        }
                    }
                    else
                    {
                        limit = await reader.GetFieldValueAsync<int>(0);
                        total = await reader.GetFieldValueAsync<int>(1);
                        i = 1;
                        if (await reader.IsDBNullAsync(2) == false)
                        {
                            listOfTransactions.Add(new Transacao(
                                await reader.GetFieldValueAsync<int>(2),
                                await reader.GetFieldValueAsync<char>(3),
                                await reader.GetFieldValueAsync<string>(4),
                                await reader.GetFieldValueAsync<DateTime>(5)));
                        }
                    }
                }
            }

            return new Extract(new Balance(total, DateTime.UtcNow.ToString(), limit), listOfTransactions);
        }
        catch (MySqlException ex)
        {
            Console.WriteLine("[Error] Trying to get client extracts, error reason: " + ex.Message);
        }

        return default;
    }

    public Customer? GetByIdLock(int id)
    {
        lock (this)
        {
            Console.WriteLine($"Process starts for id: {id}");
            try
            {
                int id_result = 0;
                int limit_result = 0;
                int balance_result = 0;

                using (var reader = MySqlHelper.ExecuteReader(_str, $"SELECT id, c_limit, balance FROM clients WHERE id={id};"))
                {
                    while (reader.Read())
                    {
                        id_result = reader.GetOrdinal("id");
                        limit_result = reader.GetOrdinal("c_limit");
                        balance_result = reader.GetOrdinal("balance");
                    }
                };

                return new Customer(id_result, limit_result, balance_result);
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("[ERROR] Trying to make query to get customer by id async, reason: " + ex.Message + " link: " + ex.InnerException);
            }
            finally
            {
                Console.WriteLine($"Process finishes for id: {id}");
            }

            return default;
        }
    }

    public void DoTransactionLock(Transaction transaction)
    {
        lock (this)
        {
            try
            {
                if (transaction.Customer is not null)
                {
                    var query = $"INSERT INTO transactions VALUES(NULL, {transaction.Value}, '{transaction.Type}', '{transaction.Description}', '{DateTime.UtcNow.ToString("yyyy/MM/dd hh:mm:ss.f")}', {transaction.Customer.Id});";
                    MySqlHelper.ExecuteNonQuery(_str, query);
                }
                else
                {
                    throw new InvalidParameterException("[ERROR] On transactions, customer is required!!");
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("[ERROR] Trying to make transaction at db reason: " + ex.Message);
            }
        }
    }

    public void UpdateCustomerBalanceLock(Customer customer)
    {
        lock (this)
        {
            try
            {
                MySqlHelper.ExecuteNonQuery(_str, $"UPDATE clients SET balance = {customer.Balance} WHERE id = {customer.Id};");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("[ERROR] Trying to update client balance, reason: " + ex.Message);
            }
        }
    }

    public Extract? GetLastExtractsByClientIdLock(int id)
    {
        lock (this)
        {
            try
            {
                int i = 0;
                var total = 0;
                var limit = 0;

                var listOfTransactions = new List<Transacao>();

                using (var reader = MySqlHelper.ExecuteReader(_str, $"SELECT c.c_limit, c.balance, t.t_value, t.t_type, t.t_desc, t.process_at FROM clients c LEFT JOIN transactions t ON c.id = t.client_id WHERE c.id = {id} ORDER BY t.process_at DESC LIMIT 10;"))
                {
                    while (reader.Read())
                    {
                        if (i != 0)
                        {
                            if (reader.IsDBNull(reader.GetOrdinal("t_value")) is false)
                            {
                                listOfTransactions.Add(new Transacao(reader.GetOrdinal("t_value"), reader.GetChar("t_type"), reader.GetString("t_desc"), reader.GetDateTime("process_at")));
                            }
                        }
                        else
                        {
                            limit = reader.GetOrdinal("c_limit");
                            total = reader.GetOrdinal("balance");
                            if (reader.IsDBNull(reader.GetOrdinal("t_value")) is false)
                            {
                                listOfTransactions.Add(new Transacao(reader.GetOrdinal("t_value"), reader.GetChar("t_type"), reader.GetString("t_desc"), reader.GetDateTime("process_at")));
                            }
                        }
                    }
                }

                return new Extract(new Balance(total, DateTime.UtcNow.ToString(), limit), listOfTransactions);
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("[Error] Trying to get client extracts, error reason: " + ex.Message);
            }

            return default;
        }
    }
}