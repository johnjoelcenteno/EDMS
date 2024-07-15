namespace DPWH.EDMS.Web.Client.BFF;

using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

public class AccessTokenHandler : DelegatingHandler
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navManager;
    private readonly string _accessTokenEndpoint = "AccessToken";

    public AccessTokenHandler(
        AuthenticationStateProvider authenticationStateProvider, HttpClient httpClient, NavigationManager navManager )
    {
        _authenticationStateProvider = authenticationStateProvider;
        _navManager = navManager;
        _httpClient = httpClient;
    }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        try
        {
            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                var tokenRes = await _httpClient.GetFromJsonAsync<string>(_accessTokenEndpoint);
                if (!string.IsNullOrEmpty(tokenRes))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenRes);
                }
            }
            else
            {
                _navManager.NavigateTo("/bff/login", true);
            }
        }
        catch (Exception ex)
        {
            _navManager.NavigateTo("/bff/login", true);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}

