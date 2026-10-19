using HousingCostTrackerAPI.Models.Place;
using HousingCostTrackerAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HousingCostTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class PlaceController : Controller
    {
        private readonly PlaceService _placeService;

        public PlaceController(PlaceService placeService)
        {
            _placeService = placeService;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            List<PlaceDTO> places = await _placeService.GetAllPlacesAsync();
            return Ok(places);
        }

        [HttpGet]
        public async Task<PlaceDTO> GetPlaceAsync([FromQuery] Guid id)
        {
            PlaceDTO model = await _placeService.GetPlaceAsync(id);
            return model;
        }

        [HttpPost]
        public async Task UpsertAsync([FromBody] PlaceDTO model)
        {
            await _placeService.UpsertAsync(model);
        }

        [HttpDelete]
        public async Task DeleteAsync([FromQuery] Guid id)
        {
            await _placeService.DeleteAsync(id);
        }
    }
}
