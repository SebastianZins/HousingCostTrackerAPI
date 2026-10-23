using HousingCostTrackerAPI.Models;
using HousingCostTrackerAPI.Models.Person;
using HousingCostTrackerAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HousingCostTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class PersonController : Controller
    {
        private readonly PersonService _personService;

        public PersonController(PersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                List<PersonDTO> persons = await _personService.GetAllPersonsAsync();
                return new ApiJsonResult(persons);
            }
            catch (Exception ex)
            {
                return new ApiJsonResult(false, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPersonAsync([FromQuery] Guid id)
        {
            try
            {
                PersonDTO model = await _personService.GetPersonAsync(id);
                return new ApiJsonResult(model);
            }
            catch (Exception ex)
            {
                return new ApiJsonResult(false, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertAsync([FromBody] PersonDTO model)
        {
            try
            {
                await _personService.UpsertAsync(model);
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
                await _personService.DeleteAsync(id);
                return new ApiJsonResult(true);
            }
            catch (Exception ex)
            {
                return new ApiJsonResult(false, ex.Message);
            }
        }
    }
}
