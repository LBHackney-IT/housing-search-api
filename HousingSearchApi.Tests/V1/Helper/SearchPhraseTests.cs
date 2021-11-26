using System;
using FluentAssertions;
using Hackney.Core.ElasticSearch;
using Hackney.Shared.HousingSearch.Gateways.Models.Persons;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Infrastructure.Factories;
using Nest;
using Xunit;

namespace HousingSearchApi.Tests.V1.Helper
{
    public class SearchPhraseTests
    {
        private readonly PersonQueryGenerator _sut;

        public SearchPhraseTests()
        {
            _sut = new PersonQueryGenerator(new QueryBuilder<QueryablePerson>(new WildCardAppenderAndPrepender()));
        }

        [Theory]
        [InlineData("")]
        [InlineData("AnyData")]
        [InlineData(null)]
        public void ShouldReturnNullIfRequestTypeIsUnknown(string searchText)
        {
            // Arrange + Act
            QueryContainer Func() => _sut.Create<HousingSearchRequest>(new GetAccountListRequest { SearchText = searchText }, new QueryContainerDescriptor<QueryablePerson>());

            // Assert
            Exception ex = Assert.Throws<ArgumentNullException>((Func<QueryContainer>) Func);

        }
    }
}
