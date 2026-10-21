using HousingCostTrackerAPI.Models.Models.Person;
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

        public PersonDTO(PersonModel model)
        {
            Id = model.Id;
            FirstName = model.FirstName;
            LastName = model.LastName;
            ShortName = model.ShortName;
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

        public PersonModel WriteToModel(PersonModel model)
        {
            model.Id = Id;
            model.FirstName = FirstName;
            model.LastName = LastName;
            model.ShortName = ShortName;
            return model;
        }
    }
}
