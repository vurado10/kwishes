using System.ComponentModel.DataAnnotations;
using KWishes.Core.Domain.Users;

namespace KWishes.API.Dto.Users;

public record DtoShortUser(
    [Required] Guid Id,
    [Required] string FirstName,
    [Required] string SecondName,
    [Required] Role Role
)
{
    public Uri? Avatar { get; init; }
}