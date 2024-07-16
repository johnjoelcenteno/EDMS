using Duende.AccessTokenManagement.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Web.Server.Controllers;

/// <summary>
/// Controller to handle access token retrieval.
/// </summary>
[ApiController]
[Route("[controller]")]
public class AccessTokenController : ControllerBase
{
    private readonly ILogger<AccessTokenController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccessTokenController"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public AccessTokenController(ILogger<AccessTokenController> logger) => _logger = logger;

    /// <summary>
    /// Gets the user access token.
    /// </summary>
    /// <returns>The user access token as JSON.</returns>
    [HttpGet]
    public async Task<IActionResult?> GetToken()
    {
        try
        {
            var token = await HttpContext.GetUserAccessTokenAsync();
            //token.Expiration = DateTime.UtcNow.AddHours(-1); // TEST EXPIRATION

            return _IsTokenValid(token) ? new JsonResult(token.AccessToken) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving the access token.");
            return StatusCode(500, "An error occurred while retrieving the access token.");
        }
    }

    private bool _IsTokenValid(UserToken token)
    {
        return !token.IsError && DateTimeOffset.UtcNow < token.Expiration.UtcDateTime;
    }
}
