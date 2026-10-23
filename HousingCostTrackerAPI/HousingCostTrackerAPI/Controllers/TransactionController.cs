using HousingCostTrackerAPI.Models;
using HousingCostTrackerAPI.Models.Transaction;
using HousingCostTrackerAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace HousingCostTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                List<TransactionDTO> transactions = await _transactionService.GetAllTransactionsAsync();
                return new ApiJsonResult(transactions);
            }
            catch (Exception ex)
            {
                return new ApiJsonResult(false, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactionAsync([FromQuery] Guid id)
        {
            try
            {
                TransactionDTO model = await _transactionService.GetTransactionAsync(id);
                return new ApiJsonResult(model);
            }
            catch (Exception ex)
            {
                return new ApiJsonResult(false, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertAsync([FromBody] TransactionDTO model)
        {
            try
            {
                await _transactionService.UpsertAsync(model);
                return new ApiJsonResult(true);
            }
            catch (Exception ex)
            {
                return new ApiJsonResult(false, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromQuery] Guid id)
        {
            try
            {
                await _transactionService.DeleteAsync(id);
                return new ApiJsonResult(true);
            }
            catch (Exception ex)
            {
                return new ApiJsonResult(false, ex.Message);
            }
        }
    }
}
