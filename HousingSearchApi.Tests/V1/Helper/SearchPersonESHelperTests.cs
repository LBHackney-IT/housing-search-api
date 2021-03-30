using System.Threading.Tasks;
using FluentAssertions;
using HousingSearchApi.V1.Domain;
using HousingSearchApi.V1.Gateways;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Xunit;

namespace HousingSearchApi.Tests.V1.Helper
{
    public class SearchPersonESHelperTests
    {
        private SearchPersonESHelper _classUnderTest;
        private ServiceCollection _services;

        public SearchPersonESHelperTests()
        {
            _services = new ServiceCollection();
            Startup.ConfigureServices(_services);

            _classUnderTest = new SearchPersonESHelper(_services.BuildServiceProvider().GetService<IElasticClient>());
        }

        [Fact]
        // In our case, the query should be a SHOULD (the ES option for OR), followed by wildcards for :
        // firstname, lastname, middlename,prefferedfirstname, preferredsurname, dateofbirth
        public async Task WhenCallingESHelperShouldGenerateTheRightQuery()
        {
            // given
            var searchText = "abc";
            // correctQuery is the query NEST generates behind the scenes to, in turn, send to the ES server.
            // In our case, it wildcards the 6 fields by which we are searching.
            var correctQuery =
                "{\"should\":[{\"wildcard\":{\"firstname\":{\"value\":\"*{0}*\"}}},{\"wildcard\":{\"surname\":{\"value\":\"*{0}*\"}}},{\"wildcard\":{\"middleName\":{\"value\":\"*{0}*\"}}},{\"wildcard\":{\"dateOfBirth\":{\"value\":\"*{0}*\"}}},{\"wildcard\":{\"preferredSurname\":{\"value\":\"*{0}*\"}}},{\"wildcard\":{\"preferredFirstname\":{\"value\":\"*{0}*\"}}}]}";
            correctQuery = correctQuery.Replace("{0}", searchText);

            // when
            var response = await _classUnderTest.Search(new GetPersonListRequest { SearchText = searchText });

            // then
            response.DebugInformation.IndexOf(correctQuery).Should().BeGreaterOrEqualTo(0);
        }
    }
}
