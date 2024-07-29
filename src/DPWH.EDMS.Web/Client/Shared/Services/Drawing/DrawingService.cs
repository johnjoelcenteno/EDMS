using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DPWH.EDMS.Web.Client.Shared.Services.Drawing
{
    public class DrawingService
    {
        // The JavaScript namespace
        private const string JsNamespace = "telerikClientExporter";

        // IJSRuntime is resolved through DI
        public DrawingService(IJSRuntime jsRuntime) => JsRuntime = jsRuntime;

        private IJSRuntime JsRuntime { get; }

        /// <summary>
        /// Export an element from the page as a data image URI
        /// </summary>
        /// <param name="elementRef"></param>
        /// <returns>Data Image URI as a string</returns>
        public async ValueTask<string> ExportImage(ElementReference elementRef)
            => await JsRuntime.InvokeAsync<string>($"{JsNamespace}.exportImage", elementRef);

        /// <summary>
        /// Export an element from the page as a PDF formatted data URI.
        /// For options, see <a href="https://docs.telerik.com/kendo-ui/framework/drawing/pdf-output/pdf-options">telerik PDF Options</a>
        /// </summary>
        /// <param name="elementRef"></param>
        /// <param name="options"></param>
        /// <returns>Data URI as a string</returns>
        public async ValueTask<string> ExportPdf(ElementReference elementRef, object? options)
            => await JsRuntime.InvokeAsync<string>($"{JsNamespace}.exportPDF", elementRef, options);

        /// <summary>
        /// Invokes the browser to save a Data URI formatted string to a file.
        /// </summary>
        /// <param name="dataUri">Data URI string</param>
        /// <param name="fileName">File name to save as</param>
        public async Task SaveAs(string dataUri, string fileName)
            => await JsRuntime.InvokeVoidAsync($"{JsNamespace}.saveAs", dataUri, fileName);
        public async Task ResizeGraph()
          => await JsRuntime.InvokeVoidAsync($"{JsNamespace}.resizeGraph");
        public async Task countPageLegalElements()
          => await JsRuntime.InvokeVoidAsync($"{JsNamespace}.countPageLegalElements");
        public async Task removeDivWrap()
            => await JsRuntime.InvokeVoidAsync($"{JsNamespace}.removeDivWrap");
    }

}
