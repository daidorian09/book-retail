using Domain.Entities;

namespace Application.Persistence;

public interface ICustomerRepository : IAsyncRepository<Customer>
{
}
