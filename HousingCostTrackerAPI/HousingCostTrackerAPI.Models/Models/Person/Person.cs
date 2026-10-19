namespace HousingCostTrackerAPI.Models.Models.Person
{
    public class PersonModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
    }
}
