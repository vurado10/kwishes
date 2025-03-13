using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KWishes.API.Controllers;

[Route("api/v1/debug")]
public class DebugController : Controller
{
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<string> Ping() => "Pong";

    [HttpGet("user-claims")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetUsers()
    {
        return Ok(User.Claims.Select(claim => $"{claim.Type} : {claim.ValueType} : {claim.Value}"));
    }
}