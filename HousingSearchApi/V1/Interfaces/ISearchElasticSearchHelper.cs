using Nest;
using System.Threading.Tasks;

namespace HousingSearchApi.V1.Interfaces
{
    public interface ISearchElasticSearchHelper<TRequest, TQueryable>
        where TRequest : class
        where TQueryable : class
    {
        Task<ISearchResponse<TQueryable>> Search(TRequest request);
    }
}
