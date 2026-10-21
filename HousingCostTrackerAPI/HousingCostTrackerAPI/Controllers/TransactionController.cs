using HousingCostTrackerAPI.Models.Transaction;
using HousingCostTrackerAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HousingCostTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class TransactionController : Controller
    {
        private readonly TransactionService _transactionService;

        public TransactionController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            List<TransactionDTO> transactions = await _transactionService.GetAllTransactionsAsync();
            return Ok(transactions);
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactionAsync([FromQuery] Guid id)
        {
            TransactionDTO model = await _transactionService.GetTransactionAsync(id);
            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpsertAsync([FromBody] TransactionDTO model)
        {
            await _transactionService.UpsertAsync(model);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromQuery] Guid id)
        {
            await _transactionService.DeleteAsync(id);
            return Ok();
        }
    }
}
