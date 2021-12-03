using Nest;

namespace HousingSearchApi.V1.Interfaces.Filtering
{
    public interface IFilterFactory
    {
        QueryContainer Filter<T, TRequest>(TRequest request, QueryContainerDescriptor<T> q) where T : class where TRequest : class;
    }
}
