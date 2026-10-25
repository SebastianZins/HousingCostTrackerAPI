using HousingCostTrackerAPI.Common.Enums;
using HousingCostTrackerAPI.Models.Models.Transaction;
using HousingCostTrackerAPI.Repositories.Repositories;

namespace HousingCostTrackerAPI.UnitTests.RepositoryTests
{
    public class TransactionRepositoryTests : MongoRepositoryTestBase
    {
        private const string CollectionName = "Transactions";

        private TransactionRepository CreateRepository()
        {
            return new(Database);
        }

        private static TransactionModel CreateTransaction(
            Guid? id = null,
            string description = "Test transaction")
        {
            return new TransactionModel
            {
                Id = id ?? Guid.NewGuid(),
                Date = 1792800000000,
                Amount = 42.50,
                Description = description,
                PlaceId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                PayedById = Guid.NewGuid(),
                TransactionType = ETransactionType.EXPENSE
            };
        }

        [Fact]
        public async Task GetAllAsync_WhenTransactionsExist_ReturnsAllTransactions()
        {
            // Arrange
            await ClearCollectionAsync<TransactionModel>(CollectionName);

            TransactionRepository repository = CreateRepository();
            TransactionModel first = CreateTransaction();
            TransactionModel second = CreateTransaction();

            await repository.UpsertAsync(first);
            await repository.UpsertAsync(second);

            // Act
            List<TransactionModel> result = await repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, x => x.Id == first.Id);
            Assert.Contains(result, x => x.Id == second.Id);
        }

        [Fact]
        public async Task GetTransactionAsync_WhenTransactionExists_ReturnsTransaction()
        {
            // Arrange
            await ClearCollectionAsync<TransactionModel>(CollectionName);

            TransactionRepository repository = CreateRepository();
            TransactionModel expected = CreateTransaction();

            await repository.UpsertAsync(expected);

            // Act
            TransactionModel result =
                await repository.GetTransactionAsync(expected.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.Id, result.Id);
            Assert.Equal(expected.Amount, result.Amount);
            Assert.Equal(expected.Description, result.Description);
        }

        [Fact]
        public async Task GetTransactionAsync_WhenTransactionDoesNotExist_ReturnsNull()
        {
            // Arrange
            await ClearCollectionAsync<TransactionModel>(CollectionName);

            TransactionRepository repository = CreateRepository();

            // Act
            TransactionModel result =
                await repository.GetTransactionAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpsertAsync_WhenTransactionDoesNotExist_InsertsTransaction()
        {
            // Arrange
            await ClearCollectionAsync<TransactionModel>(CollectionName);

            TransactionRepository repository = CreateRepository();
            TransactionModel transaction = CreateTransaction();

            // Act
            await repository.UpsertAsync(transaction);

            // Assert
            TransactionModel result =
                await repository.GetTransactionAsync(transaction.Id);

            Assert.NotNull(result);
            Assert.Equal(transaction.Id, result.Id);
            Assert.Equal(transaction.Amount, result.Amount);
            Assert.Equal(transaction.Description, result.Description);
        }

        [Fact]
        public async Task UpsertAsync_WhenTransactionExists_UpdatesTransaction()
        {
            // Arrange
            await ClearCollectionAsync<TransactionModel>(CollectionName);

            TransactionRepository repository = CreateRepository();
            Guid id = Guid.NewGuid();

            TransactionModel original = CreateTransaction(
                id,
                "Original description");

            await repository.UpsertAsync(original);

            TransactionModel updated = CreateTransaction(
                id,
                "Updated description");

            updated.Amount = 99.99;

            // Act
            await repository.UpsertAsync(updated);

            // Assert
            TransactionModel result =
                await repository.GetTransactionAsync(id);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal(99.99, result.Amount);
            Assert.Equal("Updated description", result.Description);

            List<TransactionModel> all = await repository.GetAllAsync();
            _ = Assert.Single(all);
        }

        [Fact]
        public async Task DeleteAsync_WhenTransactionExists_RemovesTransaction()
        {
            // Arrange
            await ClearCollectionAsync<TransactionModel>(CollectionName);

            TransactionRepository repository = CreateRepository();
            TransactionModel transaction = CreateTransaction();

            await repository.UpsertAsync(transaction);

            // Act
            await repository.DeleteAsync(transaction.Id);

            // Assert
            TransactionModel result =
                await repository.GetTransactionAsync(transaction.Id);

            Assert.Null(result);
            Assert.Empty(await repository.GetAllAsync());
        }
    }
}
