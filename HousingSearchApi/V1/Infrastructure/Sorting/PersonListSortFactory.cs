using HousingSearchApi.V1.Domain;

namespace HousingSearchApi.V1.Infrastructure.Sorting
{
    public class PersonListSortFactory : IPersonListSortFactory
    {
        public IPersonListSort Create(GetPersonListRequest request)
        {
            if (string.IsNullOrEmpty(request.SortBy))
                return new DefaultSort();

            switch (request.IsDesc)
            {
                case true:
                    return new LastNameDesc();
                case false:
                    return new LastNameAsc();
            }
        }
    }
}
