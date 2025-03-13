using KWishes.API;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsProduction())
    builder.Configuration.SetBasePath(Environment.GetEnvironmentVariable("CONFIG_DIRECTORY"));

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile("secrets.json");

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();
startup.Configure(app, app.Environment);

app.Run();
