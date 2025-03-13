using System.ComponentModel.DataAnnotations;
using KWishes.Core.Domain.Users;

namespace KWishes.API.Dto.Users;

public record DtoUser(
    [Required] Guid Id,
    [Required] string Username,
    [Required] string FirstName,
    [Required] string SecondName,
    [Required] string Email,
    [Required] Role Role,
    [Required] string GoogleNameId
)
{
    public Uri? Avatar { get; init; }
}