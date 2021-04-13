using System.Threading.Tasks;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Response;
using HousingSearchApi.V1.Interfaces;
using HousingSearchApi.V1.UseCase.Interfaces;

namespace HousingSearchApi.V1.UseCase
{
    public class GetPersonListUseCase : IGetPersonListUseCase
    {
        private readonly ISearchPersonsGateway _searchPersonsGateway;

        public GetPersonListUseCase(ISearchPersonsGateway searchPersonsGateway)
        {
            _searchPersonsGateway = searchPersonsGateway;
        }

        public async Task<GetPersonListResponse> ExecuteAsync(GetPersonListRequest getPersonListRequest)
        {
            return await _searchPersonsGateway.GetListOfPersons(getPersonListRequest);
        }
    }
}
