using Enums;

namespace Models;

public class Command
{
    public ECommandType Type { get; set; }
    
    public Command(string flag)
    {
        if (string.IsNullOrEmpty(flag) || string.IsNullOrWhiteSpace(flag)) throw new ArgumentException("Flag for command should be informed!");
        Type = Categorize(flag);
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