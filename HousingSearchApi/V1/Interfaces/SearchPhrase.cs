using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models;
using HousingSearchApi.V1.Infrastructure;
using Nest;
using System.Collections.Generic;

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

            if (string.IsNullOrWhiteSpace(request.SearchText))
            {
                result = queryDescriptor.Bool(bq => bq
                    .Must(mq => mq
                        .ConstantScore(cs => cs
                            .Filter(f => f.Term(field => field.PersonTypes, request.PersonType.ToString().ToLower())))));

                return result;
            }

            var listOfWildCardedWords = new List<string>();

            listOfWildCardedWords = _wildCardAppenderAndPrepender.Process(request.SearchText);

            result = queryDescriptor.Bool(bq => bq
                .Filter(f => f.QueryString(m => m.Query(string.Join(' ', listOfWildCardedWords))
                    .Fields(f => f.Field("*"))
                    .Type(TextQueryType.MostFields)))
                .Must(mq => mq
                        .ConstantScore(cs => cs
                            .Filter(f => f.Term(field => field.PersonTypes, request.PersonType.ToString().ToLower())))));

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
