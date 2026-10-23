using HousingCostTrackerAPI.Models.Models.Category;

namespace HousingCostTrackerAPI.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<CategoryModel>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<CategoryModel> GetCategoryAsync(Guid id, CancellationToken cancellationToken = default);
        Task UpsertAsync(CategoryModel model, CancellationToken cancellationToken = default);
    }
}