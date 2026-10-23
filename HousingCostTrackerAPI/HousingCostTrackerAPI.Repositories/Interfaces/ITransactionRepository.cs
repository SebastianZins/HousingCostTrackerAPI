using HousingCostTrackerAPI.Models.Models.Transaction;

namespace HousingCostTrackerAPI.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<TransactionModel>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TransactionModel> GetTransactionAsync(Guid id, CancellationToken cancellationToken = default);
        Task UpsertAsync(TransactionModel model, CancellationToken cancellationToken = default);
    }
}