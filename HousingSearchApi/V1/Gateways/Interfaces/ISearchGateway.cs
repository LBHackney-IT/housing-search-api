using Hackney.Shared.HousingSearch.Domain.Asset;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Boundary.Responses.Transactions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HousingSearchApi.V1.Gateways.Interfaces
{
    public interface ISearchGateway
    {
        Task<GetPersonListResponse> GetListOfPersons(GetPersonListRequest query);
        Task<GetTenureListResponse> GetListOfTenures(GetTenureListRequest query);
        Task<GetAllTenureListResponse> GetListOfTenuresSets(GetAllTenureListRequest query);
        Task<GetAssetListResponse> GetListOfAssets(GetAssetListRequest query);
        Task<GetAllAssetListResponse> GetListOfAssetsSets(GetAllAssetListRequest query);
        Task<GetAccountListResponse> GetListOfAccounts(GetAccountListRequest query);
        Task<GetTransactionListResponse> GetListOfTransactions(GetTransactionListRequest request);
        Task<GetProcessListResponse> GetListOfProcesses(GetProcessListRequest query);
        Task<List<Asset>> GetChildAssets(GetAssetRelationshipsRequest request);
    }
}
