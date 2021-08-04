using System.Threading.Tasks;
using Hackney.Core.Logging;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Response;
using HousingSearchApi.V1.Interfaces;
using HousingSearchApi.V1.UseCase.Interfaces;

namespace HousingSearchApi.V1.UseCase
{
    public class GetTenureListUseCase : IGetTenureListUseCase
    {
        private readonly ISearchPersonsGateway _searchPersonsGateway;

        public GetTenureListUseCase(ISearchPersonsGateway searchPersonsGateway)
        {
            _searchPersonsGateway = searchPersonsGateway;
        }

        [LogCall]
        public async Task<GetTenureListResponse> ExecuteAsync(GetTenureListRequest getTenureListRequest)
        {
            return await _searchPersonsGateway.GetListOfTenures(getTenureListRequest).ConfigureAwait(false);
        }
    }
}
