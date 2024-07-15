using FluentAssertions;
using Hackney.Core.ElasticSearch.Interfaces;
using Hackney.Shared.HousingSearch.Gateways.Models.Assets;
using HousingSearchApi.V1.Infrastructure.Core;
using Moq;
using Nest;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Hackney.Core.Tests.ElasticSearch
{
    public class FilterQueryBuilderTests
    {
        private readonly FilterQueryBuilder<QueryableAsset> _sut;
        private readonly Mock<IWildCardAppenderAndPrepender> _wildcardAppenderAndPrependerMock;
        private readonly QueryContainerDescriptor<QueryableAsset> _queryContainerDesc;
        public FilterQueryBuilderTests()
        {
            _wildcardAppenderAndPrependerMock = new Mock<IWildCardAppenderAndPrepender>();
            _queryContainerDesc = new QueryContainerDescriptor<QueryableAsset>();

            _sut = new FilterQueryBuilder<QueryableAsset>(_wildcardAppenderAndPrependerMock.Object);
        }

        [Fact]
        public void WhenCreatingQuery_WithWildstar_ResultantQueryShouldHaveOneSubquery()
        {
            // Arrange 
            string searchText = "12 Pitcairn Street";
            var fields = new List<string> { "field1", "field2" };
            _wildcardAppenderAndPrependerMock.Setup(x => x.Process(It.IsAny<string>()))
                .Returns(new List<string> { "*12*", "*Pitcairn*", "*Street*" });

            // Act
            QueryContainer query = _sut.WithWildstarQuery(searchText, fields)
                .Build(_queryContainerDesc);

            // Assert
            var container = (query as IQueryContainer).Bool.Should;

            container.Count().Should().Be(2);
            container.Count(q => q != null).Should().Be(1);
        }

        [Fact]
        public void WhenCreatingSimpleQuery_WithWildstar_ResultantQueryBeOfSimpleType()
        {
            // Arrange 
            string searchText = "17 Dulwich Park Avenue";
            var fields = new List<string> { "field11", "field12" };

            // Act
            QueryContainer query = _sut.BuildSimpleQuery(_queryContainerDesc, searchText, fields);

            // Assert
            var container = (query as IQueryContainer).SimpleQueryString;

            container.Should().NotBeNull();
            container.Fields.Count().Should().Be(2);
            container.Query.Should().Be(searchText);
        }

        [Fact]
        public void QueryBuilder_WhenAddingMultipleFilterQueries_GeneratesCorrectClauses()
        {
            // Arrange 
            _sut
                .WithFilterQuery("Dwelling,Block", new List<string> { "assetType" })
                .WithFilterQuery("RES,OCC", new List<string> { "assetManagement.propertyOccupiedStatus" });

            // Act
            var query = _sut.Build(_queryContainerDesc);

            // Assert
            var container = query as IQueryContainer;
            container.Should().NotBeNull();

            var mustClause = container.Bool.Must.Where(c => c != null).Cast<IQueryContainer>().ToList();
            mustClause.Should().SatisfyRespectively(
                e => e.Bool.Should.Should().NotBeNull(),
                e => e.Bool.Should.Should().NotBeNull()
            );

            var firstClause = mustClause[0].Bool.Should.Cast<IQueryContainer>().ToList();
            var secondClause = mustClause[1].Bool.Should.Cast<IQueryContainer>().ToList();

            firstClause.SelectMany(c => c.QueryString.Fields)
                .Should().OnlyContain(x => x.Name == "assetType");
            firstClause.Select(c => c.QueryString.Query).Should().SatisfyRespectively(
                e => e.Should().Be("Dwelling"),
                e => e.Should().Be("Block")
            );

            secondClause.SelectMany(c => c.QueryString.Fields)
                .Should().OnlyContain(x => x.Name == "assetManagement.propertyOccupiedStatus");
            secondClause.Select(c => c.QueryString.Query).Should().SatisfyRespectively(
                e => e.Should().Be("RES"),
                e => e.Should().Be("OCC")
            );
        }

        [Fact]
        public void QueryBuilder_WhenCombiningMultipleQueryTypes_GeneratesCorrectQueryStructure()
        {
            // Arrange 
            _wildcardAppenderAndPrependerMock.Setup(x => x.Process(It.IsAny<string>()))
                .Returns(new List<string> { "*12*", "*Pitcairn*", "*Street*" });
            _sut
                .WithMultipleFilterQuery("1", new List<string> { "assetCharacteristics.numberOfBedrooms" })
                .WithWildstarBoolQuery("12 Pitcairn Street", new List<string> { "assetAddress.addressLine1", "assetAddress.uprn", "assetAddress.postCode" })
                .WithWildstarQuery("12 Pitcairn Street", new List<string> { "assetAddress.addressLine1" })
                .WithExactQuery("12 Pitcairn Street", new List<string> { "assetAddress.addressLine1" })
                .WithFilterQuery("Dwelling,Block", new List<string> { "assetType" })
                .WithFilterQuery("RES,OCC", new List<string> { "assetManagement.propertyOccupiedStatus" });

            // Act
            var query = _sut.Build(_queryContainerDesc);

            // Assert
            var container = query as IQueryContainer;

            var mustClause = container.Bool.Must.Where(c => c != null)
                .Cast<IQueryContainer>()
                .ToList();
            mustClause.Should().SatisfyRespectively(
                e => e.Bool.Should.Should().NotBeNull(),
                e => e.Bool.Should.Should().NotBeNull(),
                e => e.Bool.Must.Should().BeAssignableTo<IList<QueryContainer>>()
                        .Which.Should().SatisfyRespectively(
                            i => ((IQueryContainer) i).Bool.Must.Should().NotBeNull(),
                            i => ((IQueryContainer) i).Bool.Should.Should().NotBeNull()
                        )
            );
        }


    }
}
