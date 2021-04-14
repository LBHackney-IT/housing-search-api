using System.Threading.Tasks;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Infrastructure;
using Nest;

namespace HousingSearchApi.V1.Interfaces
{
    public interface ISearchPersonESHelper
    {
        Task<ISearchResponse<QueryablePerson>> Search(GetPersonListRequest request);
    }
}
