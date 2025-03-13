using KWishes.API.Authentication;
using KWishes.API.Dto.Errors;
using KWishes.API.Dto.Misc;
using KWishes.API.Dto.Wishes;
using KWishes.Core.Application.Votes.Requests;
using KWishes.Core.Application.Wishes.Requests;
using KWishes.Core.Domain;
using KWishes.Core.Domain.Users;
using KWishes.Core.Domain.Wishes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KWishes.API.Controllers;

[Route("api/v1/wishes")]
public class WishesController : Controller
{
    private readonly ISender sender;

    public WishesController(ISender sender)
    {
        this.sender = sender;
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> Create([FromBody] DtoWishCreateInfo info)
    {
        var wish = new Wish
        {
            ProductId = info.ProductId,
            CreatorId = User.GetKWishesUserId(),
            Text = info.Text
        };
        
        var createResult = await sender.Send(new CreateWish.Request(wish));
        if (createResult.TryGetError(out var createdWish, out var errorInfo))
            return ApiErrorRegistry.ErrorInfo(errorInfo);
        
        return createdWish.Id.Value;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<DtoWish>> GetWish(Guid id)
    {
        var result = await sender.Send(new GetWishById.Request(id));
        if (result.TryGetError(out var wish, out var errorInfo))
            return ApiErrorRegistry.ErrorInfo(errorInfo);
        
        var resultIsLikedByUser = await sender.Send(new IsVoteExist.Request(
            id,
            User.GetKWishesUserId(),
            VoteType.WishVote));
        if (resultIsLikedByUser.TryGetError(out var isLiked, out errorInfo))
            return ApiErrorRegistry.ErrorInfo(errorInfo);

        return DtoWishMapper.MapFrom(wish, isLiked);
    }
    
    [HttpPost("{id:guid}/process")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> StartProcessing(Guid id)
    {
        var role = User.GetKWishesUserRole();
        if (role is not Role.Admin && User.GetKWishesUserRole() is not Role.Moderator)
            return ApiErrorRegistry.Forbidden();
        
        var result = await sender.Send(new ChangeWishStatus.Request(id, WishStatus.Processing));
        if (result.TryGetError(out _, out var errorInfo))
            return ApiErrorRegistry.ErrorInfo(errorInfo);

        return NoContent();
    }
    
    [HttpPost("{id:guid}/complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Complete(Guid id)
    {
        var role = User.GetKWishesUserRole();
        if (role is not Role.Admin && User.GetKWishesUserRole() is not Role.Moderator)
            return ApiErrorRegistry.Forbidden();
        
        var result = await sender.Send(new ChangeWishStatus.Request(id, WishStatus.Completed));
        if (result.TryGetError(out _, out var errorInfo))
            return ApiErrorRegistry.ErrorInfo(errorInfo);

        return NoContent();
    }
    
    [HttpPost("{id:guid}/reject")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Reject(Guid id, [FromBody] DtoRejectCreateInfo createInfo)
    {
        var role = User.GetKWishesUserRole();
        if (role is not Role.Admin && User.GetKWishesUserRole() is not Role.Moderator)
            return ApiErrorRegistry.Forbidden();
        
        var result = await sender.Send(
            new RejectWish.Request(
                new WishId(id),
                User.GetKWishesUserId(),
                createInfo.Text));
        
        if (result.TryGetError(out _, out var errorInfo))
            return ApiErrorRegistry.ErrorInfo(errorInfo);
    
        return NoContent();
    }
    
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdateWish(Guid id, [FromBody] DtoWishUpdateInfo updateInfo)
    {
        var result = await sender.Send(new UpdateWish.Request(new WishId(id), updateInfo.Text));
        if (result.TryGetError(out _, out var errorInfo))
            return ApiErrorRegistry.ErrorInfo(errorInfo);

        return Ok();
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteWish(Guid id)
    {
        var result = await sender.Send(new DeleteWish.Request(id));
        if (result.TryGetError(out _, out var errorInfo))
            return ApiErrorRegistry.ErrorInfo(errorInfo);

        return NoContent();
    }

    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<DtoItems<DtoWish>>> Search([FromQuery] DtoSearchWishesQuery query)
    {
        var result = await sender.Send(new SearchWish.Request(
            query.ProductId, 
            query.ByCurrentUser ? new UserId(User.GetKWishesUserId()) : null,
            query.Statuses?.ToHashSet() ?? new HashSet<WishStatus>(),
            query.Text,
            query.CreatedAtPeriod?.From, 
            query.CreatedAtPeriod?.To,
            query.Page.Skip,
            query.Page.Take));
        
        if (result.TryGetError(out var wishes, out var errorInfo))
            return ApiErrorRegistry.ErrorInfo(errorInfo);
        var res = new List<DtoWish>();
        var userId = User.GetKWishesUserId();
        foreach (var wish in wishes)
        {
            var resultIsLikedByUser = await sender.Send(new IsVoteExist.Request(
                wish.Id,
                userId,
                VoteType.WishVote));
            if (resultIsLikedByUser.TryGetError(out var isLiked, out errorInfo))
                return ApiErrorRegistry.ErrorInfo(errorInfo);
            res.Add(DtoWishMapper.MapFrom(wish, isLiked));
        }

        return new DtoItems<DtoWish>(res);
    }
}