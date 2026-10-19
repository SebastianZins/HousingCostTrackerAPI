using HousingCostTrackerAPI.Models.Models.Person;
using HousingCostTrackerAPI.Models.Models.Transaction;
using Newtonsoft.Json;

namespace HousingCostTrackerAPI.Models.Person
{
    public class PersonDTO
    {
        [JsonProperty("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonProperty("firstName")]
        public string FirstName { get; set; } = string.Empty;

        [JsonProperty("lastName")]
        public string LastName { get; set; } = string.Empty;

        [JsonProperty("shortName")]
        public string ShortName { get; set; } = string.Empty;

        public PersonDTO() { }

        public PersonDTO(PersonModel transaction)
        {
            Id = transaction.Id;
            FirstName = transaction.FirstName;
            LastName = transaction.LastName;
            ShortName = transaction.ShortName;
        }

        public PersonModel ToModel()
        {
            return new PersonModel()
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                ShortName = ShortName,
            };
        }

        public PersonModel WriteToModel(PersonModel transaction)
        {
            transaction.Id = Id;
            transaction.FirstName = FirstName;
            transaction.LastName = LastName;
            transaction.ShortName = ShortName;
            return transaction;
        }
    }
}
