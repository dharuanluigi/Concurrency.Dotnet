using Entity;

namespace Entity;

public class Transaction
{
    public int Value { get; set; }

    public char Type { get; set; }

    public string Description { get; set; } = null!;

    public int ClientId { get; set; }

    public Customer? Customer { get; set; }

    public Transaction(int value, char type, string description, int clientId)
    {
        Value = value;
        Type = type;
        Description = description;
        ClientId = clientId;
    }

    public Transaction(Customer customer, int value, char type, string description)
    {
        Customer = customer;
        Value = value;
        Type = type;
        Description = description;
    }
}