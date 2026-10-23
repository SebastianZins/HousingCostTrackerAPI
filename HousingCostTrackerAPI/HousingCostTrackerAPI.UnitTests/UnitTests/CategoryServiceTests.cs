using HousingCostTrackerAPI.Models.Category;
using HousingCostTrackerAPI.Models.Models.Category;
using HousingCostTrackerAPI.Repositories.Interfaces;
using HousingCostTrackerAPI.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace HousingCostTrackerAPI.UnitTests.UnitTests
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _repositoryMock = new();
        private readonly Mock<ILogger<CategoryService>> _loggerMock = new();

        private CategoryService CreateService() =>
            new(_repositoryMock.Object, _loggerMock.Object);

        #region GetAll

        [Fact]
        public async Task GetAll_WhenRepositoryReturnsData_ReturnsDtos()
        {
            var models = new List<CategoryModel>
            {
                new() { Id = Guid.NewGuid(), Name = "Lebensmittel" },
                new() { Id = Guid.NewGuid(), Name = "Miete" }
            };

            _repositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(models);

            var result = await CreateService().GetAllCategorysAsync();

            Assert.Equal(2, result.Count);
            Assert.Equal(models[0].Id, result[0].Id);
            Assert.Equal(models[0].Name, result[0].Name);
            Assert.Equal(models[1].Id, result[1].Id);
            Assert.Equal(models[1].Name, result[1].Name);

            _repositoryMock.Verify(
                r => r.GetAllAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetAll_WhenRepositoryReturnsEmptyList_ReturnsEmptyList()
        {
            _repositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CategoryModel>());

            var result = await CreateService().GetAllCategorysAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAll_WhenRepositoryFails_ThrowsExpectedException()
        {
            _repositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            var exception = await Assert.ThrowsAsync<Exception>(
                () => CreateService().GetAllCategorysAsync());

            Assert.Equal("Laden aller Kategorien fehlgeschlagen", exception.Message);
        }

        #endregion

        #region GetById

        [Fact]
        public async Task GetById_WhenRepositoryReturnsModel_ReturnsDto()
        {
            var id = Guid.NewGuid();
            var model = new CategoryModel { Id = id, Name = "Lebensmittel" };

            _repositoryMock
                .Setup(r => r.GetCategoryAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(model);

            var result = await CreateService().GetCategoryAsync(id);

            Assert.Equal(id, result.Id);
            Assert.Equal(model.Name, result.Name);

            _repositoryMock.Verify(
                r => r.GetCategoryAsync(id, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetById_WhenRepositoryReturnsNull_ThrowsExpectedException()
        {
            var id = Guid.NewGuid();

            _repositoryMock
                .Setup(r => r.GetCategoryAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((CategoryModel)null!);

            var exception = await Assert.ThrowsAsync<Exception>(
                () => CreateService().GetCategoryAsync(id));

            Assert.Equal("Laden der Kategorie fehlgeschlagen", exception.Message);
        }

        [Fact]
        public async Task GetById_WhenRepositoryFails_ThrowsExpectedException()
        {
            var id = Guid.NewGuid();

            _repositoryMock
                .Setup(r => r.GetCategoryAsync(id, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            var exception = await Assert.ThrowsAsync<Exception>(
                () => CreateService().GetCategoryAsync(id));

            Assert.Equal("Laden der Kategorie fehlgeschlagen", exception.Message);
        }

        #endregion

        #region Upsert

        [Fact]
        public async Task Upsert_WhenCalled_PassesMappedModelToRepository()
        {
            var dto = new CategoryDTO
            {
                Id = Guid.NewGuid(),
                Name = "Lebensmittel"
            };

            await CreateService().UpsertAsync(dto);

            _repositoryMock.Verify(
                r => r.UpsertAsync(
                    It.Is<CategoryModel>(m =>
                        m.Id == dto.Id &&
                        m.Name == dto.Name),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Upsert_WhenRepositoryFails_ThrowsExpectedException()
        {
            var dto = new CategoryDTO
            {
                Id = Guid.NewGuid(),
                Name = "Lebensmittel"
            };

            _repositoryMock
                .Setup(r => r.UpsertAsync(
                    It.IsAny<CategoryModel>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            var exception = await Assert.ThrowsAsync<Exception>(
                () => CreateService().UpsertAsync(dto));

            Assert.Equal(
                "Aktualisieren oder Erstellen der Kategorie fehlgeschlagen",
                exception.Message);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task Delete_WhenCalled_DeletesCategory()
        {
            var id = Guid.NewGuid();

            await CreateService().DeleteAsync(id);

            _repositoryMock.Verify(
                r => r.DeleteAsync(id, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Delete_WhenRepositoryFails_ThrowsExpectedException()
        {
            var id = Guid.NewGuid();

            _repositoryMock
                .Setup(r => r.DeleteAsync(id, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            var exception = await Assert.ThrowsAsync<Exception>(
                () => CreateService().DeleteAsync(id));

            Assert.Equal("Löschen der Kategorie fehlgeschlagen", exception.Message);
        }

        #endregion
    }
}
