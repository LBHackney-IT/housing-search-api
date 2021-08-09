// TODO: 1 Return when last commit
//using Hackney.Core.Logging;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Interfaces;
using HousingSearchApi.V1.UseCase.Interfaces;
using System.Threading.Tasks;

namespace HousingSearchApi.V1.UseCase
{
    public class GetPersonListUseCase : IGetPersonListUseCase
    {
        private readonly ISearchPersonsGateway _searchPersonsGateway;

        public GetPersonListUseCase(ISearchPersonsGateway searchPersonsGateway)
        {
            _searchPersonsGateway = searchPersonsGateway;
        }
        // TODO: 1 Return when last commit
        //[LogCall]
        public async Task<GetPersonListResponse> ExecuteAsync(GetPersonListRequest getPersonListRequest)
        {
            return await _searchPersonsGateway.GetListOfPersons(getPersonListRequest).ConfigureAwait(false);
        }
    }
}
