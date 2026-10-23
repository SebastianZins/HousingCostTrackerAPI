using HousingCostTrackerAPI.Models.Models.Place;
using HousingCostTrackerAPI.Models.Place;
using HousingCostTrackerAPI.Repositories.Interfaces;
using HousingCostTrackerAPI.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace HousingCostTrackerAPI.Services
{
    public class PlaceService : IPlaceService
    {
        private readonly ILogger<PlaceService> _logger;
        private readonly IPlaceRepository _placeRepository;

        public PlaceService(IPlaceRepository placeRepository, ILogger<PlaceService> logger)
        {
            _placeRepository = placeRepository;
            _logger = logger;
        }

        public async Task<List<PlaceDTO>> GetAllPlacesAsync()
        {
            try
            {
                List<PlaceModel> models = await _placeRepository.GetAllAsync();
                return models.Select(m => new PlaceDTO(m)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load all places");
                throw new Exception("Laden aller Empfänger fehlgeschlagen");
            }
        }

        public async Task<PlaceDTO> GetPlaceAsync(Guid id)
        {
            try
            {
                PlaceModel model = await _placeRepository.GetPlaceAsync(id);
                return new PlaceDTO(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to load place with Id {id.ToString()}");
                throw new Exception("Laden des Empfängers fehlgeschlagen");
            }
        }

        public async Task UpsertAsync(PlaceDTO dto)
        {
            try
            {
                PlaceModel model = dto.ToModel();
                await _placeRepository.UpsertAsync(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to upsert item {dto.Id}");
                throw new Exception("Aktualisieren oder Erstellen des Empfängers fehlgeschlagen");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                await _placeRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete place");
                throw new Exception("Löschen des Empfängers fehlgeschlagen");
            }
        }
    }
}
