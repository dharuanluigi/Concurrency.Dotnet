using Core;
using Core.Interfaces;

// if -c is informed, then console mode is used
if (args[0] == "-c")
{
    var console = new ConsoleApp();
    await console.Run(args);
}
else
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddSingleton<IConsoleApp, ConsoleApp>();

    builder.Services.AddControllers();

    var app = builder.Build();
    app.MapControllers();
    app.Run();
}