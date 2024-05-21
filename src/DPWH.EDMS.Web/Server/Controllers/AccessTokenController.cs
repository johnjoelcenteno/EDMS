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
    public async Task<IActionResult> GetToken()
    {
        try
        {
            var token = await HttpContext.GetUserAccessTokenAsync();
            return new JsonResult(token.AccessToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving the access token.");
            return StatusCode(500, "An error occurred while retrieving the access token.");
        }
    }
}
