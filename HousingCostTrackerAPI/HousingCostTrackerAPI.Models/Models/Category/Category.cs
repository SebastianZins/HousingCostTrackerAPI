namespace HousingCostTrackerAPI.Models.Models.Category
{
    public class CategoryModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
    }
}
