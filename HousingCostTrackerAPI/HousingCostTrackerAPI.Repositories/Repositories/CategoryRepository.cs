using HousingCostTrackerAPI.Models.Models.Category;
using HousingCostTrackerAPI.Repositories.Interfaces;
using MongoDB.Driver;

namespace HousingCostTrackerAPI.Repositories.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMongoCollection<CategoryModel> _collection;

        public CategoryRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<CategoryModel>("Categories");
        }

        public async Task<List<CategoryModel>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            List<CategoryModel> models = await _collection
                .Find(Builders<CategoryModel>.Filter.Empty)
                .ToListAsync(cancellationToken);
            return models;
        }

        public async Task<CategoryModel> GetCategoryAsync(Guid id, CancellationToken cancellationToken = default)
        {
            CategoryModel model = await _collection
                .Find(Builders<CategoryModel>.Filter.Eq(t => t.Id, id))
                .SingleOrDefaultAsync(cancellationToken);
            return model;
        }

        public async Task UpsertAsync(CategoryModel model, CancellationToken cancellationToken = default)
        {
            CategoryModel foundModel = await GetCategoryAsync(model.Id, cancellationToken);
            if (foundModel == null)
            {

                await _collection.InsertOneAsync(model, null, cancellationToken);
            }
            else
            {
                await _collection.ReplaceOneAsync(
                        Builders<CategoryModel>.Filter.Eq(t => t.Id, model.Id),
                        model,
                        new ReplaceOptions(),
                        cancellationToken
                    );
            }
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _collection.DeleteOneAsync(
                    Builders<CategoryModel>.Filter.Eq(t => t.Id, id),
                    cancellationToken
                );
        }
    }
}
