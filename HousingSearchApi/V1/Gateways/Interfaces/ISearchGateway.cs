using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using System.Threading.Tasks;

namespace HousingSearchApi.V1.Gateways.Interfaces
{
    public interface ISearchGateway
    {
        Task<GetPersonListResponse> GetListOfPersons(HousingSearchRequest query);
        Task<GetTenureListResponse> GetListOfTenures(HousingSearchRequest query);
        Task<GetAssetListResponse> GetListOfAssets(HousingSearchRequest query);
        Task<GetAccountListResponse> GetListOfAccounts(HousingSearchRequest query);
    }
}
