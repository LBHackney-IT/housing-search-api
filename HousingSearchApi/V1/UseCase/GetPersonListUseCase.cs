using System.Linq;
using Hackney.Core.Logging;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.UseCase.Interfaces;
using System.Threading.Tasks;
using HousingSearchApi.V1.Gateways.Interfaces;
using System;

namespace HousingSearchApi.V1.UseCase
{
    public class GetPersonListUseCase : IGetPersonListUseCase
    {
        private readonly ISearchGateway _searchGateway;

        public GetPersonListUseCase(ISearchGateway searchGateway)
        {
            _searchGateway = searchGateway;
        }

        [LogCall]
        public async Task<GetPersonListResponse> ExecuteAsync(GetPersonListRequest housingSearchRequest)
        {
            if (housingSearchRequest == null)
            {
                throw new ArgumentNullException(nameof(housingSearchRequest));
            }
            var personListResponse = await _searchGateway.GetListOfPersons(housingSearchRequest).ConfigureAwait(false);

            if (personListResponse.Persons.Count == 0)
            {
                return personListResponse;
            }

            var persons = personListResponse.Persons;
            var accounts = await
                _searchGateway.GetAccountListByTenureIdsAsync(persons.SelectMany(p => p.Tenures.Select(t => t.Id))).ConfigureAwait(false);

            var populatedPersons = persons
                .SelectMany(p => p.Tenures)
                .Join(accounts, t => t.Id, a => a.TargetId.ToString(), (tenure, account) => new { Tenure = tenure, Account = account })
                .ToList();

            populatedPersons.ForEach(keyValue =>
            {
                keyValue.Tenure.TotalBalance = keyValue.Account.ConsolidatedBalance;
            });

            return personListResponse;
        }
    }
}
