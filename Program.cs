using System.Diagnostics;
using Context;

try
{
    var mode = Environment.GetCommandLineArgs()[1];
    var sw = new Stopwatch();
    var db = new Db();

    sw.Start();

    if (mode == "async")
    {
        Console.WriteLine("!!! PERFORMING ASYNC METHOD !!!");
        var acc = await db.GetUserByIdAsync("1");

        for (var i = 1; i < 5000; i++)
        {
            if (acc != null)
            {
                await acc.WithdrawAsync(i, db);
            }
        }
    }
    else
    {
        Console.WriteLine("!!! PERFORMING MULTTREADING METHOD !!!");
        var acc2 = db.GetUserById("2");
        for (var i = 0; i < 5000; i++)
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
    Console.WriteLine("Wrong usage, pls insert -- async | mt");
}