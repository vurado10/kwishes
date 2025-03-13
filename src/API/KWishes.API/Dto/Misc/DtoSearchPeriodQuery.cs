namespace KWishes.API.Dto.Misc;

public record DtoSearchPeriodQuery
{
    public DateTime? From { get; init; }
    
    public DateTime? To { get; init; }
}