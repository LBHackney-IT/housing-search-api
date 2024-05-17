using AutoFixture;
using Bogus;
using Hackney.Shared.HousingSearch.Gateways.Models.Tenures;

namespace HousingSearchApi.Tests.V1.TestHelpers
{
    public class QueryableTenureHelper
    {
        private static readonly string _startOfTenureDateFormat = "yyyy-MM-ddTHH:mm:ssZ";
        private static readonly Faker _faker = new();
        private static readonly Fixture _fixture = new();

        public static QueryableTenure CreateQueyableTenure()
        {
            return _fixture
                .Build<QueryableTenure>()
                .With(x => x.StartOfTenureDate, _faker.Date.Past().ToString(_startOfTenureDateFormat))
                .Create();
        }
    }
}
