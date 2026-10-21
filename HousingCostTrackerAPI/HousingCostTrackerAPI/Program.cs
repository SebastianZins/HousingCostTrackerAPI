using HousingCostTrackerAPI.Database.MongoDB;
using HousingCostTrackerAPI.Repositories.Repositories;
using HousingCostTrackerAPI.Services;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

BsonSerializer.RegisterSerializer(
    new GuidSerializer(GuidRepresentation.Standard)
);

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("https://localhost:1101");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularClient", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services
    .AddControllers()
    .AddNewtonsoftJson();
builder.Services.AddMongoDb(builder.Configuration);

builder.Services.AddScoped<TransactionRepository>();
builder.Services.AddScoped<PersonRepository>();
builder.Services.AddScoped<PlaceRepository>();
builder.Services.AddScoped<CategoryRepository>();

builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<PersonService>();
builder.Services.AddScoped<PlaceService>();
builder.Services.AddScoped<CategoryService>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AngularClient");

app.UseAuthorization();

app.MapControllers();

app.Run();
