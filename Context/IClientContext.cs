using Entity;

namespace Context;

public interface IClientContext
{
    Task<Customer?> GetByIdAsync(int id);

    Customer? GetByIdLock(int id);

    Task DoTransactionAsync(Transaction transaction);
    
    void DoTransactionLock(Transaction transaction);

    Task UpdateCustomerBalanceAsync(Customer customer);

    void UpdateCustomerBalanceLock(Customer customer);

    Task<Extract?> GetLastExtractsByClientIdAsync(int id);

    Extract? GetLastExtractsByClientIdLock(int id);
}