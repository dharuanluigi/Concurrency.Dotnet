namespace Models;

public class Transaction
{
    public int Value { get; set; }

    public char Type { get; set; }

    public string Description { get; set; } = null!;

    public int ClientId { get; set; }

    public Transaction(int value, char type, string description, int clientId)
    {
        Value = value;
        Type = type;
        Description = description;
        ClientId = clientId;
    }
}