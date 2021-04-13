using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using FluentAssertions;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Interfaces;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Nest;
using Xunit;
using QueryablePerson = HousingSearchApi.V1.Infrastructure.QueryablePerson;

namespace HousingSearchApi.Tests.V1.Helper
{
    public class SearchSurnamesTests
    {
        private SearchSurnames _sut;

        public SearchSurnamesTests()
        {
            _sut = new SearchSurnames();
        }

        [Fact(Skip = "Testing ES integration testing")]
        public async Task Something()
        {
            DockerClient dockerClient = new DockerClientConfiguration()
                .CreateClient();

            var parameters = new ContainersListParameters();
            parameters.All = true;

            ContainerListResponse container = (await dockerClient.Containers.ListContainersAsync(parameters)).FirstOrDefault(x => x.Image == "test-elasticsearch");
            if (container.State != "running")
            {
                await dockerClient.Containers.StartContainerAsync(container.ID, new ContainerStartParameters());
            }
        }

        [Xunit.Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldReturnNullIfRequestSearchTextIsEmpty(string searchText)
        {
            // Arrange + Act
            var result = _sut.Create(new GetPersonListRequest { SearchText = searchText }, new QueryContainerDescriptor<QueryablePerson>());

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ShouldReturnQueryThatSearchesForNames()
        {
            // Arrange
            var nameToSearchFor = "SomeName";

            // Act
            var result = _sut.Create(new GetPersonListRequest { SearchText = nameToSearchFor },
                new QueryContainerDescriptor<QueryablePerson>());

            // Assert
            (result as IQueryContainer).Wildcard.Field.Expression.Print().Should().Be("f => f.Surname");
            (result as IQueryContainer).Wildcard.Value.Should().Be($"*{nameToSearchFor.ToLower()}*");
        }
    }
}
