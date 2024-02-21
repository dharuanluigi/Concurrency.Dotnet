using Context;
using Core;

// if -c is informed, then console mode is used
if (args.Length > 0 && args[0] == "-c")
{
    var console = new ConsoleApp();
    await console.Run(args);
}
else
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddSingleton<IClientContext, ClientContext>();

    builder.Services.AddControllers();

    var app = builder.Build();
    app.MapControllers();
    app.Run();
}