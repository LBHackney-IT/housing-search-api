using Xunit;

namespace HousingSearchApi.Tests
{
    [CollectionDefinition("ES collection")]
    public class ESCollection : ICollectionFixture<ESFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
