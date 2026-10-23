using Microsoft.AspNetCore.Mvc;

namespace HousingCostTrackerAPI.Models
{
    public class ApiJsonResult : ObjectResult
    {
        public ApiJsonResult() : base(new { result = true })
        {
        }

        public ApiJsonResult(bool result, string? message = null)
            : base(new { result, message })
        {
            StatusCode = result ? 200 : 400;
        }

        public ApiJsonResult(object result, string? message = null)
            : base(new { result, message })
        {
            StatusCode = result != null ? 200 : 400;
        }
    }
}
