using Hackney.Shared.HousingSearch.Gateways.Models.Assets;
using Hackney.Shared.HousingSearch.Gateways.Models.Persons;
using Hackney.Shared.HousingSearch.Gateways.Models.Transactions;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Interfaces.Sorting;
using QueryablePerson = Hackney.Shared.HousingSearch.Gateways.Models.Persons.QueryablePerson;

namespace HousingSearchApi.V1.Infrastructure.Sorting
{
    public class SortFactory : ISortFactory
    {
        public ISort<T> Create<T, TRequest>(TRequest request) where T : class where TRequest : class
        {
            if (typeof(T) == typeof(QueryablePerson))
            {
                if (string.IsNullOrEmpty(((HousingSearchRequest) (object) request).SortBy))
                    return new DefaultSort<T>();

                switch (((HousingSearchRequest) (object) request).IsDesc)
                {
                    case true:
                        return (ISort<T>) new SurnameDesc();
                    case false:
                        return (ISort<T>) new SurnameAsc();
                }
            }
            if (typeof(T) == typeof(QueryableAsset))
            {
                var customSort = new CustomSort<QueryableAsset>("assetType", "Dwelling");
                var housingSearchRequest = (HousingSearchRequest) (object) request;

                if (!string.IsNullOrEmpty(housingSearchRequest.SortBy))
                {
                    return housingSearchRequest.IsDesc
                      ? (ISort<T>) new AssetIdDesc(customSort)
                      : (ISort<T>) new AssetIdAsc();
                }

                return (ISort<T>) customSort;
            }


            if (typeof(T) == typeof(QueryableTransaction))
            {
                return (ISort<T>) new TransactionDateDesc();
            }

            return new DefaultSort<T>();
        }
    }
}
