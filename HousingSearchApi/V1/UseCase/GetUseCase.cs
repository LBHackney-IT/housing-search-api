using System.Collections.Generic;
using Hackney.Core.Logging;
using HousingSearchApi.V1.Gateways.Interfaces;
using HousingSearchApi.V1.UseCase.Interfaces;
using System.Threading.Tasks;

namespace HousingSearchApi.V1.UseCase
{
    public class GetUseCase : IGetUseCase
    {
        private readonly IGetGateway _getGateway;

        public GetUseCase(IGetGateway getGateway)
        {
            _getGateway = getGateway;
        }

        [LogCall]
        public async Task<IReadOnlyCollection<object>> ExecuteAsync(string indexName, string searchText)
        {
            return await _getGateway.FreeSearch(indexName, searchText).ConfigureAwait(false);
        }
    }
}
