using Hackney.Shared.HousingSearch.Gateways.Models.Assets;
using Hackney.Shared.HousingSearch.Gateways.Models.Processes;
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

            if (typeof(T) == typeof(QueryableTransaction))
            {
                return (ISort<T>) new TransactionDateDesc();
            }

            if (typeof(T) == typeof(QueryableProcess))
            {
                var sortBy = ((HousingSearchRequest) (object) request).SortBy;
                if (string.IsNullOrEmpty(sortBy))
                    return new DefaultSort<T>();

                if (((HousingSearchRequest) (object) request).IsDesc)
                {
                    switch (sortBy)
                    {
                        case "name":
                            return (ISort<T>) new PersonNameDesc();
                        case "process":
                            return (ISort<T>) new ProcessNameDesc();
                        case "patch":
                            return (ISort<T>) new PatchDesc();
                        case "state":
                            return (ISort<T>) new StateDesc();
                    }
                }
                else
                {
                    switch (sortBy)
                    {
                        case "name":
                            return (ISort<T>) new PersonNameAsc();
                        case "process":
                            return (ISort<T>) new ProcessNameAsc();
                        case "patch":
                            return (ISort<T>) new PatchAsc();
                        case "state":
                            return (ISort<T>) new StateAsc();
                    }
                }
            }

            return new DefaultSort<T>();
        }
    }
}
