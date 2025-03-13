using KWishes.API.Authentication;
using KWishes.API.Dto.Errors;
using KWishes.API.Dto.Misc;
using KWishes.API.Dto.Users;
using KWishes.Core.Application.Misc;
using KWishes.Core.Application.Users.Requests;
using KWishes.Core.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace KWishes.API.Controllers;

[Route("api/v1/users")]
public class UsersController : Controller
{
    private readonly ISender sender;

    public UsersController(ISender sender)
    {
        this.sender = sender;
    }

    [HttpPost("by-google-account")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> CreateUserByGoogleAccount()
    {
        var getResult = await sender.Send(new CreateUserByGoogleAccount.Request(User));
        if (getResult.TryGetError(out _, out var errorInfo))
            // TODO: return ApiErrorRegistry.ErrorInfo(errorInfo);
            return NoContent(); // hack for mvp, delete later

        return NoContent();
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<DtoUser>> GetUser(Guid id)
    {
        var result = await sender.Send(new GetUserById.Request(new UserId(id)));
        if (result.TryGetError(out var user, out var errorInfo))
            return ApiErrorRegistry.ErrorInfo(errorInfo);

        return DtoUserMapper.MapFrom(user);
    }
    
    [HttpPut("{id:guid}/role")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> SetRole(Guid id, [FromQuery, BindRequired] Role role)
    {
        if (User.GetKWishesUserRole() is not Role.Admin)
            return ApiErrorRegistry.Forbidden();

        var result = await sender.Send(new UpdateUser.Request(id, role));
        if (result.TryGetError(out _, out var errorInfo))
            return ApiErrorRegistry.ErrorInfo(errorInfo);

        return NoContent();
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<DtoItems<DtoUser>>> GetAll()
    {
        var result = await sender.Send(new GetAllUsers.Request());
        if (result.TryGetError(out var users, out var errorInfo))
            return ApiErrorRegistry.ErrorInfo(errorInfo);

        return new DtoItems<DtoUser>(users.Select(DtoUserMapper.MapFrom).ToList());
    }
    
    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<DtoUser>> GetMe()
    {
        var result = await sender.Send(new GetUserByGoogleNameId.Request(User.GetGoogleNameId()));
        if (result.TryGetError(out var user, out var errorInfo))
            return ApiErrorRegistry.ErrorInfo(errorInfo);

        return DtoUserMapper.MapFrom(user);
    }
}