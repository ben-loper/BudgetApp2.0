using AspNetCore.Identity.Mongo;
using Infrastructure.DatabaseConfig;
using Infrastructure.Models;

var builder = WebApplication.CreateBuilder(args);

var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>()
        ?? throw new Exception("Could not find configuration for MongoDB");

// Add services to the container.
builder.Services.AddIdentityMongoDbProvider<ApplicationUser, ApplicationRole>(
    identity => { },
    mongo =>
    {
        mongo.ConnectionString = $"{mongoDbSettings.ConnectionString}";
        mongo.UsersCollection = mongoDbSettings.IdentityCollectionName;
    }
);

builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
