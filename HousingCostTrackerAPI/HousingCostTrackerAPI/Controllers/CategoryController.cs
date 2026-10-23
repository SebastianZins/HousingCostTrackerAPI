using HousingCostTrackerAPI.Models;
using HousingCostTrackerAPI.Models.Category;
using HousingCostTrackerAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HousingCostTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                List<CategoryDTO> categories = await _categoryService.GetAllCategorysAsync();
                return new ApiJsonResult(categories);
            }
            catch (Exception ex)
            {
                return new ApiJsonResult(false, ex.Message );
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryAsync([FromQuery] Guid id)
        {
            try
            {
                CategoryDTO model = await _categoryService.GetCategoryAsync(id);
                return new ApiJsonResult(model);
            }
            catch (Exception ex)
            {
                return new ApiJsonResult(false, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertAsync([FromBody] CategoryDTO model)
        {
            try
            {
                await _categoryService.UpsertAsync(model);
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
                await _categoryService.DeleteAsync(id);
                return new ApiJsonResult(true);
            }
            catch (Exception ex)
            {
                return new ApiJsonResult(false, ex.Message);
            }
        }
    }
}
