using Nest;

namespace HousingSearchApi.V1.Interfaces
{
    public interface ISearchQueryContainerOrchestrator<TRequest, TQueryable>
        where TRequest : class
        where TQueryable : class
    {
        QueryContainer Create(TRequest request, QueryContainerDescriptor<TQueryable> q);
    }
}
