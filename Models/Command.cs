using Enums;

namespace Models;

public class Command
{
    public ECommandType Type { get; set; }
    
    public string? UserId { get; private set; }

    public int Loops { get; private set; }
    
    public Command(string flag)
    {
        if (string.IsNullOrEmpty(flag) || string.IsNullOrWhiteSpace(flag)) throw new ArgumentException("Flag for command should be informed!");
        Type = Categorize(flag);
    }

    public void SetUserId(string id)
    {
        if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id)) throw new ArgumentException("User id must be sent");
        UserId = id;
    }

    public void SetLoops(string loops)
    {
        if (string.IsNullOrEmpty(loops) || string.IsNullOrWhiteSpace(loops)) throw new ArgumentException("Loops should be pass!");
        try {
            Loops = int.Parse(loops) + 1;
        }
        catch (Exception)
        {
            throw new ArgumentException("Loop should be a number");
        }
    }

    private static ECommandType Categorize(string flag)
    {
        string? realFlagName = flag switch
        {
            "-a" => "ASYNC",
            "-m" => "MULTTREADING",
            _ => "HELP",
        };
        
        return Enum.Parse<ECommandType>(realFlagName);
    }
}