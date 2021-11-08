using Hackney.Core.Logging;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Response;
using HousingSearchApi.V1.Domain;
using HousingSearchApi.V1.Domain.QueryableModels;
using HousingSearchApi.V1.Infrastructure.Helpers.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HousingSearchApi.V1.Gateways
{
    public class GetAccountGateway : IGetAccountGateway
    {
        private readonly ISearchElasticSearchHelper<QueryableAccount, GetAccountListRequest> _elasticClient;
        private readonly ILogger<GetAccountGateway> _logger;

        public GetAccountGateway(ISearchElasticSearchHelper<QueryableAccount,GetAccountListRequest> elasticClient,ILogger<GetAccountGateway> logger)
        {
            _elasticClient = elasticClient;
            _logger = logger;
        }

        [LogCall]
        public async Task<APIResponse<Account>> SearchAsync(GetAccountListRequest getAccountListRequest)
        {
            var searchResponse = await _elasticClient.Search(getAccountListRequest).ConfigureAwait(false);
            List<Account> responses = searchResponse.Documents.Select(p =>
                new Account
                {
                    Id = p.Id,
                    Tenure = p.Tenure == null ? null : new Tenure
                    {
                        FullAddress = p.Tenure.FullAddress,
                        PrimaryTenants = p.Tenure.PrimaryTenants?.Select(t => new PrimaryTenants
                        {
                            Id = t.Id,
                            FullName = t.FullNameName
                        }).ToList()
                    },
                    ConsolidatedCharges = p.ConsolidatedCharges?.Select(c => new ConsolidatedCharge
                    {
                        Amount = c.Amount,
                        Frequency = c.Frequency,
                        Type = c.Type
                    }).ToList(),
                    CreatedBy = p.CreatedBy,
                    LastUpdatedBy = p.LastUpdatedBy,
                    AccountBalance = p.AccountBalance,
                    StartDate = p.StartDate,
                    AccountType = p.AccountType,
                    LastUpdatedAt = p.LastUpdatedAt,
                    EndDate = p.EndDate,
                    TargetId = p.TargetId,
                    AccountStatus = p.AccountStatus,
                    RentGroupType = p.RentGroupType,
                    AgreementType = p.AgreementType,
                    ConsolidatedBalance = p.ConsolidatedBalance,
                    CreatedAt = p.CreatedAt,
                    ParentAccountId = p.ParentAccountId,
                    PaymentReference = p.PaymentReference,
                    TargetType = p.TargetType
                }).ToList();

            return new APIResponse<Account>(responses) { Total = searchResponse.Total };
        }
    }
}
