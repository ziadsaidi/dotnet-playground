using Sales.Domain.Entities;

namespace Sales.Application.Interfaces;

public interface ISaleRepository
{
  Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

  IAsyncEnumerable<Sale> GetSales(CancellationToken cancellationToken);

  Task<bool> ExistsAsync(string name, CancellationToken cancellationToken);

  Task AddAsync(Sale sale, CancellationToken cancellationToken);

  Task UpdateAsync(Sale sale, CancellationToken cancellationToken);

  Task DeleteAsync(Guid id, CancellationToken cancellationToken);

  IAsyncEnumerable<Sale> GetSalesByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken);

  IAsyncEnumerable<Sale> GetSalesByProductIdAsync(Guid productId, CancellationToken cancellationToken);

  IAsyncEnumerable<Sale> GetSalesByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken);
}
