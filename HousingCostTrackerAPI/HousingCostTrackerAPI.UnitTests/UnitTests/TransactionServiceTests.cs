using HousingCostTrackerAPI.Models.Models.Transaction;
using HousingCostTrackerAPI.Models.Transaction;
using HousingCostTrackerAPI.Repositories.Interfaces;
using HousingCostTrackerAPI.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace HousingCostTrackerAPI.UnitTests.UnitTests
{
    public class TransactionServiceTests
    {
        #region GetAll

        [Fact]
        public async Task GetAll_WhenRepositoryFails_ThrowsExpectedException()
        {
            // Arrange
            Mock<ITransactionRepository> repositoryMock = new();
            Mock<ILogger<TransactionService>> loggerMock = new();

            repositoryMock
                .Setup(repo => repo.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException(
                    "Simulated Database Error"));

            TransactionService service = new(
                repositoryMock.Object,
                loggerMock.Object);

            // Act
            Exception exception = await Assert.ThrowsAsync<Exception>(
                service.GetAllTransactionsAsync);

            // Assert
            Assert.Equal(
                "Laden aller Transaktion fehlgeschlagen",
                exception.Message);

            repositoryMock.Verify(
                repo => repo.GetAllAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetAll_WhenRepositoryReturnsData_ReturnsDtos()
        {
            // Arrange
            Mock<ITransactionRepository> repositoryMock = new();
            Mock<ILogger<TransactionService>> loggerMock = new();

            TransactionModel model1 = new()
            {
                Id = Guid.NewGuid(),
                Amount = 2020L,
                Description = "Groceries"
            };

            TransactionModel model2 = new()
            {
                Id = Guid.NewGuid(),
                Amount = 4040L,
                Description = "Rent"
            };

            repositoryMock
                .Setup(repo => repo.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TransactionModel>
                {
                    model1,
                    model2
                });

            TransactionService service = new(
                repositoryMock.Object,
                loggerMock.Object);

            // Act
            List<TransactionDTO> result =
                await service.GetAllTransactionsAsync();

            // Assert
            Assert.Equal(2, result.Count);

            Assert.Equal(model1.Id, result[0].Id);
            Assert.Equal(model1.Amount, result[0].Amount);
            Assert.Equal(model1.Description, result[0].Description);

            Assert.Equal(model2.Id, result[1].Id);
            Assert.Equal(model2.Amount, result[1].Amount);
            Assert.Equal(model2.Description, result[1].Description);

            repositoryMock.Verify(
                repo => repo.GetAllAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetAll_WhenRepositoryReturnsEmptyList_ReturnsEmptyList()
        {
            // Arrange
            Mock<ITransactionRepository> repositoryMock = new();
            Mock<ILogger<TransactionService>> loggerMock = new();

            repositoryMock
                .Setup(repo => repo.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TransactionModel>());

            TransactionService service = new(
                repositoryMock.Object,
                loggerMock.Object);

            // Act
            List<TransactionDTO> result =
                await service.GetAllTransactionsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            repositoryMock.Verify(
                repo => repo.GetAllAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        #endregion

        #region GetById

        [Fact]
        public async Task GetById_WhenRepositoryReturnsModel_ReturnsDto()
        {
            // Arrange
            Mock<ITransactionRepository> repositoryMock = new();
            Mock<ILogger<TransactionService>> loggerMock = new();

            Guid id = Guid.NewGuid();

            TransactionModel model = new()
            {
                Id = id,
                Amount = 4050L,
                Description = "Test transaction"
            };

            repositoryMock
                .Setup(repo => repo.GetTransactionAsync(
                    id,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(model);

            TransactionService service = new(
                repositoryMock.Object,
                loggerMock.Object);

            // Act
            TransactionDTO result =
                await service.GetTransactionAsync(id);

            // Assert
            Assert.Equal(model.Id, result.Id);
            Assert.Equal(model.Amount, result.Amount);
            Assert.Equal(model.Description, result.Description);

            repositoryMock.Verify(
                repo => repo.GetTransactionAsync(
                    id,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetById_WhenRepositoryFails_ThrowsExpectedException()
        {
            // Arrange
            Mock<ITransactionRepository> repositoryMock = new();
            Mock<ILogger<TransactionService>> loggerMock = new();

            Guid id = Guid.NewGuid();

            repositoryMock
                .Setup(repo => repo.GetTransactionAsync(
                    id,
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException(
                    "Simulated Database Error"));

            TransactionService service = new(
                repositoryMock.Object,
                loggerMock.Object);

            // Act
            Exception exception = await Assert.ThrowsAsync<Exception>(
                () => service.GetTransactionAsync(id));

            // Assert
            Assert.Equal(
                "Laden der Transaktion fehlgeschlagen",
                exception.Message);

            repositoryMock.Verify(
                repo => repo.GetTransactionAsync(
                    id,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetById_WhenRepositoryReturnsNull_ThrowsExpectedException()
        {
            // Arrange
            Mock<ITransactionRepository> repositoryMock = new();
            Mock<ILogger<TransactionService>> loggerMock = new();

            Guid id = Guid.NewGuid();

            repositoryMock
                .Setup(repo => repo.GetTransactionAsync(
                    id,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((TransactionModel)null!);

            TransactionService service = new(
                repositoryMock.Object,
                loggerMock.Object);

            // Act
            Exception exception = await Assert.ThrowsAsync<Exception>(
                () => service.GetTransactionAsync(id));

            // Assert
            Assert.Equal(
                "Laden der Transaktion fehlgeschlagen",
                exception.Message);

            repositoryMock.Verify(
                repo => repo.GetTransactionAsync(
                    id,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        #endregion

        #region Upsert

        [Fact]
        public async Task Upsert_WhenCalled_PassesMappedModelToRepository()
        {
            // Arrange
            Mock<ITransactionRepository> repositoryMock = new();
            Mock<ILogger<TransactionService>> loggerMock = new();

            Guid id = Guid.NewGuid();
            const long amount = 4050L;
            const string description = "Test transaction";

            TransactionDTO dto = new()
            {
                Id = id,
                Amount = amount,
                Description = description
            };

            repositoryMock
                .Setup(repo => repo.UpsertAsync(
                    It.IsAny<TransactionModel>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            TransactionService service = new(
                repositoryMock.Object,
                loggerMock.Object);

            // Act
            await service.UpsertAsync(dto);

            // Assert
            repositoryMock.Verify(
                repo => repo.UpsertAsync(
                    It.Is<TransactionModel>(model =>
                        model.Id == id &&
                        model.Amount == amount &&
                        model.Description == description),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Upsert_WhenRepositoryFails_ThrowsExpectedException()
        {
            // Arrange
            Mock<ITransactionRepository> repositoryMock = new();
            Mock<ILogger<TransactionService>> loggerMock = new();

            TransactionDTO dto = new()
            {
                Id = Guid.NewGuid(),
                Amount = 4050L,
                Description = "Test transaction"
            };

            repositoryMock
                .Setup(repo => repo.UpsertAsync(
                    It.IsAny<TransactionModel>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException(
                    "Simulated Database Error"));

            TransactionService service = new(
                repositoryMock.Object,
                loggerMock.Object);

            // Act
            Exception exception = await Assert.ThrowsAsync<Exception>(
                () => service.UpsertAsync(dto));

            // Assert
            Assert.Equal(
                "Aktualisieren oder Erstellen der Transaktion fehlgeschlagen",
                exception.Message);

            repositoryMock.Verify(
                repo => repo.UpsertAsync(
                    It.IsAny<TransactionModel>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task Delete_WhenCalled_DeletesTransaction()
        {
            // Arrange
            Mock<ITransactionRepository> repositoryMock = new();
            Mock<ILogger<TransactionService>> loggerMock = new();

            Guid id = Guid.NewGuid();

            repositoryMock
                .Setup(repo => repo.DeleteAsync(
                    id,
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            TransactionService service = new(
                repositoryMock.Object,
                loggerMock.Object);

            // Act
            await service.DeleteAsync(id);

            // Assert
            repositoryMock.Verify(
                repo => repo.DeleteAsync(
                    id,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Delete_WhenRepositoryFails_ThrowsExpectedException()
        {
            // Arrange
            Mock<ITransactionRepository> repositoryMock = new();
            Mock<ILogger<TransactionService>> loggerMock = new();

            Guid id = Guid.NewGuid();

            repositoryMock
                .Setup(repo => repo.DeleteAsync(
                    id,
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException(
                    "Simulated Database Error"));

            TransactionService service = new(
                repositoryMock.Object,
                loggerMock.Object);

            // Act
            Exception exception = await Assert.ThrowsAsync<Exception>(
                () => service.DeleteAsync(id));

            // Assert
            Assert.Equal(
                "Löschen der Transaktion fehlgeschlagen",
                exception.Message);

            repositoryMock.Verify(
                repo => repo.DeleteAsync(
                    id,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        #endregion
    }
}
