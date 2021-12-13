using HousingSearchApi.V1.Interfaces.Filtering;
using Nest;

namespace HousingSearchApi.V1.Infrastructure.Filtering
{
    public class DefaultFilter<T> : IFilter<T> where T : class
    {
        public QueryContainerDescriptor<T> GetDescriptor<TRequest>(QueryContainerDescriptor<T> descriptor, TRequest request)
        {
            return descriptor;
        }
    }
}
