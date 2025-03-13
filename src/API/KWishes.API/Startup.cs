using System.Text.Json;
using System.Text.Json.Serialization;
using KWishes.API.Configuration;
using KWishes.API.Dto.Errors;
using KWishes.Core.Application.Misc;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.FileProviders;

namespace KWishes.API;

public class Startup
{
    private readonly IConfiguration configuration;

    public Startup(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        var kWishesConfiguration = configuration.GetSection("KWishes").Get<KWishesConfiguration?>();

        if (kWishesConfiguration is null)
            throw new NullReferenceException("No config for KWishes");

        services.AddSingleton(kWishesConfiguration);

        services
            .AddMvcCore()
            .AddCors()
            .AddApiExplorer()
            .AddDataAnnotations()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.AddMappers();

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var message = context.ModelState
                    .Values
                    .First(entry => entry.ValidationState == ModelValidationState.Invalid)
                    .Errors
                    .First()
                    .ErrorMessage;

                return ApiErrorRegistry.Validation(message);
            };
        });
        
        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();

        services.AddAuthentication(kWishesConfiguration);
        services.AddAuthorization();

        services.AddPooledDbContextFactory<RepositoryContext>(
            builder => RepositoryContext.SetOptions(builder, kWishesConfiguration.Db),
            kWishesConfiguration.Db.ContextPoolSize);
        
        services.AddHandlers();

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.All;
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseForwardedHeaders();
        
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseCors(builder => builder.AllowAnyOrigin());

        UseLocalFiles(app);

        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(builder => builder.MapControllers());
    }

    private static void UseLocalFiles(IApplicationBuilder app)
    {
        // Note that Rewriter ignore '/' in the PathString beginning
        var rewriterOptions = new RewriteOptions()
            .AddRewrite(@"^(.*.png)", "$1", true)
            .AddRewrite(@"^(.*.icon)", "$1", true)
            .AddRewrite(@"^(.*.ico)", "$1", true)
            .AddRewrite(@"^(.*.svg)", "$1", true)
            .AddRewrite(@"^(assets/*.*)", "$1", true)
            .AddRewrite(@"^(?!api).*", "/", true);

        app.UseRewriter(rewriterOptions);
        
        var staticDirectory = app.ApplicationServices.GetService<KWishesConfiguration>()!.Static.Directory;
        if (staticDirectory == "")
            staticDirectory = Environment.CurrentDirectory;

        var fileProvider = new PhysicalFileProvider(staticDirectory);
        
        app.UseDefaultFiles(new DefaultFilesOptions
        {
            FileProvider = fileProvider
        });
        
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = fileProvider
        });
    }
}