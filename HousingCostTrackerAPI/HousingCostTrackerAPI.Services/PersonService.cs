using HousingCostTrackerAPI.Models.Models.Person;
using HousingCostTrackerAPI.Models.Person;
using HousingCostTrackerAPI.Repositories.Repositories;

namespace HousingCostTrackerAPI.Services
{
    public class PersonService
    {
        private readonly PersonRepository _personRepository;

        public PersonService(PersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<List<PersonDTO>> GetAllPersonsAsync()
        {
            List<PersonModel> models = await _personRepository.GetAllAsync();
            return models.Select(m => new PersonDTO(m)).ToList();
        }

        public async Task<PersonDTO> GetPersonAsync(Guid id)
        {
            PersonModel model = await _personRepository.GetPersonAsync(id);
            return new PersonDTO(model);
        }

        public async Task UpsertAsync(PersonDTO dto)
        {
            PersonModel model = dto.ToModel();
            await _personRepository.UpsertAsync(model);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _personRepository.DeleteAsync(id);
        }
    }
}
