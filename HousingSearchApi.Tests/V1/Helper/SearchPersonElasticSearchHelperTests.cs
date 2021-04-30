using System;
using System.Threading.Tasks;
using FluentAssertions;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Interfaces;
using HousingSearchApi.V1.Interfaces.Sorting;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Xunit;

namespace HousingSearchApi.Tests.V1.Helper
{
    public class SearchPersonElasticSearchHelperTests
    {
        private SearchPersonElasticSearchHelper _classUnderTest;
        private ServiceCollection _services;

        public SearchPersonElasticSearchHelperTests()
        {
            _services = new ServiceCollection();
            Startup.ConfigureServices(_services);

            _classUnderTest = new SearchPersonElasticSearchHelper(_services.BuildServiceProvider().GetService<IElasticClient>(),
                _services.BuildServiceProvider().GetService<ISearchPersonsQueryContainerOrchestrator>(),
                _services.BuildServiceProvider().GetService<IPagingHelper>(),
                _services.BuildServiceProvider().GetService<IPersonListSortFactory>());
        }

        [Fact]
        // In our case, the query should be a SHOULD (the ElasticSearch option for OR), followed by wildcards for :
        // firstname, surname, middlename,prefferedfirstname, preferredsurname, dateofbirth
        public async Task WhenCallingElasticSearchHelperShouldGenerateTheRightQuery()
        {
            // arrange
            var searchText = "abc";
            // correctQuery is the query NEST generates behind the scenes to, in turn, send to the ES server.
            // In our case, it wildcards the 6 fields by which we are searching.
            var correctQuery =
                "{\"should\":[{\"wildcard\":{\"firstname\":{\"value\":\"*{0}*\"}}},{\"wildcard\":{\"surname\":{\"value\":\"*{0}*\"}}}]}";
            correctQuery = correctQuery.Replace("{0}", searchText, StringComparison.CurrentCulture);

            // act
            var response = await _classUnderTest.Search(new GetPersonListRequest { SearchText = searchText }).ConfigureAwait(false);

            // assert
            response.DebugInformation.IndexOf(correctQuery, StringComparison.CurrentCulture).Should().BeGreaterOrEqualTo(0);
        }
    }
}
