namespace HousingCostTrackerAPI.Models.Models.Place
{
    public class PlaceModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
    }
}
