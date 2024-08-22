using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequestSupportingFiles;
using DPWH.EDMS.Client.Shared.APIClient.Services.RequestManagement;
using DPWH.EDMS.Client.Shared.Models;
 
using DPWH.EDMS.Web.Client.Shared.Services.Document;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler; 
using Microsoft.AspNetCore.Components; 
using Telerik.Blazor.Components; 
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Drawing; 
using PdfSharpCore.Fonts;
using DPWH.EDMS.Web.Client.Shared.Services.FontService; 
using DPWH.EDMS.Web.Client.Shared.Services.FontService.FontModel; 
using MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes;
using PdfSharpCore.Utils;
using SixLabors.ImageSharp.PixelFormats;
using QRCoder;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp;
using DPWH.EDMS.Shared.Enums;
using DPWH.NGOBIA.Client.Shared.APIClient.Services.Users;
using DPWH.EDMS.Components.Helpers;
using System.Net;

namespace DPWH.EDMS.Web.Client.Pages.RequestManagement.ViewRequestedRecordForm;

public class ViewRequestedRecordFormBase : ComponentBase
{
    [Parameter] public required string RequestId { get; set; }
    [Parameter] public required string DocumentId { get; set; }
    [Inject] public required IRequestManagementService RequestManagementService { get; set; }
    [Inject] public required IToastService ToastService { get; set; }
    [Inject] public required IExceptionHandlerService ExceptionHandlerService { get; set; }
    [Inject] public required IRecordRequestSupportingFilesService RecordRequestSupportingFilesService { get; set; }
    [Inject] public required IDocumentService DocumentService { get; set; }
    [Inject] public required IUsersService UserService { get; set; }
    [Inject] public required FontServices FontService { get; set; }
    protected RecordRequestModel SelectedRecordRequest { get; set; } = new();
    protected RequestedRecordModel RequestedRecord { get; set; } = new();
    [Inject] public required NavigationManager NavManager { get; set; }
    protected TelerikDialog dialogReference = new();
    protected bool IsOpen { get; set; } = false;
    protected bool IsSigning { get; set; } = false;

