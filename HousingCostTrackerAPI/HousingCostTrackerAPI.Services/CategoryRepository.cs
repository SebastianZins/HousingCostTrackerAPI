using HousingCostTrackerAPI.Models.Models.Category;
using HousingCostTrackerAPI.Models.Category;
using HousingCostTrackerAPI.Repositories.Repositories;

namespace HousingCostTrackerAPI.Services
{
    public class CategoryService
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoryService(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryDTO>> GetAllCategorysAsync()
        {
            List<CategoryModel> models = await _categoryRepository.GetAllAsync();
            return models.Select(m => new CategoryDTO(m)).ToList();
        }

        public async Task<CategoryDTO> GetCategoryAsync(Guid id)
        {
            CategoryModel model = await _categoryRepository.GetCategoryAsync(id);
            return new CategoryDTO(model);
        }

        public async Task UpsertAsync(CategoryDTO dto)
        {
            CategoryModel model = dto.ToModel();
            await _categoryRepository.UpsertAsync(model);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _categoryRepository.DeleteAsync(id);
        }
    }
}
