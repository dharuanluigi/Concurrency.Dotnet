using System.Text;
using Models;

namespace Root;

public class Commander
{
    public Command Command { get; set; }

    public Commander(string flag)
    {
        if (string.IsNullOrEmpty(flag) || string.IsNullOrWhiteSpace(flag)) throw new ArgumentException("Flag to start must be informed, cannot be null or empty");
        Command = new Command(flag);
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.AppendLine("Command type: " + Command.Type);
        return builder.ToString();
    }
}