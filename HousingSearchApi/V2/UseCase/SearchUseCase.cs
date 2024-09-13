using System.Collections.Generic;
using Hackney.Core.Logging;
using HousingSearchApi.V2.Gateways.Interfaces;
using HousingSearchApi.V2.UseCase.Interfaces;
using System.Threading.Tasks;

namespace HousingSearchApi.V2.UseCase
{
    public class SearchUseCase : ISearchUseCase
    {
        private readonly ISearchGateway _searchGateway;

        public SearchUseCase(ISearchGateway searchGateway)
        {
            _searchGateway = searchGateway;
        }

        [LogCall]
        public async Task<IReadOnlyCollection<object>> ExecuteAsync(string indexName, string searchText)
        {
            return await _searchGateway.FreeSearch(indexName, searchText).ConfigureAwait(false);
        }
    }
}
