using DPWH.EDMS.Api.Contracts;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Shared.Services.Document
{
    public interface IDocumentService
    {
        string GetContentTypeFromFileName(string fileName);
        string GetExtensionValue(string contentType);
        //Task<List<AssetDocumentModelFileParameterable>> GetFilesToUpload(FileSelectEventArgs args, List<AssetDocumentModelFileParameterable> filesToUpload);
        //Task UploadFiles(Func<AssetDocumentModelFileParameterable, Task> cb, List<AssetDocumentModelFileParameterable> filesToUpload);
        Task<Stream> ConvertToStream(FileSelectFileInfo file);
        Task<FileParameter> GetFileToUpload(FileSelectEventArgs args, FileParameter fileParameter);
    }
}