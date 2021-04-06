using HousingSearchApi.V1.Domain;
using HousingSearchApi.V1.Infrastructure;
using Nest;

namespace HousingSearchApi.V1.Gateways
{
    public class SearchLastNames : ISearchPersonQueryContainer
    {
        public QueryContainer Create(GetPersonListRequest request, QueryContainerDescriptor<QueryablePerson> q)
        {
            if (string.IsNullOrWhiteSpace(request.SearchText)) return null;
            var searchText = request.SearchText?.Replace(" ", "").ToLower();

            var searchLastNames = q.Wildcard(m =>
                m.Field(f => f.Surname).Value($"*{searchText}*"));

            return searchLastNames;
        }
    }
}
