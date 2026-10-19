namespace HousingCostTrackerAPI.Models.Database.MongoDB
{
    public abstract class MongoDbBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
