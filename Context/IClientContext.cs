using Entity;

namespace Context;

public interface IClientContext
{
    Customer? GetById(int id);

    void DoTransaction(Transaction transaction);

    void UpdateCustomerBalance(Customer customer);

    Extract? GetLastExtractsByClientId(int id);
}