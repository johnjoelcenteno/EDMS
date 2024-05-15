using Azure;
using Azure.Data.Tables;

namespace DPWH.EDMS.Infrastructure.Storage.Services;

public class TableService : ITableService
{
    public const string RowKey = "RowKey";
    public const string PartitionKey = "PartitionKey";
    public const string Timestamp = "Timestamp";
    private readonly TableServiceClient _tableServiceClient;

    public TableService(string tableStorageConnectionString)
    {
        _tableServiceClient = new TableServiceClient(tableStorageConnectionString);
    }

    private TableClient GetTable(string tableName)
    {
        var tables = _tableServiceClient.Query(x => x.Name == tableName).ToList();
        if (!tables.Any())
        {
            _tableServiceClient.CreateTable(tableName);
        }

        var tableClient = _tableServiceClient.GetTableClient(tableName);
        return tableClient;
    }
    public async Task InsertAsync<T>(string tableName, T entity) where T : class, ITableEntity, new()
    {
        var tableClient = GetTable(tableName);
        await tableClient.AddEntityAsync(entity);
    }
    public async Task SetAsync<T>(string tableName, T entity) where T : class, ITableEntity, new()
    {
        var tableClient = GetTable(tableName);

        var result = await tableClient.UpsertEntityAsync(entity: entity, TableUpdateMode.Merge);
    }

    public async Task UpSertAsync<T>(string tableName, IEnumerable<T> entities) where T : class, ITableEntity, new()
    {
        var tableClient = GetTable(tableName);

        List<TableTransactionAction> batch = new List<TableTransactionAction>();

        // Add the entities to be added to the batch.
        batch.AddRange(entities.Select(e => new TableTransactionAction(TableTransactionActionType.Add, e)));

        // Execute the transaction.
        var batchResult = await tableClient.SubmitTransactionAsync(batch).ConfigureAwait(false);
    }
    public async Task<IEnumerable<T>> GetAsync<T>(string tableName, string partitionKey, DateTimeOffset? timestampOnOrBefore = null) where T : class, ITableEntity, new()
    {
        var tableClient = GetTable(tableName);

        var queryResults = tableClient.Query<T>(x => x.PartitionKey == partitionKey);

        var result = new List<T>();

        foreach (var item in queryResults.ToList())
        {
            result.Add(item);
        }

        return result;
    }

    public async Task<T> Get<T>(string tableName, string partitionKey, string key) where T : class, ITableEntity, new()
    {
        var tableClient = GetTable(tableName);
        var result = await tableClient.GetEntityAsync<T>(partitionKey, rowKey: key);
        return result.Value;
    }

    public async Task DeleteAsync<T>(string tableName, string partitionKey, string rowKey) where T : class, ITableEntity, new()
    {
        var tableClient = GetTable(tableName);

        await tableClient.DeleteEntityAsync(partitionKey, rowKey);
    }
    public async Task BatchDeleteAsync<T>(string tableName, string filter) where T : class, ITableEntity, new()
    {
        var tableClient = GetTable(tableName);

        AsyncPageable<T> entities = tableClient.QueryAsync<T>(filter: filter, maxPerPage: 1000, select: new List<string>() { "PartitionKey", "RowKey" });

        await entities.AsPages().ForEachAwaitAsync(async page =>
        {
            // Since we don't know how many rows the table has and the results are ordered by PartitonKey+RowKey
            // we'll delete each page immediately and not cache the whole table in memory
            await BatchManipulateEntities(tableClient, page.Values, TableTransactionActionType.Delete).ConfigureAwait(false);
        });

    }

    /// <summary>
    /// Groups entities by PartitionKey into batches of max 100 for valid transactions
    /// </summary>
    /// <returns>List of Azure Responses for Transactions</returns>
    private async Task<List<Response<IReadOnlyList<Response>>>> BatchManipulateEntities<T>(TableClient tableClient, IEnumerable<T> entities, TableTransactionActionType tableTransactionActionType) where T : class, ITableEntity, new()
    {
        var groups = entities.GroupBy(x => x.PartitionKey);
        var responses = new List<Response<IReadOnlyList<Response>>>();
        foreach (var group in groups)
        {
            List<TableTransactionAction> actions;
            var items = group.AsEnumerable();
            while (items.Any())
            {
                var batch = items.Take(100);
                items = items.Skip(100);

                actions = new List<TableTransactionAction>();
                actions.AddRange(batch.Select(e => new TableTransactionAction(tableTransactionActionType, e)));
                var response = await tableClient.SubmitTransactionAsync(actions).ConfigureAwait(false);
                responses.Add(response);
            }
        }
        return responses;
    }
}

public interface ITableService
{
    Task InsertAsync<T>(string tableName, T entity) where T : class, ITableEntity, new();
    Task SetAsync<T>(string tableName, T entity) where T : class, ITableEntity, new();
    Task<IEnumerable<T>> GetAsync<T>(string tableName, string partitionKey, DateTimeOffset? timestampOnOrBefore = null) where T : class, ITableEntity, new();
    Task<T> Get<T>(string tableName, string partitionKey, string key) where T : class, ITableEntity, new();
    Task DeleteAsync<T>(string tableName, string partitionKey, string rowKey) where T : class, ITableEntity, new();
    Task BatchDeleteAsync<T>(string tableName, string filter) where T : class, ITableEntity, new();
    Task UpSertAsync<T>(string tableName, IEnumerable<T> entities) where T : class, ITableEntity, new();
}
