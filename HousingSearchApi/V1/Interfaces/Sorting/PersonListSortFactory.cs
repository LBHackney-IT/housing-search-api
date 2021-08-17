using HousingSearchApi.V1.Boundary.Requests;

namespace HousingSearchApi.V1.Interfaces.Sorting
{
    public class PersonListSortFactory : IPersonListSortFactory
    {
        public ISort<T> Create<T>(HousingSearchRequest request) where T : class
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
    }
}
