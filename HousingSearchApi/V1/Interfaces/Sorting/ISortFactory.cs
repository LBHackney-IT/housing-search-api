using HousingSearchApi.V1.Boundary.Requests;

namespace HousingSearchApi.V1.Interfaces.Sorting
{
    public interface ISortFactory
    {
        ISort<T> Create<T, TRequest>(TRequest request) where T : class where TRequest : class;
    }
}
