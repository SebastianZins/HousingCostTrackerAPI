using HousingCostTrackerAPI.Models.Models.Transaction;
using HousingCostTrackerAPI.Models.Transaction;
using HousingCostTrackerAPI.Repositories.Repositories;
using MongoDB.Driver;

namespace HousingCostTrackerAPI.Services
{
    public class TransactionService
    {
        private readonly TransactionRepository _transactionRepository;

        public TransactionService(TransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<List<TransactionDTO>> GetAllTransactionsAsync()
        {
            List<TransactionModel> models = await _transactionRepository.GetAllAsync();
            return models.Select(m => new TransactionDTO(m)).ToList();
        }

        public async Task<TransactionDTO> GetTransactionAsync(Guid id)
        {
            TransactionModel model = await _transactionRepository.GetTransactionAsync(id);
            return new TransactionDTO(model);
        }

        public async Task UpsertAsync(TransactionDTO dto)
        {
            TransactionModel model = dto.ToModel();
            await _transactionRepository.UpsertAsync(model);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _transactionRepository.DeleteAsync(id);
        }
    }
}
