using System.Threading.Tasks;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models;
using Nest;

namespace HousingSearchApi.V1.Interfaces
{
    public interface ISearchPersonElasticSearchHelper
    {
        Task<ISearchResponse<QueryablePerson>> SearchPersons(GetPersonListRequest request);
        Task<ISearchResponse<QueryableTenure>> SearchTenures(GetTenureListRequest query);
    }
}
