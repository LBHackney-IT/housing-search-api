using Xunit;

namespace HousingSearchApi.Tests.V1.Helper
{
    [CollectionDefinition("ElasticSearch collection")]
    public class ElasticSearchCollection : ICollectionFixture<ElasticSearchFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
