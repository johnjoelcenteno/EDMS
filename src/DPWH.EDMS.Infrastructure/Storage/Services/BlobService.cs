using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using DPWH.EDMS.Application.Contracts.Services;
using DPWH.EDMS.Domain.Entities.Storage;

namespace DPWH.EDMS.Infrastructure.Storage.Services;

public class BlobService : IBlobService
{

    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _blobStorageConnectionString;
    private readonly Dictionary<string, dynamic> _tokensByContainer;

    public BlobService(string blobStorageConnectionString)
    {
        _blobStorageConnectionString = blobStorageConnectionString;
        _tokensByContainer = new Dictionary<string, dynamic>();
        _blobServiceClient = new BlobServiceClient(blobStorageConnectionString);
    }

    async Task<BlobContainerClient> GetCloudBlobContainer(string container)
    {
        var cloudBlobContainer = _blobServiceClient.GetBlobContainerClient(container);

        await cloudBlobContainer.CreateIfNotExistsAsync(PublicAccessType.Blob);

        return cloudBlobContainer;
    }

    public async Task<byte[]> Get(string container, string filename)
    {
        var blobContainer = await GetCloudBlobContainer(container);
        var blob = blobContainer.GetBlobClient(filename);

        var responseStream = new MemoryStream();
        await blob.DownloadToAsync(responseStream);

        responseStream.Seek(0, SeekOrigin.Begin);
        return responseStream.ToArray();
    }

    public async Task Delete(string container, string filename)
    {
        var blobContainer = await GetCloudBlobContainer(container);
        var blob = blobContainer.GetBlobClient(filename);

        await blob.DeleteIfExistsAsync();
    }

    public async Task<string> Put(
        string container,
        string filename,
        Stream stream,
        string contentType,
        IDictionary<string, string>? metadata = null)
    {
        var blobContainer = await GetCloudBlobContainer(container);
        var blob = blobContainer.GetBlobClient(filename);
        await blob.UploadAsync(stream, overwrite: true);

        await blob.SetHttpHeadersAsync(new BlobHttpHeaders { ContentType = contentType });

        if (metadata is not null)
        {
            await blob.SetMetadataAsync(metadata);
        }

        return blob.Uri.ToString();
    }

    public async Task<string> Put(string container, string filename, byte[] data, string contentType, IDictionary<string, string>? metadata = null)
    {
        var uri = await Put(container, filename, new MemoryStream(data), contentType, metadata);
        return uri;
    }

    public async Task<BlobClient> GetBlobReference(string container, string filename)
    {
        var blobContainer = await GetCloudBlobContainer(container);
        var blob = blobContainer.GetBlobClient(filename);
        return blob;
    }

    public async Task<BlobBinaryContent> GetBlobAsBinary(string containerName, string blobName)
    {
        var blobClient = await GetBlobReference(containerName, blobName);
        if (!blobClient.ExistsAsync().Result)
        {
            return new BlobBinaryContent(null, null);
        }

        using var ms = new MemoryStream();
        blobClient.DownloadTo(ms);

        return new BlobBinaryContent(ms.ToArray(), blobClient.GetProperties().Value.ContentType);
    }

    public async Task<dynamic> GetSasToken(string container)
    {
        return await GetContainerSasUri(container);
    }

    public dynamic? PublicToken { get; set; }

    /// <summary>
    /// Get the shared access token from BlobStorage
    /// </summary>
    private async Task<dynamic> GetContainerSasUri(string container)
    {
        if (PublicToken?.ExpiresAt < DateTime.UtcNow)
        {
            var blobContainer = await GetCloudBlobContainer(container);
            BlobSasBuilder sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = blobContainer.Name,
                Resource = "c",
                ExpiresOn = DateTimeOffset.UtcNow.AddDays(2)
            };
            sasBuilder.SetPermissions(BlobAccountSasPermissions.Read);

            var uri = blobContainer.GenerateSasUri(sasBuilder);
            var sasContainerToken = uri.Query;

            PublicToken = new System.Dynamic.ExpandoObject();
            PublicToken.SasContainerToken = sasContainerToken;
            PublicToken.ExpiresAt = DateTime.UtcNow.AddDays(1);
        }

        return PublicToken;
    }
}