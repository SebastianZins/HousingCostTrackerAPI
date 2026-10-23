using HousingCostTrackerAPI.Models.Models.Transaction;
using HousingCostTrackerAPI.Models.Transaction;
using HousingCostTrackerAPI.Repositories.Repositories;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace HousingCostTrackerAPI.Services
{
    public class TransactionService
    {
        private readonly ILogger<CategoryService> _logger;
        private readonly TransactionRepository _transactionRepository;

        public TransactionService(TransactionRepository transactionRepository, ILogger<CategoryService> logger)
        {
            _transactionRepository = transactionRepository;
            _logger = logger;
        }

        public async Task<List<TransactionDTO>> GetAllTransactionsAsync()
        {
            try
            {
                List<TransactionModel> models = await _transactionRepository.GetAllAsync();
                return models.Select(m => new TransactionDTO(m)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load all transactions");
                throw new Exception("Laden aller Transaktion fehlgeschlagen");
            }
        }

        public async Task<TransactionDTO> GetTransactionAsync(Guid id)
        {
            try
            {
                TransactionModel model = await _transactionRepository.GetTransactionAsync(id);
                return new TransactionDTO(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to load transaction with Id {id.ToString()}");
                throw new Exception("Laden der Transaktion fehlgeschlagen");
            }
        }

        public async Task UpsertAsync(TransactionDTO dto)
        {
            try
            {
                TransactionModel model = dto.ToModel();
                await _transactionRepository.UpsertAsync(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to upsert item {dto.Id}");
                throw new Exception("Aktualisieren oder Erstellen der Transaktion fehlgeschlagen");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                await _transactionRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete transaction");
                throw new Exception("Löschen der Transaktion fehlgeschlagen");
            }
        }
    }
}
