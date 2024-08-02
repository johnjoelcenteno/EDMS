using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

public class AntiforgeryHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if(request.RequestUri.AbsolutePath.Contains("/api/", StringComparison.OrdinalIgnoreCase))
        {
            try
            {    
                ///Note: Inject the "/r" representing external api (as proxy)
                Uri myUri = request.RequestUri;
                UriBuilder builder = new(myUri);
                builder.Path = "r" + builder.Path;
                request.RequestUri = builder.Uri;

                // NOTE: We can acquire the access token here if we need

            }
            catch (AccessTokenNotAvailableException ex)
            {
                ex.Redirect();
            }            
            catch (Exception)
            {
                throw;
            }
        }
        request.Headers.Add("X-CSRF", "1");
        return base.SendAsync(request, cancellationToken);
    }
}