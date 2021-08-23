using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models;
using HousingSearchApi.V1.Infrastructure;
using Nest;
using System.Collections.Generic;
using System.Linq;

namespace HousingSearchApi.V1.Interfaces
{
    public class SearchPhrase : ISearchPersonQueryContainer
    {
        private readonly IWildCardAppenderAndPrepender _wildCardAppenderAndPrepender;

        public SearchPhrase(IWildCardAppenderAndPrepender wildCardAppenderAndPrepender)
        {
            _wildCardAppenderAndPrepender = wildCardAppenderAndPrepender;
        }

        public QueryContainer CreatePersonQuery(GetPersonListRequest request, QueryContainerDescriptor<QueryablePerson> queryDescriptor)
        {
            QueryContainer result = new QueryContainer();

            var types = request.GetPersonTypes();

            var listOfWildCardedTypes = _wildCardAppenderAndPrepender.Process(types);

            // Hanna Holosova
            // Because of GetPersonListRequestValidator we cannot be there. So do we need this one?
            if (string.IsNullOrWhiteSpace(request.SearchText))
            {
                result = queryDescriptor.Bool(bq => bq
                    .Filter(f => f.QueryString(q => q.Query(string.Join(' ', listOfWildCardedTypes)).Fields(f => f.Field(ff => ff.PersonTypes)).Type(TextQueryType.MostFields))));

                return result;
            }

            var listOfWildCardedWords = _wildCardAppenderAndPrepender.Process(request.SearchText);

            result = queryDescriptor.Bool(bq => bq
                .Filter(filter => filter.QueryString(q => q.Query(string.Join(' ', listOfWildCardedWords))
                    .Fields(f => f.Field("*"))
                    .Type(TextQueryType.MostFields))
                    &&
                        filter.QueryString(q => q.Query(string.Join(' ', listOfWildCardedTypes))
                    .Fields(f => f.Field(ff => ff.PersonTypes))
                    .Type(TextQueryType.BestFields))));

            return result;
        }

        public QueryContainer CreateTenureQuery(GetTenureListRequest request, QueryContainerDescriptor<QueryableTenure> queryDescriptor)
        {
            if (string.IsNullOrWhiteSpace(request.SearchText)) return null;

            var listOfWildCardedWords = _wildCardAppenderAndPrepender.Process(request.SearchText);

            var searchSurnames = queryDescriptor.QueryString(m =>
                m.Query(string.Join(' ', listOfWildCardedWords))
                    .Fields(f => f.Field(p => p.PaymentReference)
                        .Field(p => p.TenuredAsset.FullAddress)
                        .Field(p => p.HouseholdMembers)
                        .Field("householdMembers.fullName"))
                    .Type(TextQueryType.MostFields));

            return searchSurnames;
        }
    }
}
