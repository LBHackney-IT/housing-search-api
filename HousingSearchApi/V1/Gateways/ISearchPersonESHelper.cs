using System.Threading.Tasks;
using HousingSearchApi.V1.Domain;
using HousingSearchApi.V1.Infrastructure;
using Nest;

namespace HousingSearchApi.V1.Gateways
{
    public interface ISearchPersonESHelper
    {
        Task<ISearchResponse<QueryablePerson>> Search(GetPersonListRequest request);
    }
}
