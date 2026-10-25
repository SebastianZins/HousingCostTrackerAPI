using HousingCostTrackerAPI.Models.Models.Person;
using HousingCostTrackerAPI.Repositories.Repositories;

namespace HousingCostTrackerAPI.UnitTests.RepositoryTests
{
    public class PersonRepositoryTest : MongoRepositoryTestBase
    {
        private const string CollectionName = "Persons";

        private PersonRepository CreateRepository()
        {
            return new(Database);
        }

        private static PersonModel CreatePerson(
            string firstName,
            string lastName,
            string shortName,
            Guid? id = null)
        {
            return new PersonModel
            {
                Id = id ?? Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                ShortName = shortName,
            };
        }

        [Fact]
        public async Task GetAllAsync_WhenPersonsExist_ReturnsAllPersons()
        {
            // Arrange
            await ClearCollectionAsync<PersonModel>(CollectionName);

            PersonRepository repository = CreateRepository();
            PersonModel first = CreatePerson("First F", "First L", "First S");
            PersonModel second = CreatePerson("Second F", "Second L", "Second S");

            await repository.UpsertAsync(first);
            await repository.UpsertAsync(second);

            // Act
            List<PersonModel> result = await repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, x => x.Id == first.Id);
            Assert.Contains(result, x => x.Id == second.Id);
        }

        [Fact]
        public async Task GetPersonAsync_WhenPersonExists_ReturnsPerson()
        {
            // Arrange
            await ClearCollectionAsync<PersonModel>(CollectionName);

            PersonRepository repository = CreateRepository();
            PersonModel expected = CreatePerson("First", "Last", "Short");

            await repository.UpsertAsync(expected);

            // Act
            PersonModel result =
                await repository.GetPersonAsync(expected.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.Id, result.Id);
            Assert.Equal(expected.FirstName, result.FirstName);
            Assert.Equal(expected.LastName, result.LastName);
            Assert.Equal(expected.ShortName, result.ShortName);
        }

        [Fact]
        public async Task GetPersonAsync_WhenPersonDoesNotExist_ReturnsNull()
        {
            // Arrange
            await ClearCollectionAsync<PersonModel>(CollectionName);

            PersonRepository repository = CreateRepository();

            // Act
            PersonModel result =
                await repository.GetPersonAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpsertAsync_WhenPersonDoesNotExist_InsertsPerson()
        {
            // Arrange
            await ClearCollectionAsync<PersonModel>(CollectionName);

            PersonRepository repository = CreateRepository();
            PersonModel Person = CreatePerson("First", "Last", "Short");

            // Act
            await repository.UpsertAsync(Person);

            // Assert
            PersonModel result =
                await repository.GetPersonAsync(Person.Id);

            Assert.NotNull(result);
            Assert.Equal(Person.Id, result.Id);
            Assert.Equal(Person.FirstName, result.FirstName);
            Assert.Equal(Person.LastName, result.LastName);
            Assert.Equal(Person.ShortName, result.ShortName);
        }

        [Fact]
        public async Task UpsertAsync_WhenPersonExists_UpdatesPerson()
        {
            // Arrange
            await ClearCollectionAsync<PersonModel>(CollectionName);

            PersonRepository repository = CreateRepository();
            Guid id = Guid.NewGuid();

            PersonModel original = CreatePerson(
                "First OG", "Last OG", "Short OG", id);

            await repository.UpsertAsync(original);

            PersonModel updated = CreatePerson(
               "First UP", "Last UP", "Short UP", id);

            // Act
            await repository.UpsertAsync(updated);

            // Assert
            PersonModel result =
                await repository.GetPersonAsync(id);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("First UP", result.FirstName);
            Assert.Equal("Last UP", result.LastName);
            Assert.Equal("Short UP", result.ShortName);

            List<PersonModel> all = await repository.GetAllAsync();
            _ = Assert.Single(all);
        }

        [Fact]
        public async Task DeleteAsync_WhenPersonExists_RemovesPerson()
        {
            // Arrange
            await ClearCollectionAsync<PersonModel>(CollectionName);

            PersonRepository repository = CreateRepository();
            PersonModel Person = CreatePerson("First", "Last", "Short");

            await repository.UpsertAsync(Person);

            // Act
            await repository.DeleteAsync(Person.Id);

            // Assert
            PersonModel result =
                await repository.GetPersonAsync(Person.Id);

            Assert.Null(result);
            Assert.Empty(await repository.GetAllAsync());
        }
    }
}
