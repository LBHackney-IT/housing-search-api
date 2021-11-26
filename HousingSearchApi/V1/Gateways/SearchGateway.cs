using Hackney.Core.Logging;
using Hackney.Shared.HousingSearch.Gateways.Models.Accounts;
using Hackney.Shared.HousingSearch.Gateways.Models.Assets;
using Hackney.Shared.HousingSearch.Gateways.Models.Transactions;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Boundary.Responses.Transactions;
using HousingSearchApi.V1.Factories;
using HousingSearchApi.V1.Gateways.Interfaces;
using HousingSearchApi.V1.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using QueryablePerson = Hackney.Shared.HousingSearch.Gateways.Models.Persons.QueryablePerson;
using QueryableTenure = Hackney.Shared.HousingSearch.Gateways.Models.Tenures.QueryableTenure;

namespace HousingSearchApi.V1.Gateways
{
    public class SearchGateway : ISearchGateway
    {
        private readonly IElasticSearchWrapper _elasticSearchWrapper;

        public SearchGateway(IElasticSearchWrapper elasticSearchWrapper)
        {
            _elasticSearchWrapper = elasticSearchWrapper;
        }

        [LogCall]
        public async Task<GetPersonListResponse> GetListOfPersons(GetPersonListRequest query)
        {
            var searchResponse = await _elasticSearchWrapper.Search<QueryablePerson, GetPersonListRequest>(query).ConfigureAwait(false);
            var personListResponse = new GetPersonListResponse();

            personListResponse.Persons.AddRange(searchResponse.Documents.Select(queryablePerson =>
                queryablePerson.Create())
            );

            personListResponse.SetTotal(searchResponse.Total);

            return personListResponse;
        }

        [LogCall]
        public async Task<GetTenureListResponse> GetListOfTenures(GetTenureListRequest query)
        {
            var searchResponse = await _elasticSearchWrapper.Search<QueryableTenure, GetTenureListRequest>(query).ConfigureAwait(false);
            var tenureListResponse = new GetTenureListResponse();

            tenureListResponse.Tenures.AddRange(searchResponse.Documents.Select(queryableTenure =>
                queryableTenure.Create())
            );

            tenureListResponse.SetTotal(searchResponse.Total);

            return tenureListResponse;
        }

        [LogCall]
        public async Task<GetAssetListResponse> GetListOfAssets(GetAssetListRequest query)
        {
            var searchResponse = await _elasticSearchWrapper.Search<QueryableAsset, GetAssetListRequest>(query).ConfigureAwait(false);
            var assetListResponse = new GetAssetListResponse();

            assetListResponse.Assets.AddRange(searchResponse.Documents.Select(queryableAsset =>
                queryableAsset.Create())
            );

            assetListResponse.SetTotal(searchResponse.Total);

            return assetListResponse;
        }

        public async Task<GetAccountListResponse> GetListOfAccounts(GetAccountListRequest query)
        {
            var searchResponse = await _elasticSearchWrapper.Search<QueryableAccount, GetAccountListRequest>(query).ConfigureAwait(false);
            var accountListResponse = GetAccountListResponse.Create(searchResponse.Documents.Select(queryableAccount =>
                queryableAccount.ToAccount())?.ToList());

            accountListResponse.SetTotal(searchResponse.Total);

            return accountListResponse;
        }

        public async Task<GetTransactionListResponse> GetListOfTransactions(GetTransactionListRequest request)
        {
            var searchRequest = new GetTransactionListRequest
            {
                SearchText = request.SearchText,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var searchResponse = await _elasticSearchWrapper.Search<QueryableTransaction, GetTransactionListRequest>(searchRequest).ConfigureAwait(false);

            if (!searchResponse.IsValid)
            {
                throw new Exception($"Cannot load transactions list. Error: {searchResponse.ServerError}");
            }

            var transactions = searchResponse.Documents.Select(queryableTransaction => queryableTransaction.ToTransaction());

            return GetTransactionListResponse.Create(searchResponse.Total, transactions.ToResponse());
        }
    }
}