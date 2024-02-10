using System.Data;
using Models;
using MySql.Data.MySqlClient;

namespace Context;

public class Db
{
    private readonly MySqlConnection Connection;

    private readonly MySqlCommand Command;

    private readonly string connectionString = "server=127.0.0.1;uid=root;pwd=Brasil123;database=tft;sslmode=none;";

    public Db()
    {
        Connection = new MySqlConnection(connectionString);
        Command = new MySqlCommand()
        {
            Connection = Connection,
        };
    }

    public async Task<Client?> GetUserByIdAsync(string clientId)
    {
        var user = new Client();
        try
        {
            Command.CommandText = @"SELECT id, c_limit, balance FROM clients WHERE id = @clientId;";
            Command.Parameters.AddWithValue("@clientId", clientId);

            await Connection.OpenAsync();
            using var reader = await Command.ExecuteReaderAsync();
            {   
                while(await reader.ReadAsync())
                {
                    user.Id = reader.GetInt32("id");
                    user.Limit = reader.GetInt32("c_limit");
                    user.Balance = reader.GetInt32("balance");
                }
            }
        }
        catch (MySqlException e)
        {
            Console.WriteLine("[ERROR] Try to get client. Reason: " + e.Message);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine("[ERROR] Try to get client. Reason: " + e.Message);
        }
        finally
        {
            await Connection.CloseAsync();
        }

        return user;
    }

    public Client GetUserById(string clientId)
    {
        var user = new Client();
        try
        {
            Command.CommandText = @"SELECT id, c_limit, balance FROM clients WHERE id = @clientId;";
            Command.Parameters.AddWithValue("@clientId", clientId);

            Connection.Open();
            using var reader = Command.ExecuteReader();
            {   
                while(reader.Read())
                {
                    user.Id = reader.GetInt32("id");
                    user.Limit = reader.GetInt32("c_limit");
                    user.Balance = reader.GetInt32("balance");
                }
            }
        }
        catch (MySqlException e)
        {
            Console.WriteLine("[ERROR] Try to get client. Reason: " + e.Message);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine("[ERROR] Try to get client. Reason: " + e.Message);
        }
        finally
        {
            Connection.Close();
        }

        return user;
    }

    public async Task MakeTransactionAsync(Transaction transaction)
    {
        // INSERT INTO transactions VALUES(NULL, 0, 'x', 'x', 'x', 0);
        try
        {
            Command.Parameters.Clear();
            Command.CommandText = @"INSERT INTO transactions VALUES
            (NULL, @t_value, @t_type, @t_desc, @process_at, @client_id);";
            
            Command.Parameters.AddWithValue("@t_value", transaction.Value);
            Command.Parameters.AddWithValue("@t_type", transaction.Type);
            Command.Parameters.AddWithValue("@t_desc", transaction.Description);
            Command.Parameters.AddWithValue("@process_at", DateTime.UtcNow);
            Command.Parameters.AddWithValue("@client_id", transaction.ClientId);
            
            await Connection.OpenAsync();
            await Command.ExecuteNonQueryAsync();
        }
        catch (MySqlException e)
        {
            Console.WriteLine("[ERROR] Try to make a transaction. Reason: " + e.Message);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine("[ERROR] Try to get client. Reason: " + e.Message);
        }
        finally 
        {
            await Connection.CloseAsync();
        }
    }

    public void MakeTransaction(Transaction transaction)
    {
        // INSERT INTO transactions VALUES(NULL, 0, 'x', 'x', 'x', 0);
        try
        {
            Command.Parameters.Clear();
            Command.CommandText = @"INSERT INTO transactions VALUES
            (NULL, @t_value, @t_type, @t_desc, @process_at, @client_id);";
            
            Command.Parameters.AddWithValue("@t_value", transaction.Value);
            Command.Parameters.AddWithValue("@t_type", transaction.Type);
            Command.Parameters.AddWithValue("@t_desc", transaction.Description);
            Command.Parameters.AddWithValue("@process_at", DateTime.UtcNow);
            Command.Parameters.AddWithValue("@client_id", transaction.ClientId);
            
            Connection.Open();
            Command.ExecuteNonQuery();
        }
        catch (MySqlException e)
        {
            Console.WriteLine("[ERROR] Try to make a transaction. Reason: " + e.Message);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine("[ERROR] Try to get client. Reason: " + e.Message);
        }
        finally 
        {
            Connection.Close();
        }
    }

    public async Task UpdateClientBalanceAsync(Client client)
    {
        try
        {   
            Command.Parameters.Clear();
            Command.CommandText = @"UPDATE clients
                                    SET balance = @newBalance
                                    WHERE id = @clientId;";
            Command.Parameters.AddWithValue("@newBalance", client.Balance);
            Command.Parameters.AddWithValue("@clientId", client.Id);

            await Connection.OpenAsync();
            await Command.ExecuteNonQueryAsync();
        }
        catch (MySqlException e)
        {
            Console.WriteLine("[ERROR] Try to update client balance. Reason: " + e.Message);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine("[ERROR] Try to get client. Reason: " + e.Message);
        }
        finally
        {
            await Connection.CloseAsync();
        }
    }

    public void UpdateClientBalance(Client client)
    {
        try
        {   
            Command.Parameters.Clear();
            Command.CommandText = @"UPDATE clients
                                    SET balance = @newBalance
                                    WHERE id = @clientId;";
            Command.Parameters.AddWithValue("@newBalance", client.Balance);
            Command.Parameters.AddWithValue("@clientId", client.Id);

            Connection.Open();
            Command.ExecuteNonQuery();
        }
        catch (MySqlException e)
        {
            Console.WriteLine("[ERROR] Try to update client balance. Reason: " + e.Message);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine("[ERROR] Try to get client. Reason: " + e.Message);
        }
        finally
        {
            Connection.Close();
        }
    }
}