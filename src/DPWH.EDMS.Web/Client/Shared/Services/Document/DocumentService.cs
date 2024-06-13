using DPWH.EDMS.Api.Contracts;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Shared.Services.Document
{
    public class DocumentService : IDocumentService
    {
        //public async Task<List<AssetDocumentModelFileParameterable>> GetFilesToUpload(FileSelectEventArgs args, List<AssetDocumentModelFileParameterable> filesToUpload)
        //{
        //    if (filesToUpload == null)
        //    {
        //        filesToUpload = new List<AssetDocumentModelFileParameterable>();
        //    }

        //    foreach (var file in args.Files)
        //    {
        //        var clonedStream = await ConvertToStream(file);

        //        var fileParam = new FileParameter(clonedStream, file.Name, GetContentTypeFromFileName(file.Extension));
        //        filesToUpload.Add(new() { Document = default!, File = fileParam });
        //    }

        //    return filesToUpload;
        //}

        public async Task<FileParameter> GetFileToUpload(FileSelectEventArgs args, FileParameter fileParameter)
        {
            var fileParam = new FileParameter(null);

            foreach (var file in args.Files)
            {

                var clonedStream = await ConvertToStream(file);

                fileParam = new FileParameter(clonedStream, file.Name, GetContentTypeFromFileName(file.Extension));
            }

            return fileParam;
        }

        public async Task<Stream> ConvertToStream(FileSelectFileInfo file)
        {
            if (file == null || file.Size == 0)
            {
                // Handle null or empty file
                throw new ArgumentException("File is null or empty.");
            }

            // Create a MemoryStream to hold the file data
            var stream = new MemoryStream();

            try
            {
                // Open the file stream and copy its content to the MemoryStream
                await file.Stream.CopyToAsync(stream);

                // Reset the position of the MemoryStream to the beginning
                stream.Seek(0, SeekOrigin.Begin);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the CopyToAsync operation
                // You can log the error or throw a custom exception here as needed
                throw new Exception("Failed to read file stream.", ex);
            }

            // Return the MemoryStream containing the file data
            return stream;
        }


        //public async Task UploadFiles(Func<AssetDocumentModelFileParameterable, Task> cb, List<AssetDocumentModelFileParameterable> filesToUpload)
        //{
        //    try
        //    {
        //        var tasks = filesToUpload
        //            .Where(tuple => tuple.File.Data != null && !string.IsNullOrEmpty(tuple.File.FileName))
        //            .Select(async res => await cb.Invoke(res));

        //        await Task.WhenAll(tasks);

        //    }
        //    catch (ApiException e)
        //    {
        //        if (e.StatusCode != 200)
        //            Console.WriteLine($"API Exception on _AssetService.AddAssetDocument with Status code [{e.StatusCode}]");
        //    }
        //    catch (Exception e)
        //    {
        //        // Handle the custom exception and log the error or show a user-friendly error message.
        //        Console.WriteLine("Unexpected API Response Error On: _AssetService.AddAssetDocument");
        //    }
        //}

        public string GetContentTypeFromFileName(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            switch (extension)
            {
                case ".txt":
                    return "text/plain";
                case ".pdf":
                    return "application/pdf";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png"; // The content type for .png files
                                        // Add more cases as needed for other file types
                default:
                    return "application/octet-stream"; // Default content type for unknown files
            }
        }

        public string GetExtensionValue(string contentType)
        {
            switch (contentType)
            {
                case "text/plain":
                    return ".txt";
                case ".pdf": 
                case "application/pdf":
                    return ".pdf";
                case "image/jpeg":
                    return ".jpg"; // You can choose either .jpg or .jpeg based on your preference
                case "image/png":
                case ".png":
                    return ".png";
                // Add more cases as needed for other content types
                default:
                    return string.Empty; // If the provided content type is unknown, return null
            }
        }
    }
}
