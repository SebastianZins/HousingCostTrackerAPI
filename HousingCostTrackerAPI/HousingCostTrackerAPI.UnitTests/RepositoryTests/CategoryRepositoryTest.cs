using HousingCostTrackerAPI.Models.Models.Category;
using HousingCostTrackerAPI.Repositories.Repositories;

namespace HousingCostTrackerAPI.UnitTests.RepositoryTests
{
    public class CategoryRepositoryTest : MongoRepositoryTestBase
    {
        private const string CollectionName = "Categories";

        private CategoryRepository CreateRepository()
        {
            return new(Database);
        }

        private static CategoryModel CreateCategory(
            string name,
            Guid? id = null)
        {
            return new CategoryModel
            {
                Id = id ?? Guid.NewGuid(),
                Name = name
            };
        }

        [Fact]
        public async Task GetAllAsync_WhenCategoriesExist_ReturnsAllCategories()
        {
            // Arrange
            await ClearCollectionAsync<CategoryModel>(CollectionName);

            CategoryRepository repository = CreateRepository();
            CategoryModel first = CreateCategory("First");
            CategoryModel second = CreateCategory("Second");

            await repository.UpsertAsync(first);
            await repository.UpsertAsync(second);

            // Act
            List<CategoryModel> result = await repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, x => x.Id == first.Id);
            Assert.Contains(result, x => x.Id == second.Id);
        }

        [Fact]
        public async Task GetCategoryAsync_WhenCategoryExists_ReturnsCategory()
        {
            // Arrange
            await ClearCollectionAsync<CategoryModel>(CollectionName);

            CategoryRepository repository = CreateRepository();
            CategoryModel expected = CreateCategory("Name");

            await repository.UpsertAsync(expected);

            // Act
            CategoryModel result =
                await repository.GetCategoryAsync(expected.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.Id, result.Id);
            Assert.Equal(expected.Name, result.Name);
        }

        [Fact]
        public async Task GetCategoryAsync_WhenCategoryDoesNotExist_ReturnsNull()
        {
            // Arrange
            await ClearCollectionAsync<CategoryModel>(CollectionName);

            CategoryRepository repository = CreateRepository();

            // Act
            CategoryModel result =
                await repository.GetCategoryAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpsertAsync_WhenCategoryDoesNotExist_InsertsCategory()
        {
            // Arrange
            await ClearCollectionAsync<CategoryModel>(CollectionName);

            CategoryRepository repository = CreateRepository();
            CategoryModel Category = CreateCategory("Name");

            // Act
            await repository.UpsertAsync(Category);

            // Assert
            CategoryModel result =
                await repository.GetCategoryAsync(Category.Id);

            Assert.NotNull(result);
            Assert.Equal(Category.Id, result.Id);
            Assert.Equal(Category.Name, result.Name);
        }

        [Fact]
        public async Task UpsertAsync_WhenCategoryExists_UpdatesCategory()
        {
            // Arrange
            await ClearCollectionAsync<CategoryModel>(CollectionName);

            CategoryRepository repository = CreateRepository();
            Guid id = Guid.NewGuid();

            CategoryModel original = CreateCategory(
                "Original", id);

            await repository.UpsertAsync(original);

            CategoryModel updated = CreateCategory(
                "Update", id);

            // Act
            await repository.UpsertAsync(updated);

            // Assert
            CategoryModel result =
                await repository.GetCategoryAsync(id);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("Updated", result.Name);

            List<CategoryModel> all = await repository.GetAllAsync();
            _ = Assert.Single(all);
        }

        [Fact]
        public async Task DeleteAsync_WhenCategoryExists_RemovesCategory()
        {
            // Arrange
            await ClearCollectionAsync<CategoryModel>(CollectionName);

            CategoryRepository repository = CreateRepository();
            CategoryModel Category = CreateCategory("Name");

            await repository.UpsertAsync(Category);

            // Act
            await repository.DeleteAsync(Category.Id);

            // Assert
            CategoryModel result =
                await repository.GetCategoryAsync(Category.Id);

            Assert.Null(result);
            Assert.Empty(await repository.GetAllAsync());
        }
    }
}
