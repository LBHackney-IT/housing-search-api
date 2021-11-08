using HousingSearchApi.V1.Domain.Enums;
using System;
using System.Collections.Generic;

namespace HousingSearchApi.V1.Domain
{
    public class Account
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ParentAccountId { get; set; }

        public string PaymentReference { get; set; }

        public TargetType TargetType { get; set; }

        public Guid TargetId { get; set; }

        public AccountType AccountType { get; set; }

        public RentGroupType RentGroupType { get; set; }

        public string AgreementType { get; set; }

        public decimal AccountBalance { get; set; } = 0;

        public decimal ConsolidatedBalance { get; set; } = 0;

        public string CreatedBy { get; set; }

        public string LastUpdatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastUpdatedAt { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public AccountStatus AccountStatus { get; set; }

        public IEnumerable<ConsolidatedCharge> ConsolidatedCharges { get; set; }

        public Tenure Tenure { get; set; }
    }
}
