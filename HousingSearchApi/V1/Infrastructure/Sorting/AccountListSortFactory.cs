using System.Linq;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Domain.QueryableModels;
using HousingSearchApi.V1.Infrastructure.Sorting.Enum;
using HousingSearchApi.V1.Infrastructure.Sorting.Interfaces;
using Nest;

namespace HousingSearchApi.V1.Infrastructure.Sorting
{
    public class AccountListSortFactory : IListSortFactory<QueryableAccount,GetAccountListRequest>
    {
        public SortDescriptor<QueryableAccount> DynamicSort(SortDescriptor<QueryableAccount> sortDescriptor, GetAccountListRequest request)
        {
            if (request.IsDesc)
            {
                switch (request.SortBy)
                {
                    case EAccountSortBy.Address:
                        return sortDescriptor
                            .Descending(f => f.Tenure.FullAddress);
                    case EAccountSortBy.Name:
                        return sortDescriptor
                            .Descending(f => f.Tenure.PrimaryTenants.Select(r => r.FullNameName));
                    case EAccountSortBy.Prn:
                        return sortDescriptor
                            .Descending(f => f.PaymentReference);
                }
            }
            else
            {
                switch (request.SortBy)
                {
                    case EAccountSortBy.Address:
                        return sortDescriptor
                            .Ascending(f => f.Tenure.FullAddress);
                    case EAccountSortBy.Name:
                        return sortDescriptor
                            .Ascending(f => f.Tenure.PrimaryTenants.Select(r => r.FullNameName));
                    case EAccountSortBy.Prn:
                        return sortDescriptor
                            .Ascending(f => f.PaymentReference);
                }
            }

            return null;
        }
    }
}
