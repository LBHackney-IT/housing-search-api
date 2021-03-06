using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models.Persons;

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

            return new DefaultSort<T>();
        }
    }
}
