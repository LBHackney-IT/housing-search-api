using System.Threading.Tasks;
using Nest;

namespace HousingSearchApi.V1.Interfaces
{
    public interface IElasticSearchWrapper
    {
        Task<ISearchResponse<T>> Search<T, TRequest>(TRequest request) where T : class where TRequest : class;

        Task<ISearchResponse<T>> SearchSets<T, TRequest>(TRequest request) where T : class where TRequest : class;
    }
}
