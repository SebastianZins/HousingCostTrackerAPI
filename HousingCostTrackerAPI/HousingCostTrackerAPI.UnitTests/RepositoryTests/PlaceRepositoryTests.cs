using HousingCostTrackerAPI.Models.Models.Place;
using HousingCostTrackerAPI.Repositories.Repositories;

namespace HousingCostTrackerAPI.UnitTests.RepositoryTests
{
    public class PlaceRepositoryTest : MongoRepositoryTestBase
    {
        private const string CollectionName = "Places";

        private PlaceRepository CreateRepository()
        {
            return new(Database);
        }

        private static PlaceModel CreatePlace(
            string name,
            Guid? id = null)
        {
            return new PlaceModel
            {
                Id = id ?? Guid.NewGuid(),
                Name = name
            };
        }

        [Fact]
        public async Task GetAllAsync_WhenPlacesExist_ReturnsAllPlaces()
        {
            // Arrange
            await ClearCollectionAsync<PlaceModel>(CollectionName);

            PlaceRepository repository = CreateRepository();
            PlaceModel first = CreatePlace("First");
            PlaceModel second = CreatePlace("Second");

            await repository.UpsertAsync(first);
            await repository.UpsertAsync(second);

            // Act
            List<PlaceModel> result = await repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, x => x.Id == first.Id);
            Assert.Contains(result, x => x.Id == second.Id);
        }

        [Fact]
        public async Task GetPlaceAsync_WhenPlaceExists_ReturnsPlace()
        {
            // Arrange
            await ClearCollectionAsync<PlaceModel>(CollectionName);

            PlaceRepository repository = CreateRepository();
            PlaceModel expected = CreatePlace("Name");

            await repository.UpsertAsync(expected);

            // Act
            PlaceModel result =
                await repository.GetPlaceAsync(expected.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.Id, result.Id);
            Assert.Equal(expected.Name, result.Name);
        }

        [Fact]
        public async Task GetPlaceAsync_WhenPlaceDoesNotExist_ReturnsNull()
        {
            // Arrange
            await ClearCollectionAsync<PlaceModel>(CollectionName);

            PlaceRepository repository = CreateRepository();

            // Act
            PlaceModel result =
                await repository.GetPlaceAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpsertAsync_WhenPlaceDoesNotExist_InsertsPlace()
        {
            // Arrange
            await ClearCollectionAsync<PlaceModel>(CollectionName);

            PlaceRepository repository = CreateRepository();
            PlaceModel Place = CreatePlace("Name");

            // Act
            await repository.UpsertAsync(Place);

            // Assert
            PlaceModel result =
                await repository.GetPlaceAsync(Place.Id);

            Assert.NotNull(result);
            Assert.Equal(Place.Id, result.Id);
            Assert.Equal(Place.Name, result.Name);
        }

        [Fact]
        public async Task UpsertAsync_WhenPlaceExists_UpdatesPlace()
        {
            // Arrange
            await ClearCollectionAsync<PlaceModel>(CollectionName);

            PlaceRepository repository = CreateRepository();
            Guid id = Guid.NewGuid();

            PlaceModel original = CreatePlace(
                "Original", id);

            await repository.UpsertAsync(original);

            PlaceModel updated = CreatePlace(
                "Updated", id);

            // Act
            await repository.UpsertAsync(updated);

            // Assert
            PlaceModel result =
                await repository.GetPlaceAsync(id);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("Updated", result.Name);

            List<PlaceModel> all = await repository.GetAllAsync();
            _ = Assert.Single(all);
        }

        [Fact]
        public async Task DeleteAsync_WhenPlaceExists_RemovesPlace()
        {
            // Arrange
            await ClearCollectionAsync<PlaceModel>(CollectionName);

            PlaceRepository repository = CreateRepository();
            PlaceModel Place = CreatePlace("Name");

            await repository.UpsertAsync(Place);

            // Act
            await repository.DeleteAsync(Place.Id);

            // Assert
            PlaceModel result =
                await repository.GetPlaceAsync(Place.Id);

            Assert.Null(result);
            Assert.Empty(await repository.GetAllAsync());
        }
    }
}
