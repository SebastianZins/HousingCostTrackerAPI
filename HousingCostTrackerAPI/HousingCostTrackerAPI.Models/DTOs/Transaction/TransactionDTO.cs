using HousingCostTrackerAPI.Common.Enums;
using HousingCostTrackerAPI.Models.Models.Transaction;
using Newtonsoft.Json;

namespace HousingCostTrackerAPI.Models.Transaction
{
    public class TransactionDTO
    {
        [JsonProperty("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonProperty("date")]
        public long Date { get; set; } = 0;

        [JsonProperty("amount")]
        public long Amount { get; set; } = 0;

        [JsonProperty("description")]
        public string? Description { get; set; } = null;

        [JsonProperty("place")]
        public Guid? PlaceId { get; set; } = null;

        [JsonProperty("category")]
        public Guid? CategoryId { get; set; } = null;

        [JsonProperty("payedBy")]
        public Guid PayedById { get; set; } = Guid.Empty;

        [JsonProperty("payedTo")]
        public Guid? PayedToId { get; set; } = null;

        [JsonProperty("transactionType")]
        public ETransactionType TransactionType { get; set; } = ETransactionType.EXPENSE;

        public TransactionDTO() { }

        public TransactionDTO(TransactionModel transaction)
        {
            Id = transaction.Id;
            Date = transaction.Date;
            Amount = transaction.Amount;
            Description = transaction.Description;
            PlaceId = transaction.PlaceId;
            CategoryId = transaction.CategoryId;
            PayedById = transaction.PayedById;
            PayedToId = transaction.PayedToId;
            TransactionType = transaction.TransactionType;
        }

        public TransactionModel ToModel()
        {
            return new TransactionModel()
            {
                Id = Id,
                Date = Date,
                Amount = Amount,
                Description = Description,
                PlaceId = PlaceId,
                CategoryId = CategoryId,
                PayedById = PayedById,
                PayedToId = PayedToId,
                TransactionType = TransactionType,
            };
        }

        public TransactionModel WriteToModel(TransactionModel transaction)
        {
            transaction.Id = Id;
            transaction.Date = Date;
            transaction.Amount = Amount;
            transaction.Description = Description;
            transaction.PlaceId = PlaceId;
            transaction.CategoryId = CategoryId;
            transaction.PayedById = PayedById;
            transaction.PayedToId = PayedToId;
            transaction.TransactionType = TransactionType;
            return transaction;
        }
    }
}
