using Application.Persistence;
using Domain.Entities;

namespace Application.Contracts.Persistence;

public interface IBookRepository : IAsyncRepository<Book>
{
}