    protected string CancelReturnUrl = string.Empty;
    protected bool IsLoading { get; set; } 
    protected byte[] dataByte { get; set; }
    [Inject] public HttpClient Http { get; set; }
    protected bool IsSignatureOpen { get; set; } = false;
    protected string SignedValue { get; set; }
    protected string QRCodeValue = "";
    public string Status { get; set; } = "Initializing...";
    protected string SignedUrl { get; set; }
    public List<BreadcrumbModel> BreadcrumbItems { get; set; } = new()
    {
         new() { Icon = "home", Url = "/"},
    };
    protected bool XSmall { get; set; }
    protected async override Task OnInitializedAsync()
    {
        await LoadSelectedDocuments();

        BreadcrumbItems.AddRange(new List<BreadcrumbModel>
            {
                new BreadcrumbModel
                {
                    Icon = "menu",
                    Text = "Request Management",
                    Url = "/request-management",
                },
                new BreadcrumbModel
                {
                    Icon = "create_new_folder",
                    Text = "View Request Form",
                    Url = $"/request-management/view-request-form/{RequestId}",
                },
                new BreadcrumbModel
                {
                    Icon = "create_new_folder",
                    Text = $"{RequestedRecord.RecordType}",
                    Url = NavManager.Uri.ToString(),
                },
            });
    }
    protected async Task LoadSelectedDocuments()
    {
        IsLoading = true;

        await LoadData(async (res) =>
        {
            SelectedRecordRequest = res;
            if (SelectedRecordRequest != null)
            {
                LoadDocument(SelectedRecordRequest);

            }
        });

        IsLoading = false;
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
            return;
        
        FontModels font = await FontService.LoadFonts();

        try
        {
            //if (GlobalFontSettings.FontResolver is not CustomFontResolver)
            //{
            //if (GlobalFontSettings.FontResolver  == null)
            //{
            GlobalFontSettings.FontResolver = new CustomFontResolver(font);
            //}
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message.ToString());
        }
    }
    protected void OpenSignature()
    {
        IsOpen = !IsOpen;
        //NavManager.NavigateTo($"/request-management/requested-record/signature/{RequestId}/{DocumentId}");
    }
    async Task<byte[]> GetImage(string imageSource)
    {
        using var response = await Http.GetAsync(imageSource);
        response.EnsureSuccessStatusCode();
        Stream ms = await response.Content.ReadAsStreamAsync();
        byte[] byteArray;
        using (MemoryStream memoryStream = new())
        {
            ms.CopyTo(memoryStream);

            byteArray = memoryStream.ToArray();
        }
        return byteArray;
    }

    public byte[] QRImageDownloader(string Uri)
    {
        if (!string.IsNullOrEmpty(Uri))
        {
            QRCodeGenerator qrCodeGenerate = new();
            QRCodeData qrCodeData = qrCodeGenerate.CreateQrCode(Uri, QRCodeGenerator.ECCLevel.M);

            PngByteQRCode qrCode = new(qrCodeData);
            byte[] pngBytes = qrCode.GetGraphic(20);

            using MemoryStream pngStream = new(pngBytes);
            using Image<Rgba32> image = Image.Load<Rgba32>(pngStream);

            using MemoryStream jpegStream = new();
            image.Save(jpegStream, new JpegEncoder());

            return jpegStream.ToArray();
        }

        return Array.Empty<byte>();
    }
    //public void GenerateQRCode(string Uri)
    //{
    //    if (!string.IsNullOrEmpty(Uri))
    //    {
    //        QRCodeGenerator qrCodeGenerate = new();
    //        QRCodeData qrCodeData = qrCodeGenerate.CreateQrCode(Uri, QRCodeGenerator.ECCLevel.M);

    //        PngByteQRCode qrCode = new(qrCodeData);
    //        byte[] pngBytes = qrCode.GetGraphic(20);

    //        using MemoryStream pngStream = new(pngBytes);
    //        using Image<Rgba32> image = Image.Load<Rgba32>(pngStream);

    //        using MemoryStream jpegStream = new();
    //        image.Save(jpegStream, new JpegEncoder());

    //        byte[] jpegBytes = jpegStream.ToArray();
    //        //string base64 = Convert.ToBase64String(jpegBytes);
    //        //QRCodeValue = string.Format("data:image/jpeg;base64,{0}", base64);
    //    }
    //}

    public async Task GenerateStamp(string pdfUri, string relativeOutputFilePath, string QRUrl)
    {
       
        IsSigning = true;
        IsLoading = true;

        if (string.IsNullOrEmpty(SignedUrl)) 
        {
            IsSigning = false;
            IsLoading = false;

            ToastService.ShowError("Please ensure your signature file is uploaded to your profile page.");
            return;
        }
        try
        {
            Status = "Modifying PDF...";
            
            using var httpClient = new HttpClient();
            string cacheBusterUri = $"{pdfUri}?timestamp={DateTime.UtcNow.Ticks}";

            // Download the PDF from the URI
            byte[] pdfData = await httpClient.GetByteArrayAsync(cacheBusterUri);
            Status = "Processing PDF...";
            

            using MemoryStream pdfStream = new MemoryStream(pdfData);
            PdfDocument document = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Modify, PdfSharpCore.Pdf.IO.enums.PdfReadAccuracy.Moderate);
            
            Status = "Generating stamp...";
          
            // Generate the stamp content only once
            byte[] qrImageData = QRImageDownloader(pdfUri);
            XImage qrImage = CreateXImageFromByteArray(qrImageData, "qr_code.png");

            //byte[] signatureImageData = await GetImage("_content/DPWH.EDMS.Components/images/signaturePNG.png");
            byte[] signatureImageData = await GetImage(SignedUrl);
            XImage signatureImage = CreateXImageFromByteArray(signatureImageData, "signature.png");

            XFont font = new("Arial", 9, XFontStyle.Bold);

            Status = "Applying stamp to PDF pages...";
            
            // Iterate over all pages in the PDF and apply the stamp
            foreach (PdfPage page in document.Pages)
            {
                using XGraphics gfx = XGraphics.FromPdfPage(page);
                ApplyStampToPage(gfx, page, qrImage, signatureImage, font);
            }

            Status = "Saving stamped PDF...";
            

            using MemoryStream outputStream = new MemoryStream();
            document.Save(outputStream);
            byte[] outputData = outputStream.ToArray();

            Status = "Uploading stamped PDF...";
            

            FileParameter file = new FileParameter(
                new MemoryStream(outputData, writable: false),
                "Signed.pdf",
                DocumentService.GetContentTypeFromFileName("Signed.pdf")
            );

            if (file != null && RequestedRecord != null && RequestedRecord.DocumentType != null)
            {
                await RecordRequestSupportingFilesService.UploadRequestedRecord(file, Guid.Parse(DocumentId), RequestedRecord.DocumentType);
                var documentStatus = new UpdateRecordsRequestDocumentStatus
                {
                    Id = Guid.Parse(DocumentId),
                    Status = RequestedRecordStatus.Completed.ToString()
                };
                await RecordRequestSupportingFilesService.UpdateRecordStatus(documentStatus);
            }
            StateHasChanged();
            await LoadSelectedDocuments();
        }
        catch (Exception ex)
        {
            ToastService.ShowError($"Unable to sign corrupted PDF: {ex.Message}");
            ToastService.ShowError($"Scan and re-upload again the Document: {ex.Message}");
        }
        finally
        {
            IsSigning = false;
            IsLoading = false;
            Status = "Process completed successfully.";

            ToastService.ShowSuccess(Status);
            StateHasChanged();
        }
    }

    private XImage CreateXImageFromByteArray(byte[] imageData, string imageName)
    {
        try
        {
            return XImage.FromStream(() => new MemoryStream(imageData));
        }
        catch (UnknownImageFormatException ex)
        {
            throw new Exception($"Failed to load image '{imageName}': {ex.Message}", ex);
        }
    }

    private void ApplyStampToPage(XGraphics gfx, PdfPage page, XImage qrImage, XImage signatureImage, XFont font)
    {
        double qrImageWidth = 55;
        double qrImageHeight = qrImage.PixelHeight * (qrImageWidth / qrImage.PixelWidth);
        double padding = 6;
        double bottomMargin = 78;
        double pageHeight = gfx.PageSize.Height;
        double qrImageX = padding;
        double qrImageY = pageHeight - qrImageHeight - padding - bottomMargin;

        gfx.Save();
        gfx.TranslateTransform(qrImageX, qrImageY);
        gfx.RotateTransform(-90);

        double positionY = 50;
        gfx.DrawString("Department of Public Works and Highways", font, XBrushes.DarkGreen, new XRect(26, 0, qrImageHeight, positionY), XStringFormats.TopLeft);
        positionY += 10;

        gfx.TranslateTransform(0, 8);

        if (RequestedRecord.DocumentType == "TC")
        {
            gfx.DrawString($"Certified True Copy", font, XBrushes.DarkGreen, new XRect(78, 0, qrImageHeight, positionY), XStringFormats.TopLeft);
        }
        if (RequestedRecord.DocumentType == "MC")
        {
            gfx.DrawString($"Certified Machine Copy from the Record on File", font, XBrushes.DarkGreen, new XRect(18, 0, qrImageHeight, positionY), XStringFormats.TopLeft);
        }

        gfx.TranslateTransform(0, 38);

        double textX = 75;
        double textY = -12;
        double textHeight = 30;

        gfx.DrawString("Ma. Kathrina B. Abary", font, XBrushes.DarkGreen, new XRect(textX, textY, qrImageHeight, textHeight), XStringFormats.TopLeft);
        gfx.DrawString("Non-Current Record Section", font, XBrushes.DarkGreen, new XRect(55, -5, qrImageHeight, textHeight), XStringFormats.TopLeft);
        gfx.Restore();

        gfx.DrawImage(qrImage, qrImageX, qrImageY, qrImageWidth, qrImageHeight);

        textY = pageHeight - bottomMargin - qrImageHeight - 100;
        double signatureWidth = 50;
        double signatureImageHeight = signatureImage.PixelHeight * (signatureWidth / signatureImage.PixelWidth);
        double signatureX = -40 + qrImageWidth;
        double signatureY = textY - signatureImageHeight;

        gfx.Save();
        gfx.TranslateTransform(signatureX + signatureWidth / 2, signatureY + signatureImageHeight / 2);
        gfx.RotateTransform(-120);
        gfx.TranslateTransform(-(signatureX + signatureWidth / 2), -(signatureY + signatureImageHeight / 2));

        gfx.DrawImage(signatureImage, signatureX, signatureY, signatureWidth, signatureImageHeight);

        gfx.Restore();
    }
     

    protected async Task<byte[]> ConvertUrlToByte(string pdfUrl)
    {
        using (var httpClient = new HttpClient())
        {
            var pdfBytes = await httpClient.GetByteArrayAsync(pdfUrl);

            return pdfBytes;
        }
    }
    protected async void LoadDocument(RecordRequestModel data)
    {
        var guid = Guid.Parse(DocumentId);
        var document = data.RequestedRecords.FirstOrDefault(x => x.Id == guid);
        if (document != null)
        {
            RequestedRecord = document;

        }
        if (string.IsNullOrEmpty(RequestedRecord.Uri))
        {
            NavManager.NavigateTo("/404");
        }

        StateHasChanged();
    } 
    protected async Task LoadData(Action<RecordRequestModel> onLoadCb)
    {
        await ExceptionHandlerService.HandleApiException(async () => {
            var recordReq = await RequestManagementService.GetById(Guid.Parse(RequestId));

            if (recordReq.Success)
            {
                if (onLoadCb != null)
                {
                    onLoadCb.Invoke(recordReq.Data);
                }
            }
            else
            {
                ToastService.ShowError("Something went wrong on loading record request.");
                NavManager.NavigateTo(CancelReturnUrl);
            }

            try
            {
                var signedUri = await UserService.GetUserSignature();
                if (signedUri.Data != null)
                {
                    SignedUrl = signedUri.Data.UriSignature;
                }
            }
            catch (Exception)
            {
                ToastService.ShowError("Please ensure your signature file is uploaded to your profile page.");
            }
        });
    }
}