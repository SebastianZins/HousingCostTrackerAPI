using HousingCostTrackerAPI.Repositories.Interfaces;
using HousingCostTrackerAPI.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace HousingCostTrackerAPI.UnitTests.UnitTests
{
    public class TransactionServiceTests
    {
        [Fact]
        public async Task GetAllTransactionsAsync_WhenRepositoryFails_ThrowsExpectedException()
        {
            // Arrange — prepare data
            Mock<ITransactionRepository> repositoryMock = new();
            Mock<ILogger<TransactionService>> loggerMock = new();

            _ = repositoryMock
                .Setup(repo => repo.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException(
                    "Simulated Database Error"));

            TransactionService service = new(
                repositoryMock.Object,
                loggerMock.Object);

            // Act - execute functions
            Exception exception = await Assert.ThrowsAsync<Exception>(
                service.GetAllTransactionsAsync);

            // Assert — test results
            Assert.Equal(
                "Laden aller Transaktion fehlgeschlagen",
                exception.Message);

            repositoryMock.Verify(
                repo => repo.GetAllAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
