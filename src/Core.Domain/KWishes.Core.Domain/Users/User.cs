namespace KWishes.Core.Domain.Users;

public sealed record User(
    UserId Id,
    string Username,
    string FirstName,
    string SecondName,
    EmailAddress Email,
    Role Role,
    Uri? Avatar,
    string GoogleNameId
);