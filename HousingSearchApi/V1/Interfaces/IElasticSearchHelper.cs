using System.Threading.Tasks;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models;
using Nest;

namespace HousingSearchApi.V1.Interfaces
{
    public interface IElasticSearchHelper
    {
        Task<ISearchResponse<T>> Search<T>(HousingSearchRequest request) where T : class;
    }
}
