using System.ComponentModel.DataAnnotations;
using KWishes.API.Dto.Misc;
using KWishes.Core.Domain.Wishes;
using Microsoft.AspNetCore.Mvc;

namespace KWishes.API.Dto.Wishes;

public record DtoSearchWishesQuery
{
    public DtoSearchPeriodQuery? CreatedAtPeriod { get; init; }

    [Required]
    public DtoSearchPageQuery Page { get; init; } = null!;

    [Required] 
    public string ProductId { get; init; } = null!;

    [FromQuery(Name = "Statuses[]")] 
    public IReadOnlyList<WishStatus>? Statuses { get; init; } = null;
    
    public bool ByCurrentUser { get; init; } = false;
    
    public string? Text { get; init; }
}