using HousingCostTrackerAPI.Models.Models.Category;
using Newtonsoft.Json;

namespace HousingCostTrackerAPI.Models.Category
{
    public class CategoryDTO
    {
        [JsonProperty("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        public CategoryDTO() { }

        public CategoryDTO(CategoryModel transaction)
        {
            Id = transaction.Id;
            Name = transaction.Name;
        }

        public CategoryModel ToModel()
        {
            return new CategoryModel()
            {
                Id = Id,
                Name = Name,
            };
        }

        public CategoryModel WriteToModel(CategoryModel transaction)
        {
            transaction.Id = Id;
            transaction.Name = Name;
            return transaction;
        }
    }
}
