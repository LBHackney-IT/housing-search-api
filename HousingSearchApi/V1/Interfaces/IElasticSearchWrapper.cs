using System.Threading.Tasks;
using HousingSearchApi.V1.Boundary.Requests;
using Nest;

namespace HousingSearchApi.V1.Interfaces
{
    public interface IElasticSearchWrapper
    {
        Task<ISearchResponse<T>> Search<T>(HousingSearchRequest request) where T : class;

        Task<ISearchResponse<T>> SearchSets<T>(HousingSearchRequest request) where T : class;
    }
}
