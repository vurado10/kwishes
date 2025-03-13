using System.Diagnostics.CodeAnalysis;
using KWishes.Core.Application.Misc.Results;
using Microsoft.AspNetCore.Mvc;

namespace KWishes.API.Dto.Errors;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public static class ApiErrorConstants
{
    public const string Any = "error:any";
    public const string Validation = "error:validation";
    public const string Authentication = "error:authentication";
    public const string Authorization = "error:authorization";
    public const string NotFound = "error:not-found";
    
    private static readonly Dictionary<ErrorInfoCode, string> DtoErrorCode = new()
    {
        [ErrorInfoCode.Any] = Any,
        [ErrorInfoCode.NotFound] = NotFound
    };
    
    private static readonly Dictionary<ErrorInfoCode, int> HttpCode = new()
    {
        [ErrorInfoCode.Any] = 500,
        [ErrorInfoCode.NotFound] = 404
    };

    public static string MapToDtoErrorCode(ErrorInfoCode code) => 
        !DtoErrorCode.TryGetValue(code, out var dtoCode) ? Any : dtoCode;
    
    public static int MapToHttpCode(ErrorInfoCode code) => 
        !HttpCode.TryGetValue(code, out var httpCode) ? 500 : httpCode;
}

public static class ApiErrorRegistry
{
    public static DtoError AuthenticationDto()
    {
        return new DtoError(ApiErrorConstants.Authentication, "Authentication failed");
    }

    public static ObjectResult Forbidden()
    {
        return new ObjectResult(new DtoError(ApiErrorConstants.Authorization, "Forbidden"))
        {
            StatusCode = StatusCodes.Status403Forbidden
        };
    }
    
    public static ObjectResult ErrorInfo(ErrorInfo errorInfo)
    {
        return new ObjectResult(new DtoError(ApiErrorConstants.MapToDtoErrorCode(errorInfo.Code), errorInfo.Message))
        {
            StatusCode = ApiErrorConstants.MapToHttpCode(errorInfo.Code)
        };
    }
    
    public static ObjectResult Validation(string message = "something went wrong")
    {
        return new ObjectResult(new DtoError(ApiErrorConstants.Validation, message))
        {
            StatusCode = StatusCodes.Status400BadRequest
        };
    }
}