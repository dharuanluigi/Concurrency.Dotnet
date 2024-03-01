using System.Diagnostics;
using Context;
using DTO;
using Entity;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> MakeTransactionAsync([FromRoute] int id, [FromBody] DoTransactionDTO doTransactionDTO)
    {
        try
        {

            if (doTransactionDTO.Tipo != 'd' && doTransactionDTO.Tipo != 'c')
            {
                return UnprocessableEntity();
            }

            if (string.IsNullOrWhiteSpace(doTransactionDTO.Descricao) || doTransactionDTO.Descricao.Length > 10)
            {
                return UnprocessableEntity();
            }

            Customer? customer = await _clientContext.GetByIdAsync(id);


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

    [HttpGet("{id}/extrato")]
    public async Task<IActionResult> GetExtractAsync([FromRoute] int id)
    {
        try
        {
            var wt = new Stopwatch();
            wt.Start();

            Customer? customer = await _clientContext.GetByIdAsync(id);

            if (customer is null)
            {
                return NotFound();
            }

            Extract? lastExtracts = await _clientContext.GetLastExtractsByClientIdAsync(customer.Id);

            wt.Stop();
            Console.WriteLine($"\nWhole process took: {wt.ElapsedMilliseconds} ms");
            return Ok(lastExtracts);
        }
        catch (Exception ex)
        {
            Console.WriteLine("[GENERAL ERROR] When to try get extracts. Reason: " + ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}