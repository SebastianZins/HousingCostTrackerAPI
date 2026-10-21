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

        public PlaceDTO(PlaceModel model)
        {
            Id = model.Id;
            Name = model.Name;
        }

        public PlaceModel ToModel()
        {
            return new PlaceModel()
            {
                Id = Id,
                Name = Name,
            };
        }

        public PlaceModel WriteToModel(PlaceModel model)
        {
            model.Id = Id;
            model.Name = Name;
            return model;
        }
    }
}
