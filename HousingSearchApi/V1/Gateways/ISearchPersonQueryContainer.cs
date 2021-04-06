using HousingSearchApi.V1.Domain;
using HousingSearchApi.V1.Infrastructure;
using Nest;

namespace HousingSearchApi.V1.Gateways
{
    public interface ISearchPersonQueryContainer
    {
        QueryContainer Create(GetPersonListRequest request,
            QueryContainerDescriptor<QueryablePerson> q);
    }
}
