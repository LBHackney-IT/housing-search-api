using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Domain;
using HousingSearchApi.V1.Domain.ES;
using Nest;

namespace HousingSearchApi.V1.Interfaces
{
    public class SearchSurnames : ISearchPersonQueryContainer
    {
        public QueryContainer Create(GetPersonListRequest request, QueryContainerDescriptor<QueryablePerson> q)
        {
            if (string.IsNullOrWhiteSpace(request.SearchText)) return null;
            var searchText = request.SearchText?.Replace(" ", "").ToLower();

            var searchSurnames = q.Wildcard(m =>
                m.Field(f => f.Surname).Value($"*{searchText}*"));

            return searchSurnames;
        }
    }
}
