using KWishes.Core.Domain.Wishes;

namespace KWishes.Core.Domain.Products;

public sealed record Product
{
    public string Id { get; init; }
    public string Name { get; set; }
    public Uri Logo { get; set; }
    
    public List<Wish> Wishes { get; set; }
};