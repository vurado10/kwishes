using Microsoft.AspNetCore.Mvc;

namespace KWishes.API.Controllers;

[Route("api/v1/auth")]
public class AuthenticationController : Controller
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status302Found)]
    public IActionResult SignInOrSignUp() => Redirect("/auth/callback");

    [HttpGet("callback")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult ExecuteCallback() => NoContent();
}