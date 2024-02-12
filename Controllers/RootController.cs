using Core;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class RootController : ControllerBase
{
    private readonly IConsoleApp ConsoleApp;

    public RootController(IConsoleApp consoleApp)
    {
        ConsoleApp = consoleApp;
    }

    [HttpGet]
    public IActionResult GetAsync()
    {
        var args = new[] {"app.dll", "-a", "-i", "4", "-l", "2"};

        ConsoleApp.Run(args);

        return NoContent();
    }
}