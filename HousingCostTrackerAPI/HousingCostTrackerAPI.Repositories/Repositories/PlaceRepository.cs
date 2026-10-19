using HousingCostTrackerAPI.Models.Models.Place;
using MongoDB.Driver;

namespace HousingCostTrackerAPI.Repositories.Repositories
{
    public class PlaceRepository
    {
        private readonly IMongoCollection<PlaceModel> _collection;

        public PlaceRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<PlaceModel>("Places");
        }

        public async Task<List<PlaceModel>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            List<PlaceModel> models = await _collection
                .Find(Builders<PlaceModel>.Filter.Empty)
                .ToListAsync(cancellationToken);
            return models;
        }

        public async Task<PlaceModel> GetPlaceAsync(Guid id, CancellationToken cancellationToken = default)
        {
            PlaceModel model = await _collection
                .Find(Builders<PlaceModel>.Filter.Eq(t => t.Id, id))
                .SingleOrDefaultAsync(cancellationToken);
            return model;
        }

        public async Task UpsertAsync(PlaceModel model, CancellationToken cancellationToken = default)
        {
            PlaceModel foundModel = await GetPlaceAsync(model.Id, cancellationToken);
            if (foundModel == null)
            {

                await _collection.InsertOneAsync(model, null, cancellationToken);
            }
            else
            {
                await _collection.ReplaceOneAsync(
                        Builders<PlaceModel>.Filter.Eq(t => t.Id, model.Id),
                        model,
                        new ReplaceOptions(),
                        cancellationToken
                    );
            }
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _collection.DeleteOneAsync(
                    Builders<PlaceModel>.Filter.Eq(t => t.Id, id),
                    cancellationToken
                );
        }
    }
}
