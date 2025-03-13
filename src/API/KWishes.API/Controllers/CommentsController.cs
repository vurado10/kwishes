using KWishes.API.Authentication;
using KWishes.API.Dto.Comments;
using KWishes.API.Dto.Errors;
using KWishes.API.Dto.Misc;
using KWishes.Core.Application.Comments.Requests;
using KWishes.Core.Domain.Wishes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KWishes.API.Controllers;

[Route("api/v1/comments")]
public class CommentsController : Controller
{
    private readonly ISender sender;

    public CommentsController(ISender sender)
    {
        this.sender = sender;
    }
    
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<DtoItems<DtoComment>>> Search(
        [FromQuery] DtoSearchCommentsQuery query,
        [FromServices] DtoCommentMapper dtoCommentMapper)
    {
        var searchResult = await sender.Send(
            new SearchComments.Request(
                new WishId(query.WishId),
                query.CreatedAtPeriod?.From ?? DateTime.UnixEpoch, 
                query.CreatedAtPeriod?.To ?? DateTime.UtcNow + TimeSpan.FromMinutes(1),
                query.Page.Skip,
                query.Page.Take));

        if (searchResult.TryGetError(out var comments, out var errorInfo))
            return ApiErrorRegistry.ErrorInfo(errorInfo);
        
        return await dtoCommentMapper.MapFrom(comments);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> Create([FromBody] DtoCommentCreateInfo createInfo)
    {
        var createCommentResult = await sender.Send(
            new CreateComment.Request(
                User.GetKWishesUserId(),
                new WishId(createInfo.WishId),
                createInfo.Text, 
                createInfo.FileUrls));

        if (createCommentResult.TryGetError(out var commentId, out var errorInfo))
            return ApiErrorRegistry.ErrorInfo(errorInfo);

        return commentId.Value;
    }
}