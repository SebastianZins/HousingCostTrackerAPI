using HousingCostTrackerAPI.Models.Category;
using HousingCostTrackerAPI.Models.Models.Category;
using HousingCostTrackerAPI.Repositories.Interfaces;
using HousingCostTrackerAPI.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace HousingCostTrackerAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ILogger<CategoryService> _logger;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public async Task<List<CategoryDTO>> GetAllCategorysAsync()
        {
            try
            {
                List<CategoryModel> models = await _categoryRepository.GetAllAsync();
                return models.Select(m => new CategoryDTO(m)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load all categories");
                throw new Exception("Laden aller Kategorien fehlgeschlagen");
            }
        }

        public async Task<CategoryDTO> GetCategoryAsync(Guid id)
        {
            try
            {
                CategoryModel model = await _categoryRepository.GetCategoryAsync(id);
                return new CategoryDTO(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to load category with Id {id.ToString()}");
                throw new Exception("Laden der Kategorie fehlgeschlagen");
            }
        }

        public async Task UpsertAsync(CategoryDTO dto)
        {
            try
            {
                CategoryModel model = dto.ToModel();
                await _categoryRepository.UpsertAsync(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to upsert item {dto.Id}");
                throw new Exception("Aktualisieren oder Erstellen der Kategorie fehlgeschlagen");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                await _categoryRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete category");
                throw new Exception("Löschen der Kategorie fehlgeschlagen");
            }
        }
    }
}
