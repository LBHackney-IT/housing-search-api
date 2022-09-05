using Hackney.Core.Logging;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Gateways.Interfaces;
using HousingSearchApi.V1.UseCase.Interfaces;
using System.Threading.Tasks;

namespace HousingSearchApi.V1.UseCase
{
    public class GetStaffListUseCase : IGetStaffListUseCase
    {
        private readonly ISearchGateway _searchGateway;

        public GetStaffListUseCase(ISearchGateway searchGateway)
        {
            _searchGateway = searchGateway;
        }

        [LogCall]
        public async Task<GetStaffListResponse> ExecuteAsync(GetStaffListRequest staffListRequest)
        {
            return await _searchGateway.GetListOfStaffs(staffListRequest).ConfigureAwait(false);
        }
    }
}
