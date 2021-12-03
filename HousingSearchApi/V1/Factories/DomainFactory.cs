using SuspenseResolutionInfoResponse = HousingSearchApi.V1.Boundary.Responses.Transactions.SuspenseResolutionInfo;
using SuspenseResolutionInfoDomain = Hackney.Shared.HousingSearch.Domain.Transactions.SuspenseResolutionInfo;

using SenderResponse = HousingSearchApi.V1.Boundary.Responses.Transactions.Sender;
using SenderDomain = Hackney.Shared.HousingSearch.Domain.Transactions.Sender;

namespace HousingSearchApi.V1.Factories
{
    public static class DomainFactory
    {
        public static SuspenseResolutionInfoResponse ToDomain(this SuspenseResolutionInfoDomain sharedDomain)
        {
            return sharedDomain != null ? SuspenseResolutionInfoResponse.Create(sharedDomain.ResolutionDate, sharedDomain.IsConfirmed, sharedDomain.IsApproved, sharedDomain.Note) : null;
        }

        public static SenderResponse ToDomain(this SenderDomain sharedDomain)
        {
            return sharedDomain != null ? SenderResponse.Create(sharedDomain.Id, sharedDomain.FullName) : null;
        }
    }
}
