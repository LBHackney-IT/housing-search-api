using HousingSearchApi.V1.Boundary.Requests;

namespace HousingSearchApi.V1.Interfaces.Sorting
{
    public class PersonListSortFactory : IPersonListSortFactory
    {
        public IPersonListSort Create(HousingSearchRequest request)
        {
            if (string.IsNullOrEmpty(request.SortBy))
                return new DefaultSort();

            switch (request.IsDesc)
            {
                case true:
                    return new SurnameDesc();
                case false:
                    return new SurnameAsc();
            }
        }
    }
}
