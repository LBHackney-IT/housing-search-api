using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Response;
using System.Threading.Tasks;

namespace HousingSearchApi.V1.Interfaces
{
    public interface ISearchPersonsGateway
    {
        Task<GetPersonListResponse> GetListOfPersons(GetPersonListRequest query);
        Task<GetTenureListResponse> GetListOfTenures(GetTenureListRequest getTenureListRequest);
    }
}
