using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HousingCostTrackerAPI.Database.MongoDB
{
    public static class MongoDbServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDb(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var section = configuration.GetSection(MongoDbSettings.SectionName);
            var settings = new MongoDbSettings
            {
                ConnectionString = section["ConnectionString"] ?? string.Empty,
                DatabaseName = section["DatabaseName"] ?? string.Empty
            };

            if (string.IsNullOrWhiteSpace(settings.ConnectionString) &&
                string.IsNullOrWhiteSpace(settings.DatabaseName))
                throw new InvalidOperationException("MongoDb configuration is missing.");

            if (string.IsNullOrWhiteSpace(settings.ConnectionString))
                throw new InvalidOperationException(
                    "MongoDb:ConnectionString is missing.");

            if (string.IsNullOrWhiteSpace(settings.DatabaseName))
                throw new InvalidOperationException(
                    "MongoDb:DatabaseName is missing.");

            services.AddSingleton<IMongoClient>(
                _ => new MongoClient(settings.ConnectionString));

            services.AddSingleton<IMongoDatabase>(provider =>
                provider.GetRequiredService<IMongoClient>()
                    .GetDatabase(settings.DatabaseName));

            return services;
        }
    }
}
