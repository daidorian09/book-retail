namespace Application.Persistence;

public interface IRepository<T>
{
    Task<string> CreateAsync(string bucketName, string id, T item);
    Task<T> GetByIdAsync(string bucketName, string id);
    Task<bool> ExistsAsync(string bucketName, string fieldName, string value);
    Task<List<dynamic>> GetWithPagination(string bucketName, string fieldName, string value, int pageNumber, int pageSize);
    Task<bool> UpdateAsync(string bucketName, string id, T item);
}