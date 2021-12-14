using Nest;

namespace HousingSearchApi.V1.Interfaces.Filtering
{
    public interface IFilterFactory
    {
        IFilter<T> Create<T, TRequest>(TRequest request) where T : class where TRequest : class;
    }
}
