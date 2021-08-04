using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models;
using Nest;

namespace HousingSearchApi.V1.Interfaces
{
    public interface ISearchPersonQueryContainer
    {
        QueryContainer CreatePersonQuery(GetPersonListRequest request,
            QueryContainerDescriptor<QueryablePerson> q);

        QueryContainer CreateTenureQuery(GetTenureListRequest request,
            QueryContainerDescriptor<QueryableTenure> q);
    }
}
