using KWishes.Core.Domain.Users;

namespace KWishes.API.Dto.Users;

public static class DtoUserMapper
{
    public static DtoUser MapFrom(User user)
    {
        return new DtoUser(
            user.Id,
            user.Username, 
            user.FirstName,
            user.SecondName,
            user.Email.Value,
            user.Role,
            user.GoogleNameId)
        {
            Avatar = user.Avatar
        };
    }
}