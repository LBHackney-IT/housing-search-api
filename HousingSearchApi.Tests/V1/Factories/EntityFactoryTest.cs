using HousingSearchApi.V1.Factories;
using FluentAssertions;
using NUnit.Framework;
using HousingSearchApi.V1.Infrastructure;

namespace HousingSearchApi.Tests.V1.Factories
{
    [TestFixture]
    public class EntityFactoryTest
    {
        //TODO: add assertions for all the fields being mapped in `EntityFactory.ToDomain()`. Also be sure to add test cases for
        // any edge cases that might exist.
        [Test]
        public void CanMapADatabaseEntityToADomainObject()
        {
            var databaseEntity = new DatabaseEntity();
            var entity = databaseEntity.ToDomain();

            databaseEntity.Id.Should().Be(entity.Id);
            databaseEntity.CreatedAt.Should().BeSameDateAs(entity.CreatedAt);
        }
    }
}
