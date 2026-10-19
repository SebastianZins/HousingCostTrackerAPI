using HousingCostTrackerAPI.Models.Models.Place;
using HousingCostTrackerAPI.Models.Place;
using HousingCostTrackerAPI.Repositories.Repositories;

namespace HousingCostTrackerAPI.Services
{
    public class PlaceService
    {
        private readonly PlaceRepository _placeRepository;

        public PlaceService(PlaceRepository placeRepository)
        {
            _placeRepository = placeRepository;
        }

        public async Task<List<PlaceDTO>> GetAllPlacesAsync()
        {
            List<PlaceModel> models = await _placeRepository.GetAllAsync();
            return models.Select(m => new PlaceDTO(m)).ToList();
        }

        public async Task<PlaceDTO> GetPlaceAsync(Guid id)
        {
            PlaceModel model = await _placeRepository.GetPlaceAsync(id);
            return new PlaceDTO(model);
        }

        public async Task UpsertAsync(PlaceDTO dto)
        {
            PlaceModel model = dto.ToModel();
            await _placeRepository.UpsertAsync(model);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _placeRepository.DeleteAsync(id);
        }
    }
}
