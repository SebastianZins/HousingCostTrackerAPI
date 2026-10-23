using HousingCostTrackerAPI.Models.Models.Person;
using HousingCostTrackerAPI.Models.Person;
using HousingCostTrackerAPI.Repositories.Interfaces;
using HousingCostTrackerAPI.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace HousingCostTrackerAPI.Services
{
    public class PersonService : IPersonService
    {
        private readonly ILogger<PersonService> _logger;
        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository, ILogger<PersonService> logger)
        {
            _personRepository = personRepository;
            _logger = logger;
        }

        public async Task<List<PersonDTO>> GetAllPersonsAsync()
        {
            try
            {
                List<PersonModel> models = await _personRepository.GetAllAsync();
                return models.Select(m => new PersonDTO(m)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load all persons");
                throw new Exception("Laden aller Personen fehlgeschlagen");
            }
        }

        public async Task<PersonDTO> GetPersonAsync(Guid id)
        {
            try
            {
                PersonModel model = await _personRepository.GetPersonAsync(id);
                return new PersonDTO(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to load person with Id {id.ToString()}");
                throw new Exception("Laden der Person fehlgeschlagen");
            }
        }

        public async Task UpsertAsync(PersonDTO dto)
        {
            try
            {
                PersonModel model = dto.ToModel();
                await _personRepository.UpsertAsync(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to upsert item {dto.Id}");
                throw new Exception("Aktualisieren oder Erstellen der Person fehlgeschlagen");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                await _personRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete person");
                throw new Exception("Löschen der Person fehlgeschlagen");
            }
        }
    }
}
