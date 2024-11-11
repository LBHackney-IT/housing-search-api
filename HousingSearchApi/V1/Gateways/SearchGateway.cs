using Hackney.Core.Logging;
using Hackney.Shared.HousingSearch.Domain.Asset;
using Hackney.Shared.HousingSearch.Factories;
using Hackney.Shared.HousingSearch.Gateways.Models.Accounts;
using Hackney.Shared.HousingSearch.Gateways.Models.Assets;
using Hackney.Shared.HousingSearch.Gateways.Models.Processes;
using Hackney.Shared.HousingSearch.Gateways.Models.Transactions;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Boundary.Responses.Transactions;
using HousingSearchApi.V1.Factories;
using HousingSearchApi.V1.Gateways.Interfaces;
using HousingSearchApi.V1.Helper;
using HousingSearchApi.V1.Helper.Interfaces;
using HousingSearchApi.V1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QueryablePerson = Hackney.Shared.HousingSearch.Gateways.Models.Persons.QueryablePerson;
using QueryableTenure = Hackney.Shared.HousingSearch.Gateways.Models.Tenures.QueryableTenure;

namespace HousingSearchApi.V1.Gateways
{
    public class SearchGateway : ISearchGateway
    {
        private readonly IElasticSearchWrapper _elasticSearchWrapper;
        private readonly ICustomAddressSorter _customAddressSorter;

        public SearchGateway(IElasticSearchWrapper elasticSearchWrapper, ICustomAddressSorter customAddressSorter)
        {
            _elasticSearchWrapper = elasticSearchWrapper;
            _customAddressSorter = customAddressSorter;
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
            const int CustomSortPageSize = 650;

            if (query.UseCustomSorting)
            {
                // Override pageSize with CustomSortPageSize
                // CustomSorting fetches lots of results in one go
                // and does filtering/sorting on that
                // therefore pageSize (pagination) is redundant for this useCase
                query.PageSize = CustomSortPageSize;

                // if searchText contains a postCode, normalize the formatting
                if (PostCodeHelpers.SearchTextIsValidPostCode(query.SearchText))
                {
                    query.SearchText = PostCodeHelpers.NormalizePostcode(query.SearchText);
                }
            }

            var searchResponse = await _elasticSearchWrapper
                .Search<QueryableAsset, GetAssetListRequest>(query)
                .ConfigureAwait(false);

            var response = searchResponse.ToResponse();

            if (query.UseCustomSorting)
            {
                _customAddressSorter.FilterResponse(query, response);
            }

            return response;
        }

        [LogCall]
        public async Task<GetAllAssetListResponse> GetListOfAssetsSets(GetAllAssetListRequest query)
        {
            if (query.IsFilteredQuery && !string.IsNullOrEmpty(query.SearchText) && PostCodeHelpers.SearchTextIsValidPostCode(query.SearchText))
            {
                query.SearchText = PostCodeHelpers.NormalizePostcode(query.SearchText);
            }

            var searchResponse = await _elasticSearchWrapper.SearchSets<QueryableAsset, GetAllAssetListRequest>(query).ConfigureAwait(false);
            var assetListResponse = new GetAllAssetListResponse();

            if (searchResponse == null) return assetListResponse;
            assetListResponse.Assets.AddRange(searchResponse.Documents.Select(queryableAsset =>
                queryableAsset.CreateAll())
            );

            if (query.IsFilteredQuery && !string.IsNullOrEmpty(query.SearchText) && query.IsTemporaryAccomodation != "true")
            {
                _customAddressSorter.FilterResponse(query, assetListResponse);

                assetListResponse.SetTotal(assetListResponse.Assets.Count);
            }
            else
            {
                assetListResponse.SetTotal(searchResponse.Total);
            }

            if (searchResponse.Documents.Count > 0)
            {
                assetListResponse.SetLastHitId(searchResponse.Hits.Last().Id);
            }



            return assetListResponse;
        }

        [LogCall]
        public async Task<GetProcessListResponse> GetListOfProcesses(GetProcessListRequest query)
        {
            var searchResponse = await _elasticSearchWrapper.Search<QueryableProcess, GetProcessListRequest>(query).ConfigureAwait(false);
            var processListResponse = new GetProcessListResponse();

            processListResponse.Processes.AddRange(searchResponse.Documents.Select(x => x.ToDomain())
            );

            processListResponse.SetTotal(searchResponse.Total);

            return processListResponse;
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
                PageSize = request.PageSize,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };

            var searchResponse = await _elasticSearchWrapper.Search<QueryableTransaction, GetTransactionListRequest>(searchRequest).ConfigureAwait(false);

            if (searchResponse == null) throw new Exception("Cannot get response from ElasticSearch instance");

            if (!searchResponse.IsValid) throw new Exception($"Cannot load transactions list. Error: {searchResponse.ServerError}");

            if (searchResponse.Documents == null) throw new Exception($"ElasticSearch instance returns no documents. Error: {searchResponse.ServerError}");

            var transactions = searchResponse.Documents.Select(queryableTransaction => queryableTransaction.ToTransaction());

            return GetTransactionListResponse.Create(searchResponse.Total, transactions.ToResponse());
        }

        public async Task<List<Asset>> GetChildAssets(GetAssetRelationshipsRequest query)
        {
            var searchResponse = await _elasticSearchWrapper
                .Search<QueryableAsset, GetAssetRelationshipsRequest>(query)
                .ConfigureAwait(false);

            var childAssets = new List<Asset>();

            if (searchResponse == null) return childAssets;
            childAssets.AddRange(searchResponse.Documents.Select(queryableAsset =>
                queryableAsset.CreateAll())
            );

            return childAssets;
        }

        public async Task<GetAllTenureListResponse> GetListOfTenuresSets(GetAllTenureListRequest query)
        {
            var searchResponse = await _elasticSearchWrapper
                .SearchTenuresSets(query)
                .ConfigureAwait(false);

            var tenureListResponse = new GetAllTenureListResponse();

            if (searchResponse == null)
            {
                return tenureListResponse;
            }
            else
            {
                tenureListResponse
                    .Tenures
                    .AddRange(searchResponse.Documents.Select(queryableTenure => queryableTenure.Create()));

                tenureListResponse.SetTotal(searchResponse.Total);

                if (searchResponse.Documents.Any())
                {
                    var lastHit = searchResponse.Hits.Last();
                    var lastHitTenureStartDate = lastHit.Sorts?.FirstOrDefault(defaultValue: null);
                    tenureListResponse.LastHitId = lastHit.Id;
                    tenureListResponse.LastHitTenureStartDate = lastHitTenureStartDate?.ToString(); //[tenure start date, id]
                }

                return tenureListResponse;
            }
        }
    }
}

