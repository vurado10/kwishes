using KWishes.Core.Application.Misc.Results;
using KWishes.Core.Domain.Users;

namespace KWishes.Core.Application.Users.Requests;

public static class UserErrorRegistry
{
    public static ErrorInfo NoWithGoogleId(string googleId)
    {
        return new ErrorInfo($"No user with GoogleId: {googleId}", ErrorInfoCode.NotFound);
    }
    
    public static ErrorInfo NoWithId(UserId userId)
    {
        return new ErrorInfo($"No user with UserId: {userId.Value}", ErrorInfoCode.NotFound);
    }
}