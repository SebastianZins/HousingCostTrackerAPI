using HousingCostTrackerAPI.Models.Models.Person;

namespace HousingCostTrackerAPI.Repositories.Interfaces
{
    public interface IPersonRepository
    {
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<PersonModel>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<PersonModel> GetPersonAsync(Guid id, CancellationToken cancellationToken = default);
        Task UpsertAsync(PersonModel model, CancellationToken cancellationToken = default);
    }
}