namespace Application.Persistence;

public interface IRepository<T>
{
    Task<string> CreateAsync(string bucketName, string id, T item);
    Task<T> GetByIdAsync(string bucketName, string id);
    Task<(T, ulong cas)> GetByIdWithCasAsync(string bucketName, string id);
    Task<bool> ExistsAsync(string bucketName, string fieldName, string value);
    Task<List<dynamic>> GetWithPaginationAsync(string bucketName, string fieldName, string value, int pageNumber, int pageSize);
    Task<List<dynamic>> GetWithDateAsync(string bucketName, string fieldName, long startDate, long endDate);
    Task<List<dynamic>> GetMonthlyStatisticsAsync(string bucketName, int year);
    Task<bool> PartialUpdateAsync(string bucketName, string id, Dictionary<string, object> fieldsToUpdate, ulong cas);
}