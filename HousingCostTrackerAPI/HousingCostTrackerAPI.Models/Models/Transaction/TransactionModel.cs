using HousingCostTrackerAPI.Common.Enums;
using HousingCostTrackerAPI.Models.Database.MongoDB;
using MongoDB.Bson.Serialization.Attributes;

namespace HousingCostTrackerAPI.Models.Models.Transaction
{
    public class TransactionModel: MongoDbBase
    {
        public long Date { get; set; } = 0;
        public long Amount { get; set; } = 0;
        public string? Description { get; set; } = null;
        public Guid? PlaceId { get; set; } = null;
        public Guid? CategoryId { get; set; } = null;
        public Guid PayedById { get; set; } = Guid.Empty;
        public Guid? PayedToId { get; set; } = null;
        public ETransactionType TransactionType { get; set; } = ETransactionType.EXPENSE;
    }
}
