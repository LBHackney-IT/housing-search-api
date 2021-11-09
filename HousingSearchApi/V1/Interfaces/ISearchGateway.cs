using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using System.Threading.Tasks;
using HousingSearchApi.V1.Boundary.Responses.Transactions;

namespace HousingSearchApi.V1.Interfaces
{
    public interface ISearchGateway
    {
        Task<GetPersonListResponse> GetListOfPersons(HousingSearchRequest query);
        Task<GetTenureListResponse> GetListOfTenures(HousingSearchRequest query);
        Task<GetAssetListResponse> GetListOfAssets(HousingSearchRequest query);
        Task<GetTransactionListResponse> GetListOfTransactions(GetTransactionSearchRequest request);
    }
}
