using KWishes.API.Authentication;
using KWishes.API.Configuration;
using KWishes.API.Dto.Comments;
using KWishes.API.Dto.Errors;
using KWishes.Core.Application.Misc;
using KWishes.Core.Application.Misc.Caches;
using KWishes.Core.Application.Users.Repository;
using KWishes.Core.Application.Users.Requests;
using KWishes.Core.Domain.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;

namespace KWishes.API;

public static class StartupExtensions
{
    private static readonly PathString AuthenticationEndpointPath = "/api/v1/auth";
    private static readonly PathString AuthenticationCallbackPath = "/api/v1/auth/callback";
    
    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        return services
            .AddCache()
            .AddMediatR(configuration => 
                configuration.RegisterServicesFromAssembly(typeof(CreateUserByGoogleAccount).Assembly));
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static IServiceCollection AddCache(this IServiceCollection services)
    {
        return services
            .AddSingleton<IReadOnlyCache<UserId, User>>(p =>
                new UserByIdCache(
                    p.GetService<IDbContextFactory<RepositoryContext>>() 
                        ?? throw new NullReferenceException("No DbContextFactory"), 
                    new AbsoluteLruCacheOptions(1000, TimeSpan.FromMinutes(10))));
    }

    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        return services.AddSingleton<DtoCommentMapper>();
    }
    
    public static AuthenticationBuilder AddAuthentication(
        this IServiceCollection services,
        KWishesConfiguration configuration)
    {
        return services
            .AddSingleton<IClaimsTransformation, AddKWishesUserClaimsTransformation>()
            .AddAuthentication(GoogleDefaults.AuthenticationScheme)
            .AddCookie(options => options.Cookie.Name = "kws.access_token")
            .AddGoogle(options =>
            {
                options.ClientId = configuration.Authentication.ClientId; 
                options.ClientSecret = configuration.Authentication.ClientSecret;
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.SaveTokens = true;
                options.CallbackPath = AuthenticationCallbackPath;
                
                // https://stackoverflow.com/questions/53980129/oidc-login-fails-with-correlation-failed-cookie-not-found-while-cookie-is
                options.CorrelationCookie.SameSite = SameSiteMode.Lax;

                if (!configuration.Authentication.IsAutoRedirectEnabled)
                {
                    options.Events.OnRedirectToAuthorizationEndpoint = context =>
                    {
                        // AuthEndpointPath == CallbackPath
                        // => AuthHandler will try to handle request as callback
                        // => no redirect
                        if (context.Request.Path == AuthenticationEndpointPath)
                        {
                            context.Response.Redirect(context.RedirectUri);
                            return Task.CompletedTask;
                        }
                        
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        
                        return context.Response.WriteAsJsonAsync(ApiErrorRegistry.AuthenticationDto());
                    };
                }

                // TODO: refresh_token
                // options.Events.OnCreatingTicket = context =>
                // {
                //     context.Response.Headers.SetCookie += $"refresh_token={context.RefreshToken}";
                //     return Task.CompletedTask;
                // };
            });
    }
}