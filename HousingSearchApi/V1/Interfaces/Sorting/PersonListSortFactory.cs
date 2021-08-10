using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models;
using HousingSearchApi.V1.Infrastructure;
using Nest;

namespace HousingSearchApi.V1.Interfaces.Sorting
{
    public class PersonListSortFactory : IPersonListSortFactory, IListSortFactory<GetPersonListRequest, QueryablePerson>
    {
        public IPersonListSort Create(GetPersonListRequest request)
        {
            if (string.IsNullOrEmpty(request.SortBy))
                return new DefaultSort();

            switch (request.IsDesc)
            {
                case true:
                    return new SurnameDesc();
                case false:
                    return new SurnameAsc();
                // TODO: Add more classes for all sorting fields
            }
        }

        // Hanna Holasava
        // I can suggest to use the next method instead of creating new class for each field
        public SortDescriptor<QueryablePerson> DynamicSort(SortDescriptor<QueryablePerson> sortDescriptor, GetPersonListRequest request)
        {
            var sortBy = request.SortBy?.ToLower();

            switch (sortBy)
            {
                case "title":
                    sortDescriptor.SetSortOrder(request.IsDesc, x => x.Title);
                    break;

                case "firstname":
                    sortDescriptor.SetSortOrder(request.IsDesc, x => x.Firstname);
                    break;

                case "surname":
                    sortDescriptor.SetSortOrder(request.IsDesc, x => x.Surname);
                    break;

                case "middlename":
                    sortDescriptor.SetSortOrder(request.IsDesc, x => x.MiddleName);
                    break;

                case "dateOfBirth":
                    sortDescriptor.SetSortOrder(request.IsDesc, x => x.DateOfBirth);
                    break;

                case "ethinicity":
                    sortDescriptor.SetSortOrder(request.IsDesc, x => x.Ethinicity);
                    break;

                case "gender":
                    sortDescriptor.SetSortOrder(request.IsDesc, x => x.Gender);
                    break;

                case "placeofbirth":
                    sortDescriptor.SetSortOrder(request.IsDesc, x => x.PlaceOfBirth);
                    break;

                default:
                    sortDescriptor.SetSortOrder(request.IsDesc, x => x.Surname);
                    break;
            }

            return sortDescriptor;
        }
    }
}
