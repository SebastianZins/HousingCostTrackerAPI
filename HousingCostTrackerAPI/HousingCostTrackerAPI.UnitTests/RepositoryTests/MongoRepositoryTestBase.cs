using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Testcontainers.MongoDb;

namespace HousingCostTrackerAPI.UnitTests.RepositoryTests
{
    public abstract class MongoRepositoryTestBase : IAsyncLifetime
    {
        private readonly MongoDbContainer _container =
            new MongoDbBuilder("mongo:7.0").Build();

        protected IMongoDatabase Database { get; private set; } = null!;

        static MongoRepositoryTestBase()
        {
            BsonSerializer.RegisterSerializer(
                new GuidSerializer(GuidRepresentation.Standard));
        }

        public async Task InitializeAsync()
        {
            await _container.StartAsync();

            MongoClient client = new(_container.GetConnectionString());

            Database = client.GetDatabase(
                $"HousingCostTrackerTests_{Guid.NewGuid():N}");
        }

        public async Task DisposeAsync()
        {
            await _container.DisposeAsync();
        }

        protected async Task ClearCollectionAsync<T>(string collectionName)
        {
            IMongoCollection<T> collection =
                Database.GetCollection<T>(collectionName);

            await collection.DeleteManyAsync(
                Builders<T>.Filter.Empty);
        }
    }
}
