using System.Linq;
using BaseApi.Tests.V1.Helper;
using NUnit.Framework;

namespace BaseApi.Tests.V1.Infrastructure
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
