using System.Net;
using Context;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Models;
using Org.BouncyCastle.Security;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IClientContext _clientContext;

    public ClientesController(IClientContext clientContext)
    {
        _clientContext = clientContext;
    }

    [HttpPost("{id}/transacoes")]
    public async Task<IActionResult> GetAsync([FromRoute] int id, [FromBody] DoTransactionDTO doTransactionDTO)
    {
        try
        {
            var customer = await _clientContext.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            customer.Withdraw(doTransactionDTO.Valor);

            var transaction = new Transaction(customer, doTransactionDTO.Valor, doTransactionDTO.Tipo, doTransactionDTO.Descricao);

            await _clientContext.DoTransactionAsync(transaction);
            await _clientContext.UpdateCustomerBalanceAsync(customer);

            return Ok(new CustomerReport(customer.Limit, customer.Balance));
        }
        catch (InvalidParameterException ex)
        {
            return UnprocessableEntity(new ApiResponse(ex.Message));
        }
        catch (Exception ex)
        {
            Console.WriteLine("[GENERAL ERROR] What is this: " + ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}