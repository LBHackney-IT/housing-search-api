using Hackney.Shared.HousingSearch.Domain.Transactions;
using HousingSearchApi.V1.Boundary.Responses.Transactions;
using System.Collections.Generic;
using System.Linq;

namespace HousingSearchApi.V1.Factories
{
    public static class ResponseFactory
    {
        public static TransactionResponse ToResponse(this Transaction domain)
        {
            return domain == null ? null : TransactionResponse.Create(
                domain.Id,
                domain.TargetId,
                domain.TargetType,
                domain.PeriodNo,
                domain.FinancialYear,
                domain.FinancialMonth,
                domain.TransactionSource,
                domain.TransactionType,
                domain.TransactionDate,
                domain.TransactionAmount,
                domain.PaymentReference,
                domain.BankAccountNumber,
                domain.IsSuspense,
                domain.SuspenseResolutionInfo?.ToDomain(),
                domain.PaidAmount,
                domain.ChargedAmount,
                domain.BalanceAmount,
                domain.HousingBenefitAmount,
                domain.Address,
                domain.Sender?.ToDomain(),
                domain.Fund);
        }

        public static List<TransactionResponse> ToResponse(this IEnumerable<Transaction> domainList)
        {
            return domainList == null ?
                new List<TransactionResponse>() :
                domainList.Select(domain => domain.ToResponse()).ToList();
        }
    }
}
