using HousingCostTrackerAPI.Models.Person;

namespace HousingCostTrackerAPI.Services.Interfaces
{
    public interface IPersonService
    {
        Task DeleteAsync(Guid id);
        Task<List<PersonDTO>> GetAllPersonsAsync();
        Task<PersonDTO> GetPersonAsync(Guid id);
        Task UpsertAsync(PersonDTO dto);
    }
}