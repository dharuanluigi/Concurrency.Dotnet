using Core.Interfaces;
using DTO;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IConsoleApp ConsoleApp;

    public ClientesController(IConsoleApp consoleApp)
    {
        ConsoleApp = consoleApp;
    }

    [HttpPost("{id}/transacoes")]
    public IActionResult GetAsync([FromRoute] int id, [FromBody] DoTransactionDTO doTransactionDTO)
    {
        // var args = new[] {"app.dll", "-a", "-i", "4", "-l", "2"};

        // ConsoleApp.Run(args);

        return NoContent();
    }
}