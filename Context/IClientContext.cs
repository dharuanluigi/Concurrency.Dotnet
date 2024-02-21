using Entity;
using Models;

namespace Context;

public interface IClientContext
{
    Task<Customer?> GetByIdAsync(int id);

    Task DoTransactionAsync(Transaction transaction);

    Task UpdateCustomerBalanceAsync(Customer customer);
}