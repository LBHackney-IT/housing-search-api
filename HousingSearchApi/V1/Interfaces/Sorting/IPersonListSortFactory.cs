using HousingSearchApi.V1.Boundary.Requests;

namespace HousingSearchApi.V1.Interfaces.Sorting
{
    public interface IPersonListSortFactory
    {
        ISort<T> Create<T>(HousingSearchRequest request) where T : class;
    }
}
