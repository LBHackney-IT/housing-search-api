using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hackney.Core.Logging;
using Hackney.Shared.HousingSearch.Domain.Accounts;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Gateways.Domain;
using HousingSearchApi.V1.Interfaces;
using Microsoft.Extensions.Logging;

namespace HousingSearchApi.V1.Gateways
{
    public class GetAccountGateway: IGetAccountGateway
    {
        private readonly ISearchGateway _searchGateway;
        private readonly ILogger<GetAccountGateway> _logger;

        public GetAccountGateway(ISearchGateway searchGateway,ILogger<GetAccountGateway> logger)
        {
            _searchGateway = searchGateway;
            _logger = logger;
        }

        [LogCall]
        public async Task<GetAccountListResponse> Search(HousingSearchRequest parameters)
        {
            _logger.LogInformation("Housing search api for getting account list called.");
            return  await _searchGateway.GetListOfAccounts(parameters).ConfigureAwait(false);
        }
    }
}
