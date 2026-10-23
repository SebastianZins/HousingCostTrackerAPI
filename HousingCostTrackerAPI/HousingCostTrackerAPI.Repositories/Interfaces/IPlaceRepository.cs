using HousingCostTrackerAPI.Models.Models.Place;

namespace HousingCostTrackerAPI.Repositories.Interfaces
{
    public interface IPlaceRepository
    {
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<PlaceModel>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<PlaceModel> GetPlaceAsync(Guid id, CancellationToken cancellationToken = default);
        Task UpsertAsync(PlaceModel model, CancellationToken cancellationToken = default);
    }
}