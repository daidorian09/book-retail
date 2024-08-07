using Domain.Entities;

namespace Application.Persistence;

public interface IOrderRepository : IAsyncRepository<Order>
{
}
