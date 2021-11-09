using Hackney.Shared.HousingSearch.Gateways.Models.Transactions;
using HousingSearchApi.V1.Boundary.Requests;
using QueryablePerson = Hackney.Shared.HousingSearch.Gateways.Models.Persons.QueryablePerson;

namespace HousingSearchApi.V1.Interfaces.Sorting
{
    public class SortFactory : ISortFactory
    {
        public ISort<T> Create<T>(HousingSearchRequest request) where T : class
        {
            if (typeof(T) == typeof(QueryablePerson))
            {
                if (string.IsNullOrEmpty(request.SortBy))
                    return new DefaultSort<T>();

                switch (request.IsDesc)
                {
                    case true:
                        return (ISort<T>) new SurnameDesc();
                    case false:
                        return (ISort<T>) new SurnameAsc();
                }
            }

            if (typeof(T) == typeof(QueryableTransaction))
            {
                return (ISort<T>) new TransactionDateDesc();
            }

            return new DefaultSort<T>();
        }
    }
}
