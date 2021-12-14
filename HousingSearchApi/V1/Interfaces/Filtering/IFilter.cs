using Nest;

namespace HousingSearchApi.V1.Interfaces.Filtering
{
    public interface IFilter<T> where T : class
    {
        QueryContainerDescriptor<T> GetDescriptor<TRequest>(QueryContainerDescriptor<T> descriptor, TRequest request);
    }
}
