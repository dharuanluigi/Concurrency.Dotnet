using Entity;

namespace Context;

public interface IClientContext
{
    Task<Customer?> GetByIdAsync(int id);

    Task DoTransactionAsync(Transaction transaction);

    Task UpdateCustomerBalanceAsync(Customer customer);

    Task<Extract?> GetLastExtractsByClientId(int id);
}