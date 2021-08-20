using System.Linq;
using System.Threading.Tasks;
using Hackney.Core.Logging;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Response;
using HousingSearchApi.V1.Gateways.Models;

namespace HousingSearchApi.V1.Interfaces
{
    public class SearchGateway : ISearchGateway
    {
        private readonly IElasticSearchWrapper _elasticElasticSearchWrapper;

        public SearchGateway(IElasticSearchWrapper elasticElasticSearchWrapper)
        {
            _elasticElasticSearchWrapper = elasticElasticSearchWrapper;
        }

        [LogCall]
        public async Task<GetPersonListResponse> GetListOfPersons(HousingSearchRequest query)
        {
            var searchResponse = await _elasticElasticSearchWrapper.Search<QueryablePerson>(query).ConfigureAwait(false);
            var personListResponse = new GetPersonListResponse();

            personListResponse.Persons.AddRange(searchResponse.Documents.Select(queryablePerson =>
                queryablePerson.Create())
            );

            personListResponse.SetTotal(searchResponse.Total);

            return personListResponse;
        }

        [LogCall]
        public async Task<GetTenureListResponse> GetListOfTenures(HousingSearchRequest query)
        {
            var searchResponse = await _elasticElasticSearchWrapper.Search<QueryableTenure>(query).ConfigureAwait(false);
            var tenureListResponse = new GetTenureListResponse();

            tenureListResponse.Tenures.AddRange(searchResponse.Documents.Select(queryablePerson =>
                queryablePerson.Create())
            );

            tenureListResponse.SetTotal(searchResponse.Total);

            return tenureListResponse;
        }

        [LogCall]
        public async Task<GetAssetListResponse> GetListOfAssets(HousingSearchRequest query)
        {
            var searchResponse = await _elasticElasticSearchWrapper.Search<QueryableAsset>(query).ConfigureAwait(false);
            var assetListResponse = new GetAssetListResponse();

            assetListResponse.Assets.AddRange(searchResponse.Documents.Select(queryablePerson =>
                queryablePerson.Create())
            );

            assetListResponse.SetTotal(searchResponse.Total);

            return assetListResponse;
        }
    }
}
