using Nest;

namespace HousingSearchApi.V1.Infrastructure.Interfaces
{
    public interface ISearchQueryContainerOrchestrator<TQueryable, TRequest>
        where TQueryable : class
        where TRequest : class
    {
        QueryContainer Create(TQueryable request, QueryContainerDescriptor<TRequest> q);
    }
}
