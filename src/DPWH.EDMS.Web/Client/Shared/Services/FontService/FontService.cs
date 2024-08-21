using DPWH.EDMS.Web.Client.Shared.Services.FontService.FontModel;
namespace DPWH.EDMS.Web.Client.Shared.Services.FontService;

public sealed class FontServices
{
    private readonly HttpClient _httpClient;

    public FontServices(HttpClient httpClient)
    {
        this._httpClient = httpClient;
    }

    public async Task<FontModels> LoadFonts()
    {
        FontModels fonts = new()
        {
            OpenSans = await GetFontData("OpenSans-Regular.ttf"),
            OpenSansBold = await GetFontData("OpenSans-Bold.ttf"),
            OpenSansBoldItalic = await GetFontData("OpenSans-BoldItalic.ttf"),
            OpenSansItalic = await GetFontData("OpenSans-Italic.ttf")
        };
        return fonts;
    }

    private async Task<byte[]> GetFontData(string name)
    {
        var sourceStream = await _httpClient.GetStreamAsync($"fonts/{name}");

        using MemoryStream memoryStream = new();

        sourceStream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}