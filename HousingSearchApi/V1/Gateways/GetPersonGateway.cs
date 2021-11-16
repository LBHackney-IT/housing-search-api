using Hackney.Core.Logging;
using Hackney.Shared.HousingSearch.Domain.Person;
using HousingSearchApi.V1.Gateways.Domain;
using Microsoft.Extensions.Logging;
using Nest;
using System.Collections.Generic;
using System.Threading.Tasks;
using HousingSearchApi.V1.Gateways.Interfaces;

namespace HousingSearchApi.V1.Gateways
{
    public class GetPersonGateway : IGetPersonGateway
    {
        private readonly IElasticClient _esClient;
        private readonly ILogger<GetPersonGateway> _logger;

        public GetPersonGateway(IElasticClient esClient, ILogger<GetPersonGateway> logger)
        {
            _esClient = esClient;
            _logger = logger;
        }
        [LogCall]
        public Task<List<Person>> Search(SearchParameters parameters)
        {
            return Task.FromResult(new List<Person>());
        }
    }
}
