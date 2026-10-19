using HousingCostTrackerAPI.Models.Models.Transaction;
using MongoDB.Driver;

namespace HousingCostTrackerAPI.Repositories.Repositories
{
    public class TransactionRepository
    {
        private readonly IMongoCollection<TransactionModel> _collection;

        public TransactionRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<TransactionModel>("Transactions");
        }

        public async Task<List<TransactionModel>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            List<TransactionModel> models = await _collection
                .Find(Builders<TransactionModel>.Filter.Empty)
                .ToListAsync(cancellationToken);
            return models;
        }

        public async Task<TransactionModel> GetTransactionAsync(Guid id, CancellationToken cancellationToken = default)
        {
            TransactionModel model = await _collection
                .Find(Builders<TransactionModel>.Filter.Eq(t => t.Id, id))
                .SingleOrDefaultAsync(cancellationToken);
            return model;
        }

        public async Task UpsertAsync(TransactionModel model, CancellationToken cancellationToken = default)
        {
            TransactionModel foundModel = await GetTransactionAsync(model.Id, cancellationToken);
            if (foundModel == null)
            {

                await _collection.InsertOneAsync(model, null, cancellationToken);
            }
            else
            {
                await _collection.ReplaceOneAsync(
                        Builders<TransactionModel>.Filter.Eq(t => t.Id, model.Id),
                        model,
                        new ReplaceOptions(),
                        cancellationToken
                    );
            }
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _collection.DeleteOneAsync(
                    Builders<TransactionModel>.Filter.Eq(t => t.Id, id),
                    cancellationToken
                );
        }
    }
}
