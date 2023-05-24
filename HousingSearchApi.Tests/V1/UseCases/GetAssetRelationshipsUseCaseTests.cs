using AutoFixture;
using FluentAssertions;
using Hackney.Shared.HousingSearch.Gateways.Models.Assets;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Factories;
using HousingSearchApi.V1.Gateways;
using HousingSearchApi.V1.Gateways.Interfaces;
using HousingSearchApi.V1.Helper.Interfaces;
using HousingSearchApi.V1.Interfaces;
using HousingSearchApi.V1.UseCase;
using Moq;
using Nest;
using System.Threading.Tasks;
using Xunit;

namespace HousingSearchApi.Tests.V1.UseCases
{
    public class GetAssetRelationshipsUseCaseTests
    {
        private readonly GetAssetRelationshipsUseCase _sut;
        private readonly Mock<ISearchGateway> _searchGatewayMock;
        public GetAssetRelationshipsUseCaseTests()
        {
            _searchGatewayMock = new Mock<ISearchGateway>();

            _sut = new GetAssetRelationshipsUseCase(_searchGatewayMock.Object);
        }
    }
}
