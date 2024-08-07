using Application.Models;
using Application.Persistence;
using Couchbase;
using Domain.Entities;
using Google.Api;
using System.Drawing;

namespace Persistence.Repository
{
    public class CouchbaseRepository<T> : IRepository<T>
    {
        private readonly ICluster _cluster;

        public CouchbaseRepository(ICluster cluster)
        {
            _cluster = cluster;
        }

        private async Task<IBucket> GetBucketAsync(string bucketName)
        {
            return await _cluster.BucketAsync(bucketName);
        }

        public async Task<string> CreateAsync(string bucketName, string id, T item)
        {
            var bucket = await GetBucketAsync(bucketName);
            var collection = bucket.DefaultCollection();
            var result = await collection.UpsertAsync(id, item);

            return result.MutationToken.BucketRef;
        }

        public async Task<T> ReadAsync(string bucketName, string id)
        {
            var bucket = await GetBucketAsync(bucketName);
            var collection = bucket.DefaultCollection();
            var result = await collection.GetAsync(id);
            return result.ContentAs<T>();
        }

        public async Task<bool> UpdateAsync(string bucketName, string id, T item)
        {
            var bucket = await GetBucketAsync(bucketName);
            var collection = bucket.DefaultCollection();
            var result = await collection.ReplaceAsync(id, item);
            return result.Cas != 0;
        }

        public async Task<T> GetByIdAsync(string bucketName, string fieldName, string value)
        {
            var query = $"SELECT COUNT(*) AS count FROM `{bucketName}` WHERE `{fieldName}` = \"{value}\";";
            var result = await _cluster.QueryAsync<bool>(query);
            var row = await result.AnyAsync();
          //  var count = row?.count ?? 0;
           // return count > 0;


            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync(string bucketName, string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsAsync(string bucketName, string fieldName, string value)
        {
            var query = $"SELECT COUNT(*) AS count FROM `{bucketName}` WHERE `{fieldName}` = \"{value}\";";
            var result = await _cluster.QueryAsync<dynamic>(query);
            var row = await result.Rows.FirstOrDefaultAsync();

            var count = row?.count ?? 0;
            return count > 0;
        }

        public async Task<List<dynamic>> GetWithPagination(string bucketName, string fieldName, string value, int pageNumber, int pageSize)
        {
            var offset = (pageNumber - 1) * pageSize;
            var query = $"SELECT * FROM `{bucketName}` WHERE {fieldName} = \"{value}\"  LIMIT {pageSize} OFFSET {offset}";

            var queryResult = await _cluster.QueryAsync<dynamic>(query);

            return await queryResult.Rows.ToListAsync();
        }
    }
}
