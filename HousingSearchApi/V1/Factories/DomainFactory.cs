using SuspenseResolutionInfoResponse = HousingSearchApi.V1.Boundary.Responses.Transactions.SuspenseResolutionInfo;
using SuspenseResolutionInfoDomain = Hackney.Shared.HousingSearch.Domain.Transactions.SuspenseResolutionInfo;

using PersonResponse = HousingSearchApi.V1.Boundary.Responses.Transactions.Person;
using PersonDomain = Hackney.Shared.HousingSearch.Domain.Transactions.Person;

namespace HousingSearchApi.V1.Factories
{
    public static class DomainFactory
    {
        public static SuspenseResolutionInfoResponse ToDomain(this SuspenseResolutionInfoDomain sharedDomain)
        {
            return SuspenseResolutionInfoResponse.Create(sharedDomain.ResolutionDate, sharedDomain.IsConfirmed, sharedDomain.IsApproved, sharedDomain.Note);
        }

        public static PersonResponse ToDomain(this PersonDomain sharedDomain)
        {
            return PersonResponse.Create(sharedDomain.Id, sharedDomain.FullName);
        }
    }
}
