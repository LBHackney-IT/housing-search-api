using FluentAssertions;
using Hackney.Shared.HousingSearch.Domain.Accounts;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Gateways.Interfaces;
using HousingSearchApi.V1.UseCase;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HousingSearchApi.Tests.V1.UseCase
{
    public class GetPersonListUseCaseTests
    {
        private readonly Mock<ISearchGateway> _searchGateway;
        private readonly GetPersonListUseCase _getPersonListUseCase;
        public GetPersonListUseCaseTests()
        {
            new LogCallAspectFixture().RunBeforeTests();
            _searchGateway = new Mock<ISearchGateway>();
            _getPersonListUseCase = new GetPersonListUseCase(_searchGateway.Object);
        }

        [Fact]
        public async Task ExecuteAsyncReturnsNotNull()
        {
            var housingSearchRequest = new GetPersonListRequest();
            var personListResponse = new GetPersonListResponse();
            var accounts = new List<Account>();
            _searchGateway.Setup(x => x.GetListOfPersons(It.IsAny<GetPersonListRequest>())).ReturnsAsync(personListResponse);
            _searchGateway.Setup(x => x.GetAccountListByTenureIdsAsync(It.IsAny<List<string>>())).ReturnsAsync(accounts);
            var personListResponseResult = await _getPersonListUseCase.ExecuteAsync(housingSearchRequest).ConfigureAwait(false);
            personListResponseResult.Should().NotBeNull();
        }

        [Fact]
        public async Task ExecuteAsyncРousingSearchRequestIsNullThrowsArgumentNullException()
        {
            GetPersonListRequest housingSearchRequest = null;
            _searchGateway.Setup(x => x.GetListOfPersons(It.IsAny<GetPersonListRequest>())).ReturnsAsync((GetPersonListResponse) null);
            _searchGateway.Setup(x => x.GetAccountListByTenureIdsAsync(It.IsAny<List<string>>())).ReturnsAsync((List<Account>) null);
            Func<Task> act = async () => await _getPersonListUseCase.ExecuteAsync(housingSearchRequest).ConfigureAwait(false);
            await act.Should().ThrowAsync<ArgumentNullException>().ConfigureAwait(false);
        }
    }
}
