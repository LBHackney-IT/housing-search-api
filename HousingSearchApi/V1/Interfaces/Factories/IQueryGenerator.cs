using HousingSearchApi.V1.Boundary.Requests;
using Nest;

namespace HousingSearchApi.V1.Interfaces.factories
{
    public interface IQueryGenerator<T> where T : class

    {
        QueryContainer Create(HousingSearchRequest request,
            QueryContainerDescriptor<T> q);
    }
}
