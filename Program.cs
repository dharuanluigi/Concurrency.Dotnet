using System.Diagnostics;
using Context;
using Enums;
using Root;

try
{
    var commander = Initializer.Init(Environment.GetCommandLineArgs());
    if (commander.Command == null || commander.Command.UserId == null)
    {
        return;
    }

    var sw = new Stopwatch();
    var db = new Db();
    sw.Start();

    var mode = commander.Command.Type;

    if (mode == ECommandType.ASYNC)
    {
        Console.WriteLine("!!! PERFORMING ASYNC METHOD !!!");
        var acc = await db.GetUserByIdAsync(commander.Command.UserId);
        for (var i = 1; i < commander.Command.Loops; i++)
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
        var acc2 = db.GetUserById(commander.Command.UserId);
        for (var i = 1; i < commander.Command.Loops; i++)
        {
            var th = new Thread(() => acc2?.Withdraw(i, db))
            {
                Name = $"t{i}"
            };
            th.Start();
        }
    }
    else
    {
        Initializer.Help();
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