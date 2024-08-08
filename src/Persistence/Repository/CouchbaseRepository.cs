using Application.Persistence;
using Couchbase;
using Couchbase.KeyValue;
using Microsoft.Extensions.Logging;

namespace Persistence.Repository
{
    public class CouchbaseRepository<T> : IRepository<T>
    {
        private readonly ICluster _cluster;
        private readonly ILogger<CouchbaseRepository<T>> _logger;

        public CouchbaseRepository(ICluster cluster, ILogger<CouchbaseRepository<T>> logger)
        {
            _cluster = cluster;
            _logger = logger;
        }

        private async Task<IBucket> GetBucketAsync(string bucketName)
        {
            return await _cluster.BucketAsync(bucketName);
        }

        public async Task<bool> CreateAsync(string bucketName, string id, T item)
        {
            try
            {
                _logger.LogInformation("Document : {id} will be inserted into {bucket}", id, bucketName);
                var bucket = await GetBucketAsync(bucketName);
                var collection = bucket.DefaultCollection();
                var result = await collection.UpsertAsync(id, item);

                return true;
                            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred during inserting the document, Id :{id} - Bucket : {bucket}", id, bucketName);
                return default;
            }

        }

        public async Task<T> GetByIdAsync(string bucketName, string id)
        {
            try
            {
                _logger.LogInformation("Document :{id} will be fetched from {bucket}",id, bucketName);
                var bucket = await GetBucketAsync(bucketName);
                var collection = bucket.DefaultCollection();
                var result = await collection.GetAsync(id);
                return result.ContentAs<T>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Exception occurred during getting the document, Id :{id} - Bucket : {bucket}", id, bucketName);
                return default;
            }
            
        }

        public async Task<bool> ExistsAsync(string bucketName, string fieldName, string value)
        {
            var query = $"SELECT COUNT(*) AS count FROM `{bucketName}` WHERE `{fieldName}` = \"{value}\";";
            try
            {
                _logger.LogInformation("Executing query: {Query}", query);

                var result = await _cluster.QueryAsync<dynamic>(query);
                var row = await result.Rows.FirstOrDefaultAsync();

                var count = row?.count ?? 0;
                return count > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Query execution failed : {Query}", query);
                return default;
            }
            
        }

        public async Task<List<dynamic>> GetWithPaginationAsync(string bucketName, string fieldName, string value, int pageNumber, int pageSize)
        {
            var offset = (pageNumber - 1) * pageSize;
            var query = $"SELECT * FROM `{bucketName}` WHERE {fieldName} = \"{value}\"  LIMIT {pageSize} OFFSET {offset}";
            try
            {
                _logger.LogInformation("Executing query: {Query}", query);

                var queryResult = await _cluster.QueryAsync<dynamic>(query);

                return await queryResult.Rows.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Query execution failed : {Query}", query);
                return default;
            }
           
        }

        public async Task<bool> PartialUpdateAsync(string bucketName, string id, Dictionary<string, object> fieldsToUpdate, ulong cas)
        {
            try
            {
                _logger.LogInformation("Document :{id} will be partially updated for {bucket}, fields {}", id, bucketName, fieldsToUpdate);

                var bucket = await GetBucketAsync(bucketName);
                var collection = bucket.DefaultCollection();

                var specs = fieldsToUpdate
                    .Select(field => MutateInSpec.Upsert(field.Key, field.Value))
                    .ToList();

                var result = await collection.MutateInAsync(id, specs, options => options.Cas(cas));

                return result.Cas != 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred during partial updating the document, Id :{id} - Bucket : {bucket}", id, bucketName);
                return default;
            }            
        }

        public async Task<(T, ulong cas)> GetByIdWithCasAsync(string bucketName, string id)
        {
            try
            {
                _logger.LogInformation("Document :{id} will be fetched with cas value for {bucket}", id, bucketName);

                var bucket = await GetBucketAsync(bucketName);
                var collection = bucket.DefaultCollection();
                var result = await collection.GetAsync(id);

                return (result.ContentAs<T>(), result.Cas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred during getting the document with case, Id :{id} - Bucket : {bucket}", id, bucketName);
                return default;
            }
            
        }

        public async Task<List<dynamic>> GetWithDateAsync(string bucketName, string fieldName, long startDate, long endDate)
        {
            var query = $"SELECT * FROM `{bucketName}` WHERE {fieldName} BETWEEN {startDate} AND {endDate}";

            try
            {
                _logger.LogInformation("Executing query: {Query}", query);

                var queryResult = await _cluster.QueryAsync<dynamic>(query);

                return await queryResult.Rows.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Query execution failed : {Query}", query);

                return default;
            }
            
          
        }

        public async Task<List<dynamic>> GetMonthlyStatisticsAsync(string bucketName, int year)
        {
            var query = $@"
                         WITH filteredOrders AS (
                         SELECT TOSTRING(DATE_PART_STR(DATE_ADD_STR('1970-01-01T00:00:00Z', orderDate * 1000, 'millisecond'), 'month')) AS month,
                         ARRAY_LENGTH(items) AS purchasedBookCount,
                         total AS totalAmount
                         FROM `{bucketName}`
                         WHERE orderDate IS NOT NULL AND DATE_PART_STR(DATE_ADD_STR('1970-01-01T00:00:00Z', orderDate * 1000, 'millisecond'), 'year') = {year}
                        )
                        SELECT CASE month
                        WHEN '1' THEN 'January'
                        WHEN '2' THEN 'February'
                        WHEN '3' THEN 'March'
                        WHEN '4' THEN 'April'
                        WHEN '5' THEN 'May'
                        WHEN '06' THEN 'June'
                        WHEN '7' THEN 'July'
                        WHEN '8' THEN 'August'
                        WHEN '9' THEN 'September'
                        WHEN '10' THEN 'October'
                        WHEN '11' THEN 'November'
                        WHEN '12' THEN 'December'
                        ELSE 'Unknown'
                        END AS month,
                        SUM(totalAmount) AS monthlySum,
                        SUM(purchasedBookCount) AS totalPurchasedBooks,
                        COUNT(*) AS totalOrderCount
                        FROM filteredOrders
                        GROUP BY month;";

            try
            {
                _logger.LogInformation("Executing query: {Query}", query);

                var queryResult = await _cluster.QueryAsync<dynamic>(query);

                return await queryResult.Rows.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Query execution failed : {Query}", query);

                return default;
            }            
        }
    }
}
