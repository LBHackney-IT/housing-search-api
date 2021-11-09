using System;
using System.Collections.Generic;
using Nest;

namespace HousingSearchApi.V1.Domain.QueryableModels
{
    public class QueryableAccount
    {
        [Text(Name = "id")]
        public Guid Id { get; set; }

        [Text(Name = "createdAt")]
        public DateTime CreatedAt { get; set; }

        [Text(Name = "lastUpdatedAt")]
        public DateTime LastUpdatedAt { get; set; }

        [Text(Name = "lastUpdatedBy")]
        public string LastUpdatedBy { get; set; }

        [Text(Name = "createdBy")]
        public string CreatedBy { get; set; }

        [Text(Name = "startDate")]
        public DateTime StartDate { get; set; }

        [Text(Name = "endDate")]
        public DateTime? EndDate { get; set; }

        [Text(Name = "accountBalance")]
        public decimal AccountBalance { get; set; } = 0;

        [Text(Name = "consolidatedBalance")]
        public decimal ConsolidatedBalance { get; set; } = 0;

        [Text(Name = "parentAccountId")]
        public Guid ParentAccountId { get; set; }

        [Text(Name = "paymentReference")]
        public string PaymentReference { get; set; }

        [Text(Name = "targetId")]
        public Guid TargetId { get; set; }

        [Text(Name = "targetType")]
        public TargetType TargetType { get; set; }

        [Text(Name = "accountType")]
        public AccountType AccountType { get; set; }

        [Text(Name = "rentGroupType")]
        public RentGroupType RentGroupType { get; set; }

        [Text(Name = "agreementType")]
        public string AgreementType { get; set; }

        [Text(Name = "accountStatus")]
        public AccountStatus AccountStatus { get; set; }

        [Text(Name = "consolidatedCharges")]
        public IEnumerable<QueryableConsolidatedCharge> ConsolidatedCharges { get; set; }

        [Text(Name = "tenure")]
        public QueryableTenure Tenure { get; set; }
    }
}
