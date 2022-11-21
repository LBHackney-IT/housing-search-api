using FluentAssertions;
using Hackney.Shared.HousingSearch.Gateways.Models.Processes;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Infrastructure.Sorting;
using Xunit;

namespace HousingSearchApi.Tests.V1.Helper
{
    public class ProcessListSortFactoryTests
    {
        private SortFactory _sut;

        public ProcessListSortFactoryTests()
        {
            _sut = new SortFactory();
        }

        [Fact]
        public void ShouldSortAsDefault()
        {
            // Arrange + Act
            var result = _sut.Create<QueryableProcess, GetProcessListRequest>(new GetProcessListRequest());

            // Assert
            result.Should().BeOfType(typeof(DefaultSort<QueryableProcess>));
        }

        [Fact]
        public void ShouldReturnProcessesRelatedentitiesAscByPersonNameAndAsc()
        {
            // Arrange + Act
            var result = _sut.Create<QueryableProcess, GetProcessListRequest>(new GetProcessListRequest { SortBy = "name", IsDesc = false });

            // Assert
            result.Should().BeOfType(typeof(PersonNameAsc));
        }

        [Fact]
        public void ShouldReturnProcessesRelatedentitiesDescByPersonNameAndDesc()
        {
            // Arrange + Act
            var result = _sut.Create<QueryableProcess, GetProcessListRequest>(new GetProcessListRequest { SortBy = "name", IsDesc = true });

            // Assert
            result.Should().BeOfType(typeof(PersonNameDesc));
        }

        [Fact]
        public void ShouldReturnProcessesNameDescByProcessAndDesc()
        {
            // Arrange + Act
            var result = _sut.Create<QueryableProcess, GetProcessListRequest>(new GetProcessListRequest { SortBy = "process", IsDesc = true });

            // Assert
            result.Should().BeOfType(typeof(ProcessNameDesc));
        }

        [Fact]
        public void ShouldReturnProcessesNameAscByProcessAndAsc()
        {
            // Arrange + Act
            var result = _sut.Create<QueryableProcess, GetProcessListRequest>(new GetProcessListRequest { SortBy = "process", IsDesc = false });

            // Assert
            result.Should().BeOfType(typeof(ProcessNameAsc));
        }

        [Fact]
        public void ShouldReturnProcessesPatchNameDescByPatchAndDesc()
        {
            // Arrange + Act
            var result = _sut.Create<QueryableProcess, GetProcessListRequest>(new GetProcessListRequest { SortBy = "patch", IsDesc = true });

            // Assert
            result.Should().BeOfType(typeof(PatchDesc));
        }

        [Fact]
        public void ShouldReturnProcessesPatchNameAscByPatchAndAsc()
        {
            // Arrange + Act
            var result = _sut.Create<QueryableProcess, GetProcessListRequest>(new GetProcessListRequest { SortBy = "patch", IsDesc = false });

            // Assert
            result.Should().BeOfType(typeof(PatchAsc));
        }

        [Fact]
        public void ShouldReturnProcessesStateDescByStateAndDesc()
        {
            // Arrange + Act
            var result = _sut.Create<QueryableProcess, GetProcessListRequest>(new GetProcessListRequest { SortBy = "state", IsDesc = true });

            // Assert
            result.Should().BeOfType(typeof(StateDesc));
        }

        [Fact]
        public void ShouldReturnProcessesStateAscByStateAndAsc()
        {
            // Arrange + Act
            var result = _sut.Create<QueryableProcess, GetProcessListRequest>(new GetProcessListRequest { SortBy = "state", IsDesc = false });

            // Assert
            result.Should().BeOfType(typeof(StateAsc));
        }
    }
}
