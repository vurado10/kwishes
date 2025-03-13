using System.ComponentModel.DataAnnotations;
using KWishes.API.Dto.Misc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace KWishes.API.Dto.Comments;

// ReSharper disable once UnusedAutoPropertyAccessor.Global
public record DtoSearchCommentsQuery
{
    [BindRequired]
    public Guid WishId { get; init; }
    
    public DtoSearchPeriodQuery? CreatedAtPeriod { get; init; }

    [Required]
    public DtoSearchPageQuery Page { get; init; } = null!;
}