using HousingCostTrackerAPI.Models.Models.Person;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace HousingCostTrackerAPI.Repositories.Repositories
{
    public class PersonRepository
    {
        private readonly IMongoCollection<PersonModel> _collection;

        public PersonRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<PersonModel>("Persons");
        }

        public async Task<List<PersonModel>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            List<PersonModel> models = await _collection
                .Find(Builders<PersonModel>.Filter.Empty)
                .ToListAsync(cancellationToken);
            return models;
        }

        public async Task<PersonModel> GetPersonAsync(Guid id, CancellationToken cancellationToken = default)
        {
            PersonModel model = await _collection
                .Find(Builders<PersonModel>.Filter.Eq(t => t.Id, id))
                .SingleOrDefaultAsync(cancellationToken);
            return model;
        }

        public async Task UpsertAsync(PersonModel model, CancellationToken cancellationToken = default)
        {
            PersonModel foundModel = await GetPersonAsync(model.Id, cancellationToken);
            if (foundModel == null)
            {

                await _collection.InsertOneAsync(model, null, cancellationToken);
            }
            else
            {
                await _collection.ReplaceOneAsync(
                        Builders<PersonModel>.Filter.Eq(t => t.Id, model.Id),
                        model,
                        new ReplaceOptions(),
                        cancellationToken
                    );
            }
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _collection.DeleteOneAsync(
                    Builders<PersonModel>.Filter.Eq(t => t.Id, id),
                    cancellationToken
                );
        }
    }
}
