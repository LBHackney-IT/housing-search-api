using Hackney.Shared.HousingSearch.Domain.Transactions;
using System;

namespace HousingSearchApi.V1.Factories
{
    public static class DomainFactory
    {
        public static Boundary.Responses.Transactions.SuspenseResolutionInfo ToDomain(this SuspenseResolutionInfo sharedDomain)
        {
            return Boundary.Responses.Transactions.SuspenseResolutionInfo.Create(sharedDomain.ResolutionDate, sharedDomain.IsConfirmed, sharedDomain.IsApproved, sharedDomain.Note);
        }

        public static Boundary.Responses.Transactions.Person ToDomain(this Person sharedDomain)
        {
            return Boundary.Responses.Transactions.Person.Create(sharedDomain.Id, sharedDomain.FullName);
        }

        public static Boundary.Responses.TargetType ToDomain(this TargetType sharedDomain)
        {
            return sharedDomain switch
            {
                TargetType.Tenure => Boundary.Responses.TargetType.Tenure,
                _ => throw new ArgumentOutOfRangeException($"Target type is out of range. Provided value: {sharedDomain}"),
            };
        }

        public static Boundary.Responses.TransactionType ToDomain(this TransactionType sharedDomain)
        {
            return sharedDomain switch
            {
                TransactionType.Rent => Boundary.Responses.TransactionType.Rent,
                TransactionType.Charge => Boundary.Responses.TransactionType.Charge,
                _ => throw new ArgumentOutOfRangeException($"Transaction type is out of range. Provided value: {sharedDomain}"),
            };
        }
    }
}
