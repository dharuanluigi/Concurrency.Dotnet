using Models;

namespace Root;

public class Initializer
{
    public static Commander Init(string[] args)
    {
        var commander = new Commander(args[1]);
        commander.Command.SetUserId(args[3]);
        commander.Command.SetLoops(args[5]);

        return commander;
    }

    public static void Help()
    {
        Console.WriteLine("\n\n========================= HELP SCREEN ================================");
        var flags = new List<string>()
        {
            "\nFor usage to get help <dll> -h",
            "DLL is the program executable.\n",
            "<dll> -[flag]\n",
            "[flag]  [flagid] [value of id] [qtd of loops concurrencies]| [what the flag does]",
            "-----------------------------",
            "-a -i $user id$ -l $loops$ | To use async",
            "-m -i $user id$ -l $loops$ | To use multi thread",
            "-h | To see this screen"
        };

        foreach(var flag in flags)
        {
            Console.WriteLine(flag);
        }
        Console.WriteLine("\n========================= END HELP SCREEN ============================");
    }
}