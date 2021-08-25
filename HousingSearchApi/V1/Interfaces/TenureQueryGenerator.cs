using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models;
using HousingSearchApi.V1.Infrastructure;
using Nest;

namespace HousingSearchApi.V1.Interfaces
{
    public class TenureQueryGenerator : IQueryGenerator<QueryableTenure>
    {
        private readonly IWildCardAppenderAndPrepender _wildCardAppenderAndPrepender;

        public TenureQueryGenerator(IWildCardAppenderAndPrepender wildCardAppenderAndPrepender)
        {
            _wildCardAppenderAndPrepender = wildCardAppenderAndPrepender;
        }

        public QueryContainer Create(HousingSearchRequest request, QueryContainerDescriptor<QueryableTenure> q)
        {
            if (string.IsNullOrWhiteSpace(request.SearchText)) return null;

            var listOfWildCardedWords = _wildCardAppenderAndPrepender.Process(request.SearchText);

            var searchSurnames = q.QueryString(m =>
                m.Query($"({string.Join(" AND ", listOfWildCardedWords)}) " + string.Join(' ', listOfWildCardedWords))
                    .Fields(f => f.Field(p => p.PaymentReference)
                        .Field(p => p.TenuredAsset.FullAddress)
                        .Field(p => p.HouseholdMembers)
                        .Field("householdMembers.fullName"))
                    .Type(TextQueryType.MostFields));

            return searchSurnames;
        }
    }
}
