using Nest;
using System.Threading.Tasks;

namespace HousingSearchApi.V1.Infrastructure.Helpers.Interfaces
{
    public interface ISearchElasticSearchHelper<TResponse, TRequest>
        where TResponse : class
        where TRequest : class
    {
        Task<ISearchResponse<TResponse>> Search(TRequest request);
    }
}
