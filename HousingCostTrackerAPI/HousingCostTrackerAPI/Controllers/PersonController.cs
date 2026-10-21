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
            List<PersonDTO> persons = await _personService.GetAllPersonsAsync();
            return Ok(persons);
        }

        [HttpGet]
        public async Task<IActionResult> GetPersonAsync([FromQuery] Guid id)
        {
            PersonDTO model = await _personService.GetPersonAsync(id);
            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpsertAsync([FromBody] PersonDTO model)
        {
            await _personService.UpsertAsync(model);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromQuery] Guid id)
        {
            await _personService.DeleteAsync(id);
            return Ok();
        }
    }
}
