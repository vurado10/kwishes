using System.Net.Mime;
using KWishes.API.Dto.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KWishes.API.Controllers;

[Authorize]
[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[ProducesResponseType(typeof(DtoError), StatusCodes.Status500InternalServerError)]
public abstract class Controller : ControllerBase
{
}