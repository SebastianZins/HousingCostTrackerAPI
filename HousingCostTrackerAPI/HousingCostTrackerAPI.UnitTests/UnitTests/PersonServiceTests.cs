using HousingCostTrackerAPI.Models.Models.Person;
using HousingCostTrackerAPI.Models.Person;
using HousingCostTrackerAPI.Repositories.Interfaces;
using HousingCostTrackerAPI.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace HousingCostTrackerAPI.UnitTests.UnitTests
{
    public class PersonServiceTests
    {
        private readonly Mock<IPersonRepository> _repositoryMock = new();
        private readonly Mock<ILogger<PersonService>> _loggerMock = new();

        private PersonService CreateService() =>
            new(_repositoryMock.Object, _loggerMock.Object);

        #region GetAll

        [Fact]
        public async Task GetAll_WhenRepositoryReturnsData_ReturnsDtos()
        {
            var models = new List<PersonModel>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Max",
                    LastName = "Mustermann",
                    ShortName = "MM"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Erika",
                    LastName = "Musterfrau",
                    ShortName = "EM"
                }
            };

            _repositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(models);

            var result = await CreateService().GetAllPersonsAsync();

            Assert.Equal(2, result.Count);

            Assert.Equal(models[0].Id, result[0].Id);
            Assert.Equal(models[0].FirstName, result[0].FirstName);
            Assert.Equal(models[0].LastName, result[0].LastName);
            Assert.Equal(models[0].ShortName, result[0].ShortName);

            Assert.Equal(models[1].Id, result[1].Id);
            Assert.Equal(models[1].FirstName, result[1].FirstName);
            Assert.Equal(models[1].LastName, result[1].LastName);
            Assert.Equal(models[1].ShortName, result[1].ShortName);

            _repositoryMock.Verify(
                r => r.GetAllAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetAll_WhenRepositoryReturnsEmptyList_ReturnsEmptyList()
        {
            _repositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PersonModel>());

            var result = await CreateService().GetAllPersonsAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAll_WhenRepositoryFails_ThrowsExpectedException()
        {
            _repositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            var exception = await Assert.ThrowsAsync<Exception>(
                () => CreateService().GetAllPersonsAsync());

            Assert.Equal("Laden aller Personen fehlgeschlagen", exception.Message);
        }

        #endregion

        #region GetById

        [Fact]
        public async Task GetById_WhenRepositoryReturnsModel_ReturnsDto()
        {
            var id = Guid.NewGuid();
            var model = new PersonModel
            {
                Id = id,
                FirstName = "Max",
                LastName = "Mustermann",
                ShortName = "MM"
            };

            _repositoryMock
                .Setup(r => r.GetPersonAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(model);

            var result = await CreateService().GetPersonAsync(id);

            Assert.Equal(id, result.Id);
            Assert.Equal(model.FirstName, result.FirstName);
            Assert.Equal(model.LastName, result.LastName);
            Assert.Equal(model.ShortName, result.ShortName);

            _repositoryMock.Verify(
                r => r.GetPersonAsync(id, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetById_WhenRepositoryReturnsNull_ThrowsExpectedException()
        {
            var id = Guid.NewGuid();

            _repositoryMock
                .Setup(r => r.GetPersonAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((PersonModel)null!);

            var exception = await Assert.ThrowsAsync<Exception>(
                () => CreateService().GetPersonAsync(id));

            Assert.Equal("Laden der Person fehlgeschlagen", exception.Message);
        }

        [Fact]
        public async Task GetById_WhenRepositoryFails_ThrowsExpectedException()
        {
            var id = Guid.NewGuid();

            _repositoryMock
                .Setup(r => r.GetPersonAsync(id, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            var exception = await Assert.ThrowsAsync<Exception>(
                () => CreateService().GetPersonAsync(id));

            Assert.Equal("Laden der Person fehlgeschlagen", exception.Message);
        }

        #endregion

        #region Upsert

        [Fact]
        public async Task Upsert_WhenCalled_PassesMappedModelToRepository()
        {
            var dto = new PersonDTO
            {
                Id = Guid.NewGuid(),
                FirstName = "Max",
                LastName = "Mustermann",
                ShortName = "MM"
            };

            await CreateService().UpsertAsync(dto);

            _repositoryMock.Verify(
                r => r.UpsertAsync(
                    It.Is<PersonModel>(m =>
                        m.Id == dto.Id &&
                        m.FirstName == dto.FirstName &&
                        m.LastName == dto.LastName &&
                        m.ShortName == dto.ShortName),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Upsert_WhenRepositoryFails_ThrowsExpectedException()
        {
            var dto = new PersonDTO
            {
                Id = Guid.NewGuid(),
                FirstName = "Max",
                LastName = "Mustermann",
                ShortName = "MM"
            };

            _repositoryMock
                .Setup(r => r.UpsertAsync(
                    It.IsAny<PersonModel>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            var exception = await Assert.ThrowsAsync<Exception>(
                () => CreateService().UpsertAsync(dto));

            Assert.Equal(
                "Aktualisieren oder Erstellen der Person fehlgeschlagen",
                exception.Message);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task Delete_WhenCalled_DeletesPerson()
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

            Assert.Equal("Löschen der Person fehlgeschlagen", exception.Message);
        }

        #endregion
    }
}
