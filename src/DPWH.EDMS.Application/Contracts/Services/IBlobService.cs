using DPWH.EDMS.Domain.Entities.Storage;

namespace DPWH.EDMS.Application.Contracts.Services;

public interface IBlobService
{
    Task<BlobBinaryContent> GetBlobAsBinary(string container, string filename);
    Task<byte[]> Get(string container, string filename);
    Task Delete(string container, string filename);
    Task<string> Put(string container, string filename, Stream stream, string contentType, IDictionary<string, string>? metadata = null);
    Task<string> Put(string container, string filename, byte[] data, string contentType, IDictionary<string, string>? metadata = null);
    Task<dynamic> GetSasToken(string container);
}