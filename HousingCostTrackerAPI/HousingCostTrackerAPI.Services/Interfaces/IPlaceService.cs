using HousingCostTrackerAPI.Models.Place;

namespace HousingCostTrackerAPI.Services.Interfaces
{
    public interface IPlaceService
    {
        Task DeleteAsync(Guid id);
        Task<List<PlaceDTO>> GetAllPlacesAsync();
        Task<PlaceDTO> GetPlaceAsync(Guid id);
        Task UpsertAsync(PlaceDTO dto);
    }
}