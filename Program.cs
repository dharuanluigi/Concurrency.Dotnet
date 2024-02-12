using Core;
using Core.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IConsoleApp, ConsoleApp>();

builder.Services.AddControllers();


var app = builder.Build();
app.MapControllers();
app.Run();