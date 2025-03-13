using KWishes.API.Authentication;
using KWishes.API.Dto.Errors;
using KWishes.Core.Application.Votes.Requests;
using KWishes.Core.Application.Wishes.Requests;
using KWishes.Core.Domain;
using KWishes.Core.Domain.Votes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KWishes.API.Controllers;

[Route("api/v1")]
public class VotesController : Controller
{
    private readonly ISender sender;

    public VotesController(ISender sender)
    {
        this.sender = sender;
    }
    
    [HttpPost("wishes/{id:guid}/vote")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> PostWishVote(Guid id)
    {
        var searchResult = await sender.Send(new CreateWishVote.Request(
            new Vote
            {
                EntityId = id,
                CreatorId = User.GetKWishesUserId(),
                Type = VoteType.WishVote
            }
        ));
        
        if (searchResult.TryGetError(out var vote, out var errorInfo))
            return ApiErrorRegistry.ErrorInfo(errorInfo);
        
        return Ok();
    }
    
    [HttpDelete("wishes/{id:guid}/vote")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteWishVote(Guid id)
    {
        var searchResult = await sender.Send(new DeleteWishVote.Request(
            new Vote
            {
                EntityId = id,
                CreatorId = User.GetKWishesUserId(),
                Type = VoteType.WishVote
            }
        ));
        
        if (searchResult.TryGetError(out var vote, out var errorInfo))
            return ApiErrorRegistry.ErrorInfo(errorInfo);
        
        return Ok();
    }
    
    [HttpPost("comments/{id:guid}/vote")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> PostCommentVote(Guid id)
    {
        var searchResult = await sender.Send(new CreateCommentVote.Request(
            new Vote
            {
                EntityId = id,
                CreatorId = User.GetKWishesUserId(),
                Type = VoteType.CommentVote
            }
        ));
        
        if (searchResult.TryGetError(out var vote, out var errorInfo))
            return ApiErrorRegistry.ErrorInfo(errorInfo);
        
        return Ok();
    }
    
    [HttpDelete("comments/{id:guid}/vote")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteCommentVote(Guid id)
    {
        var searchResult = await sender.Send(new DeleteCommentVote.Request(
            new Vote
            {
                EntityId = id,
                CreatorId = User.GetKWishesUserId(),
                Type = VoteType.CommentVote
            }
        ));
        
        if (searchResult.TryGetError(out var vote, out var errorInfo))
            return ApiErrorRegistry.ErrorInfo(errorInfo);
        
        return Ok();
    }
    
}