using AspNetCore.Identity.Mongo;
using Domain.Models;
using Domain.Repositories;
using Domain.Services;
using Infrastructure.DatabaseConfig;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using MongoDB.Driver;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>()
        ?? throw new Exception("Could not find configuration for MongoDB");

// Register MongoDB client
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(mongoDbSettings.ConnectionString));

// Register MongoDB context
builder.Services.AddScoped<MongoDbContext>(sp => new MongoDbContext(sp.GetRequiredService<IMongoClient>(), mongoDbSettings.DatabaseName));

// Add services to the container.
builder.Services.AddIdentityMongoDbProvider<ApplicationUser, ApplicationRole>(
    identity => { },
    mongo =>
    {
        mongo.ConnectionString = $"{mongoDbSettings.ConnectionString}";
        mongo.UsersCollection = mongoDbSettings.IdentityCollectionName;
    }
);

builder.Services.AddScoped<IFamilyRepository, FamilyRepository>();


builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFamilyService, FamilyService>();

// Setup middleware to return 401 if the user is not logged instead of attempting to redirect to login
// Without this, it will return a 404
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events = new CookieAuthenticationEvents
    {
        OnRedirectToLogin = ctx =>
        {
            if (ctx.Response.StatusCode == 200)
            {
                ctx.Response.StatusCode = 401;
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
