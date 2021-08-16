using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models;
using Nest;

namespace HousingSearchApi.V1.Interfaces
{
    public interface ISearchPersonsQueryContainerOrchestrator
    {
        QueryContainer CreatePerson(HousingSearchRequest request,
            QueryContainerDescriptor<QueryablePerson> q);

        QueryContainer CreateTenure(HousingSearchRequest request, QueryContainerDescriptor<QueryableTenure> queryContainerDescriptor);
    }
}
