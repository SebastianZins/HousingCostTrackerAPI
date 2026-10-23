using HousingCostTrackerAPI.Models.Models.Place;
using HousingCostTrackerAPI.Models.Place;
using HousingCostTrackerAPI.Repositories.Interfaces;
using HousingCostTrackerAPI.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace HousingCostTrackerAPI.UnitTests.UnitTests
{
    public class PlaceServiceTests
    {
        private readonly Mock<IPlaceRepository> _repositoryMock = new();
        private readonly Mock<ILogger<PlaceService>> _loggerMock = new();

        private PlaceService CreateService() =>
            new(_repositoryMock.Object, _loggerMock.Object);

        #region GetAll

        [Fact]
        public async Task GetAll_WhenRepositoryReturnsData_ReturnsDtos()
        {
            var models = new List<PlaceModel>
            {
                new() { Id = Guid.NewGuid(), Name = "Supermarkt" },
                new() { Id = Guid.NewGuid(), Name = "Bäckerei" }
            };

            _repositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(models);

            var result = await CreateService().GetAllPlacesAsync();

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
                .ReturnsAsync(new List<PlaceModel>());

            var result = await CreateService().GetAllPlacesAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAll_WhenRepositoryFails_ThrowsExpectedException()
        {
            _repositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            var exception = await Assert.ThrowsAsync<Exception>(
                () => CreateService().GetAllPlacesAsync());

            Assert.Equal("Laden aller Empfänger fehlgeschlagen", exception.Message);
        }

        #endregion

        #region GetById

        [Fact]
        public async Task GetById_WhenRepositoryReturnsModel_ReturnsDto()
        {
            var id = Guid.NewGuid();
            var model = new PlaceModel { Id = id, Name = "Supermarkt" };

            _repositoryMock
                .Setup(r => r.GetPlaceAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(model);

            var result = await CreateService().GetPlaceAsync(id);

            Assert.Equal(id, result.Id);
            Assert.Equal(model.Name, result.Name);

            _repositoryMock.Verify(
                r => r.GetPlaceAsync(id, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetById_WhenRepositoryReturnsNull_ThrowsExpectedException()
        {
            var id = Guid.NewGuid();

            _repositoryMock
                .Setup(r => r.GetPlaceAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((PlaceModel)null!);

            var exception = await Assert.ThrowsAsync<Exception>(
                () => CreateService().GetPlaceAsync(id));

            Assert.Equal("Laden des Empfängers fehlgeschlagen", exception.Message);
        }

        [Fact]
        public async Task GetById_WhenRepositoryFails_ThrowsExpectedException()
        {
            var id = Guid.NewGuid();

            _repositoryMock
                .Setup(r => r.GetPlaceAsync(id, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            var exception = await Assert.ThrowsAsync<Exception>(
                () => CreateService().GetPlaceAsync(id));

            Assert.Equal("Laden des Empfängers fehlgeschlagen", exception.Message);
        }

        #endregion

        #region Upsert

        [Fact]
        public async Task Upsert_WhenCalled_PassesMappedModelToRepository()
        {
            var dto = new PlaceDTO
            {
                Id = Guid.NewGuid(),
                Name = "Supermarkt"
            };

            await CreateService().UpsertAsync(dto);

            _repositoryMock.Verify(
                r => r.UpsertAsync(
                    It.Is<PlaceModel>(m =>
                        m.Id == dto.Id &&
                        m.Name == dto.Name),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Upsert_WhenRepositoryFails_ThrowsExpectedException()
        {
            var dto = new PlaceDTO
            {
                Id = Guid.NewGuid(),
                Name = "Supermarkt"
            };

            _repositoryMock
                .Setup(r => r.UpsertAsync(
                    It.IsAny<PlaceModel>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            var exception = await Assert.ThrowsAsync<Exception>(
                () => CreateService().UpsertAsync(dto));

            Assert.Equal(
                "Aktualisieren oder Erstellen des Empfängers fehlgeschlagen",
                exception.Message);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task Delete_WhenCalled_DeletesPlace()
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

            Assert.Equal("Löschen des Empfängers fehlgeschlagen", exception.Message);
        }

        #endregion
    }
}
