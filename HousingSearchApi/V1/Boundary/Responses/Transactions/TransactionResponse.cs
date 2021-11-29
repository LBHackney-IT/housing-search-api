using Hackney.Shared.HousingSearch.Domain.Transactions;
using System;

namespace HousingSearchApi.V1.Boundary.Responses.Transactions
{
    public class TransactionResponse
    {
        /// <summary>
        /// The guid of a record
        /// </summary>
        /// <example>
        /// 2f378d65-38d3-4fb4-877b-afeee666209e
        /// </example>
        public Guid Id { get; }

        /// <summary>
        /// The guid of a tenancy/property
        /// </summary>
        /// <example>
        /// 94b02545-0233-4640-98dd-b2900423c0a5
        /// </example>
        public Guid TargetId { get; }

        /// <summary>
        ///     The target of provided id by target_id
        /// </summary>
        /// <example>
        ///     Asset
        /// </example>
        public TargetType TargetType { get; }
        /// <summary>
        /// Week number for Rent and Period number for LeaseHolders
        /// </summary>
        /// <example>
        /// 2
        /// </example>
        public short PeriodNo { get; }

        /// <summary>
        /// Financial year of transaction
        /// </summary>
        /// <example>
        /// 2022
        /// </example>
        public short FinancialYear { get; }

        /// <summary>
        /// Financial Month of transaction
        /// </summary>
        /// <example>
        /// 1
        /// </example>
        public short FinancialMonth { get; }

        /// <summary>
        /// Transaction Information
        /// </summary>
        /// <example>
        /// DD
        /// </example>
        public string TransactionSource { get; }

        /// <summary>
        /// Type of transaction
        /// </summary>
        /// <example>
        /// Rent
        /// </example>
        public TransactionType TransactionType { get; }

        /// <summary>
        /// Date of transaction
        /// </summary>
        /// <example>
        /// 2021-04-27T23:00:00.000Z
        /// </example>
        public DateTime TransactionDate { get; }

        /// <summary>
        /// Amount of Transaction
        /// </summary>
        /// <example>
        /// 56.78
        /// </example>
        public decimal TransactionAmount { get; }

        /// <summary>
        /// Same as Rent Account Number
        /// </summary>
        /// <example>
        /// 216704
        /// </example>
        public string PaymentReference { get; }

        /// <summary>
        /// Partially filled bank account number
        /// </summary>
        /// <example>
        /// ******78
        /// </example>
        public string BankAccountNumber { get; }

        /// <summary>
        /// Is this account need to be in suspense
        /// </summary>
        /// <example>
        /// true
        /// </example>
        public bool IsSuspense { get; }

        /// <summary>
        /// Information after this recond ceases to be suspense
        /// </summary>
        /// <example>
        /// {
        ///     "ResolutionDate": "2021-04-28T23:00:00.000Z",
        ///     "IsResolve" : true,
        ///     "Note": "Some notes about this record"
        /// }
        /// </example>
        public SuspenseResolutionInfo SuspenseResolutionInfo { get; }

        /// <summary>
        /// Total paid amount
        /// </summary>
        /// <example>
        /// 56.78
        /// </example>
        public decimal PaidAmount { get; }

        /// <summary>
        /// Total charged amount
        /// </summary>
        /// <example>
        /// 87.53
        /// </example>
        public decimal ChargedAmount { get; }

        /// <summary>
        /// Total balance amount
        /// </summary>
        /// <example>
        /// 1025.00
        /// </example>
        public decimal BalanceAmount { get; }

        /// <summary>
        /// Housing Benefit Contribution
        /// </summary>
        /// <example>
        /// 25.56
        /// </example>
        public decimal HousingBenefitAmount { get; }

        /// <summary>
        /// Address of property
        /// </summary>
        /// <example>
        /// Apartment 22, 18 G road, SW11
        /// </example>
        public string Address { get; }

        /// <summary>
        /// Sender, who paid for the transaction
        /// </summary>
        /// <example>
        /// {
        ///     "Id": "6d290de9-75aa-46a9-8bf5-cb8e9bdf4ff0",
        ///     "FullName": "Kian Hayward"
        /// }
        /// </example>
        public Person Sender { get; }

        /// <summary>
        /// ToDO: No information about this field
        /// </summary>
        /// <example>
        /// HSGSUN
        /// </example>
        public string Fund { get; }

        private TransactionResponse(Guid id, Guid targetId, TargetType targetType, short periodNo, short financialYear, short financialMonth, string transactionSource, TransactionType transactionType,
            DateTime transactionDate, decimal transactionAmount, string paymentReference, string bankAccountNumber, bool isSuspense, SuspenseResolutionInfo suspenseResolutionInfo,
            decimal paidAmount, decimal chargedAmount, decimal balanceAmount, decimal housingBenefitAmount, string address, Person sender, string fund)
        {
            Id = id;
            TargetId = targetId;
            TargetType = targetType;
            PeriodNo = periodNo;
            FinancialYear = financialYear;
            FinancialMonth = financialMonth;
            TransactionSource = transactionSource;
            TransactionType = transactionType;
            TransactionDate = transactionDate;
            TransactionAmount = transactionAmount;
            PaymentReference = paymentReference;
            BankAccountNumber = bankAccountNumber;
            IsSuspense = isSuspense;
            SuspenseResolutionInfo = suspenseResolutionInfo;
            PaidAmount = paidAmount;
            ChargedAmount = chargedAmount;
            BalanceAmount = balanceAmount;
            HousingBenefitAmount = housingBenefitAmount;
            Address = address;
            Sender = sender;
            Fund = fund;
        }

        public static TransactionResponse Create(Guid id, Guid targetId, TargetType targetType, short periodNo, short financialYear, short financialMonth, string transactionSource, TransactionType transactionType,
            DateTime transactionDate, decimal transactionAmount, string paymentReference, string bankAccountNumber, bool isSuspense, SuspenseResolutionInfo suspenseResolutionInfo,
            decimal paidAmount, decimal chargedAmount, decimal balanceAmount, decimal housingBenefitAmount, string address, Person person, string fund)
        {
            return new TransactionResponse(id, targetId, targetType, periodNo, financialYear, financialMonth, transactionSource, transactionType, transactionDate, transactionAmount,
                paymentReference, bankAccountNumber, isSuspense, suspenseResolutionInfo, paidAmount, chargedAmount, balanceAmount,
                housingBenefitAmount, address, person, fund);
        }
    }
}
