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
            List<CategoryDTO> categorys = await _categoryService.GetAllCategorysAsync();
            return Ok(categorys);
        }

        [HttpGet]
        public async Task<CategoryDTO> GetCategoryAsync([FromQuery] Guid id)
        {
            CategoryDTO model = await _categoryService.GetCategoryAsync(id);
            return model;
        }

        [HttpPost]
        public async Task UpsertAsync([FromBody] CategoryDTO model)
        {
            await _categoryService.UpsertAsync(model);
        }

        [HttpDelete]
        public async Task DeleteAsync([FromQuery] Guid id)
        {
            await _categoryService.DeleteAsync(id);
        }
    }
}
