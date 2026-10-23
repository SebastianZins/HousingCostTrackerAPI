using HousingCostTrackerAPI.Models.Category;

namespace HousingCostTrackerAPI.Services.Interfaces
{
    public interface ICategoryService
    {
        Task DeleteAsync(Guid id);
        Task<List<CategoryDTO>> GetAllCategorysAsync();
        Task<CategoryDTO> GetCategoryAsync(Guid id);
        Task UpsertAsync(CategoryDTO dto);
    }
}