using System.Diagnostics;
using Context;
using Enums;
using Root;

try
{
    var commander = Initializer.Init(Environment.GetCommandLineArgs());
    
    var sw = new Stopwatch();
    var db = new Db();
    sw.Start();

    var mode = commander.Command.Type;

    if (mode == ECommandType.ASYNC)
    {
        Console.WriteLine("!!! PERFORMING ASYNC METHOD !!!");
        var acc = await db.GetUserByIdAsync("1");

        for (var i = 1; i < 5; i++)
        {
            if (acc != null)
            {
                await acc.WithdrawAsync(i, db);
            }
        }
    }
    else if (mode == ECommandType.MULTTREADING)
    {
        Console.WriteLine("!!! PERFORMING MULTTREADING METHOD !!!");
        var acc2 = db.GetUserById("2");
        for (var i = 0; i < 5; i++)
        {
            var th = new Thread(() => acc2?.Withdraw(i, db))
            {
                Name = $"t{i}"
            };
            th.Start();
        }
    }

    sw.Stop();
    Console.WriteLine("*** End of program! ***");
    Console.WriteLine("Took: " + sw.ElapsedMilliseconds + "ms");
}
catch (IndexOutOfRangeException)
{
    Console.WriteLine("Wrong usage format. Missing parameter flag");
    Initializer.Help();
}