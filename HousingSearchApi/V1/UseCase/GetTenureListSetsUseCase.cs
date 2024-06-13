using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Gateways.Interfaces;
using HousingSearchApi.V1.UseCase.Interfaces;
using System.Threading.Tasks;

namespace HousingSearchApi.V1.UseCase
{
    public class GetTenureListSetsUseCase : IGetTenureListSetsUseCase
    {
        private readonly ISearchGateway _searchGateway;

        public GetTenureListSetsUseCase(ISearchGateway searchGateway)
        {
            _searchGateway = searchGateway;
        }
        public async Task<GetAllTenureListResponse> ExecuteAsync(GetAllTenureListRequest request)
        {
            return await _searchGateway.GetListOfTenuresSets(request).ConfigureAwait(false);
        }
    }
}
