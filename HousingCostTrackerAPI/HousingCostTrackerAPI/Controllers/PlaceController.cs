using HousingCostTrackerAPI.Models;
using HousingCostTrackerAPI.Models.Place;
using HousingCostTrackerAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HousingCostTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class PlaceController : Controller
    {
        private readonly IPlaceService _placeService;

        public PlaceController(IPlaceService placeService)
        {
            _placeService = placeService;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                List<PlaceDTO> places = await _placeService.GetAllPlacesAsync();
                return new ApiJsonResult(places);
            }
            catch (Exception ex)
            {
                return new ApiJsonResult(false, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPlaceAsync([FromQuery] Guid id)
        {
            try
            {
                PlaceDTO model = await _placeService.GetPlaceAsync(id);
                return new ApiJsonResult(model);
            }
            catch (Exception ex)
            {
                return new ApiJsonResult(false, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertAsync([FromBody] PlaceDTO model)
        {
            try
            {
                await _placeService.UpsertAsync(model);
                return new ApiJsonResult(true);
            }
            catch (Exception ex)
            {
                return new ApiJsonResult(false, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromQuery] Guid id)
        {
            try
            {
                await _placeService.DeleteAsync(id);
                return new ApiJsonResult(true);
            }
            catch (Exception ex)
            {
                return new ApiJsonResult(false, ex.Message);
            }
        }
    }
}
