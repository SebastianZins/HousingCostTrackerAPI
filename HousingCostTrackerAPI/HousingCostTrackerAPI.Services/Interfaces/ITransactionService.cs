using HousingCostTrackerAPI.Models.Transaction;

namespace HousingCostTrackerAPI.Services.Interfaces
{
    public interface ITransactionService
    {
        Task DeleteAsync(Guid id);
        Task<List<TransactionDTO>> GetAllTransactionsAsync();
        Task<TransactionDTO> GetTransactionAsync(Guid id);
        Task UpsertAsync(TransactionDTO dto);
    }
}