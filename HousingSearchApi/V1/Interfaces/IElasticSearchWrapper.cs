using HousingSearchApi.V1.Boundary.Requests;
using Nest;
using System.Threading.Tasks;

namespace HousingSearchApi.V1.Interfaces
{
    public interface IElasticSearchWrapper
    {
        Task<ISearchResponse<T>> Search<T, TRequest>(TRequest request) where T : class where TRequest : class;

        Task<ISearchResponse<T>> SearchSets<T, TRequest>(TRequest request) where T : class where TRequest : class;

        Task<ISearchResponse<T>> SearchTenuresSets<T, TRequest>(TRequest query) where T : class where TRequest : GetAllTenureListRequest;
    }
}
