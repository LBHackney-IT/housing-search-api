using System.Linq;
using NUnit.Framework;
using HousingSearchApi.Tests.V1.Helper;

namespace HousingSearchApi.Tests.V1.Infrastructure
{
    [TestFixture]
    [Ignore("Not needed until we decide on DB")]
    public class DatabaseContextTest : DatabaseTests
    {
        [Test]
        public void CanGetADatabaseEntity()
        {
            var databaseEntity = DatabaseEntityHelper.CreateDatabaseEntity();

            DatabaseContext.Add(databaseEntity);
            DatabaseContext.SaveChanges();

            var result = DatabaseContext.DatabaseEntities.ToList().FirstOrDefault();

            Assert.AreEqual(result, databaseEntity);
        }
    }
}
