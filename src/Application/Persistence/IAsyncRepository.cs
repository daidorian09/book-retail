using Application.Models;

namespace Application.Persistence;

public interface IAsyncRepository<T> where T : class
{
    Task<T?> GetById(Guid id);

    Task<T> Add(T entity);

    Task Update(T entity);

    Task<PaginatedList<T>> GetPagedReponse(int page, int size);
}