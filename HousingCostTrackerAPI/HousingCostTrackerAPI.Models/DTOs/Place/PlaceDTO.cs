using HousingCostTrackerAPI.Models.Models.Place;
using Newtonsoft.Json;

namespace HousingCostTrackerAPI.Models.Place
{
    public class PlaceDTO
    {
        [JsonProperty("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        public PlaceDTO() { }

        public PlaceDTO(PlaceModel transaction)
        {
            Id = transaction.Id;
            Name = transaction.Name;
        }

        public PlaceModel ToModel()
        {
            return new PlaceModel()
            {
                Id = Id,
                Name = Name,
            };
        }

        public PlaceModel WriteToModel(PlaceModel transaction)
        {
            transaction.Id = Id;
            transaction.Name = Name;
            return transaction;
        }
    }
}
