using System;

namespace HousingSearchApi.Tests.V1.E2ETests.Steps.ResponseModels
{
    public class TransactionDTO
    {
        public Guid Id { get; set; }
        
        public Guid TargetId { get; set; }
        
        public Hackney.Shared.HousingSearch.Domain.Transactions.TargetType TargetType { get; set; }

        public short PeriodNo { get; set; }

        public short FinancialYear { get; set; }
        
        public short FinancialMonth { get; set; }
        
        public string TransactionSource { get; set; }
        
        public Hackney.Shared.HousingSearch.Domain.Transactions.TransactionType TransactionType { get; set; }
        
        public DateTime TransactionDate { get; set; }

        public PersonDTO Person { get; set; }

        public decimal TransactionAmount { get; set; }
        
        public string PaymentReference { get; set; }
        
        public string BankAccountNumber { get; set; }
        
        public bool IsSuspense { get; set; }
        
        public decimal PaidAmount { get; set; }
        
        public decimal ChargedAmount { get; set; }
        
        public decimal BalanceAmount { get; set; }

        public decimal HousingBenefitAmount { get; set; }
        
        public string Address { get; set; }
        
        public string Fund { get; set; }
    }
}
