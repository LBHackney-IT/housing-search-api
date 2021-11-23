using Hackney.Core.Logging;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.UseCase.Interfaces;
using System.Threading.Tasks;
using HousingSearchApi.V1.Gateways.Interfaces;

namespace HousingSearchApi.V1.UseCase
{
    public class GetPersonListUseCase : IGetPersonListUseCase
    {
        private readonly ISearchGateway _searchGateway;

        public GetPersonListUseCase(ISearchGateway searchGateway)
        {
            _searchGateway = searchGateway;
        }
        [LogCall]
        public async Task<GetPersonListResponse> ExecuteAsync(HousingSearchRequest housingSearchRequest)
        {
            return await _searchGateway.GetListOfPersons(housingSearchRequest).ConfigureAwait(false);
        }
    }
}
