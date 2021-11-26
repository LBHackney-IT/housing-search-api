using Hackney.Shared.HousingSearch.Gateways.Models.Assets;
using Hackney.Shared.HousingSearch.Gateways.Models.Persons;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Interfaces.Sorting;

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
                if (string.IsNullOrEmpty(((HousingSearchRequest) (object) request).SortBy))
                    return new DefaultSort<T>();

                switch (((HousingSearchRequest) (object) request).IsDesc)
                {
                    case true:
                        return (ISort<T>) new AssetIdDesc();
                    case false:
                        return (ISort<T>) new AssetIdAsc();
                }
            }

            return new DefaultSort<T>();
        }
    }
}
