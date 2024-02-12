using Models;

namespace Root;

public class Initializer
{
    public static Commander Init(string[] args)
    {
        return new Commander(args[1]);
    }

    public static void Help()
    {
        Console.WriteLine("\n\n========================= HELP SCREEN ================================");
        var flags = new List<string>()
        {
            "\nFor usage to get help <dll> -h",
            "DLL is the program executable.\n",
            "<dll> -[flag]\n",
            "[flag] | [what the flag does]",
            "-----------------------------",
            "-a | To use async test",
            "-m | To use multi thread test",
            "-h | To see this screen"
        };

        foreach(var flag in flags)
        {
            Console.WriteLine(flag);
        }
        Console.WriteLine("\n========================= END HELP SCREEN ============================");
    }
}