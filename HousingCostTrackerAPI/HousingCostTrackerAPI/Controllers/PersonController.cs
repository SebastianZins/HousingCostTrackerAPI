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
        public async Task<PersonDTO> GetPersonAsync([FromQuery] Guid id)
        {
            PersonDTO model = await _personService.GetPersonAsync(id);
            return model;
        }

        [HttpPost]
        public async Task UpsertAsync([FromBody] PersonDTO model)
        {
            await _personService.UpsertAsync(model);
        }

        [HttpDelete]
        public async Task DeleteAsync([FromQuery] Guid id)
        {
            await _personService.DeleteAsync(id);
        }
    }
}
