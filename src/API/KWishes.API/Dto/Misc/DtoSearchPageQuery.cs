using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace KWishes.API.Dto.Misc;

public record DtoSearchPageQuery
{
    [BindRequired]
    public int Skip { get; init; }
    
    public int Take { get; init; } = 10;
}