using Entity;
using Models;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Security;

namespace Context;

public class ClientContext : IClientContext
{
    private readonly MySqlCommand _command;

    private readonly MySqlConnection _connection;

    public ClientContext()
    {
        var conn = Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? "server=localhost;uid=root;pwd=Brasil123;database=tft;sslmode=none;MaxPoolSize=1000;ConnectionLifeTime=60;UseCompression=true;CacheServerProperties=true;";
        _connection = new MySqlConnection(conn);
        _command = new MySqlCommand()
        {
            Connection = _connection
        };
    }

    public Customer? GetById(int id)
    {
        lock (this)
        {
            try
            {
                _command.Parameters.Clear();
                _command.CommandText = "SELECT id, c_limit, balance FROM clients WHERE id=@clientId;";
                _command.Parameters.AddWithValue("@clientId", id);

                int id_result = 0;
                int limit_result = 0;
                int balance_result = 0;

                _connection.Open();
                using var reader = _command.ExecuteReader();
                while(reader.Read())
                {
                    id_result = reader.GetInt32("id");
                    limit_result = reader.GetInt32("c_limit");
                    balance_result = reader.GetInt32("balance");
                }
                _connection.Close();

                return new Customer(id_result, limit_result, balance_result);
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("[ERROR] Trying to make query to get customer by id async, reason: " + ex.Message);
            }
            finally
            {
                _connection.Close();
            }

            return default;
        }
    }

    public void DoTransaction(Transaction transaction)
    {
        lock (this)
        {
            try
            {

                if (transaction.Customer != null)
                {
                    _command.Parameters.Clear();
                    _command.CommandText = "INSERT INTO transactions VALUES(NULL, @value, @type, @desc, @processAt, @clientId);";
                    _command.Parameters.AddWithValue("@value", transaction.Value);
                    _command.Parameters.AddWithValue("@type", transaction.Type);
                    _command.Parameters.AddWithValue("@desc", transaction.Description);
                    _command.Parameters.AddWithValue("@processAt", DateTime.UtcNow);
                    _command.Parameters.AddWithValue("@clientId", transaction.Customer.Id);

                    _connection.Open();
                    _command.ExecuteNonQuery();
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
            finally
            {
                _connection.Close();
            }
        }
    }

    public void UpdateCustomerBalance(Customer customer)
    {
        lock (this)
        {
            try
            {
                _command.Parameters.Clear();
                _command.CommandText = "UPDATE clients SET balance = @balance WHERE id = @id;";
                _command.Parameters.AddWithValue("@balance", customer.Balance);
                _command.Parameters.AddWithValue("@id", customer.Id);

                _connection.Open();
                _command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("[ERROR] Trying to update client balance, reason: " + ex.Message);
            }
            finally
            {
                _connection.Close();
            }
        }
    }

    public Extract? GetLastExtractsByClientId(int id)
    {
        lock (this)
        {
            try
            {
                _command.Parameters.Clear();
                _command.CommandText = "SELECT c.c_limit, c.balance, t.t_value, t.t_type, t.t_desc, t.process_at FROM clients c LEFT JOIN transactions t ON c.id = t.client_id WHERE c.id = @id ORDER BY t.process_at DESC LIMIT 10;";
                _command.Parameters.AddWithValue("@id", id);

                int i = 0;
                var total = 0;
                var limit = 0;

                var listOfTransactions = new List<Transacao>();

                _connection.Open();
                using var reader = _command.ExecuteReader();
                while (reader.Read())
                {
                    if (i != 0)
                    {
                        if (!reader.IsDBNull(2))
                        {
                            listOfTransactions.Add(new Transacao(reader.GetInt32("t_value"), reader.GetChar("t_type"), reader.GetString("t_desc"), reader.GetDateTime("process_at")));
                        }
                    }
                    else
                    {
                        total = reader.GetInt32("balance");
                        limit = reader.GetInt32("c_limit");
                        i = 1;
                        if (!reader.IsDBNull(2))
                        {
                            listOfTransactions.Add(new Transacao(reader.GetInt32("t_value"), reader.GetChar("t_type"), reader.GetString("t_desc"), reader.GetDateTime("process_at")));
                        }
                    }

                }
                _connection.Close();

                return new Extract(new Balance(total, DateTime.UtcNow.ToString(), limit), listOfTransactions);
            }
            catch(MySqlException ex)
            {
                Console.WriteLine("[Error] Trying to get client extracts, error reason: " + ex.Message);
            }
            finally
            {
                _connection.Close();
            }

            return default;
        }
    }
}