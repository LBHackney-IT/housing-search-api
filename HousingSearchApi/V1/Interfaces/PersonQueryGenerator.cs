using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models.Persons;
using HousingSearchApi.V1.Infrastructure;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HousingSearchApi.V1.Interfaces
{
    public class PersonQueryGenerator : IQueryGenerator<QueryablePerson>
    {
        private readonly IWildCardAppenderAndPrepender _wildCardAppenderAndPrepender;

        public PersonQueryGenerator(IWildCardAppenderAndPrepender wildCardAppenderAndPrepender)
        {
            _wildCardAppenderAndPrepender = wildCardAppenderAndPrepender;
        }

        public QueryContainer Create(HousingSearchRequest request, QueryContainerDescriptor<QueryablePerson> q)
        {
            if (!(request is GetPersonListRequest personListRequest))
            {
                return null;
            }

            var queryContainer = new QueryContainer();
            var filters = new List<Func<QueryContainerDescriptor<QueryablePerson>, QueryContainer>>();

            var listOfWildCardedWords = _wildCardAppenderAndPrepender.Process(personListRequest.SearchText);
            var nonWildCardWords = personListRequest.SearchText.Split(" ").ToList();
            nonWildCardWords = nonWildCardWords.Select(x => "\"" + x + "\"").ToList();

            Func<QueryContainerDescriptor<QueryablePerson>, QueryContainer> filterBySearchTextContainer =
              (containerDescriptor) => containerDescriptor.QueryString(q => q.Query($"({string.Join(" AND ", listOfWildCardedWords)}) " + string.Join(' ', listOfWildCardedWords))
                  .Fields(f => f.Field(p => p.Firstname)
                      .Field(p => p.Surname))
                  .Type(TextQueryType.MostFields));

            filters.Add(filterBySearchTextContainer);

            Func<QueryContainerDescriptor<QueryablePerson>, QueryContainer> filterBySearchTextContainerKeywords =
                (containerDescriptor) => containerDescriptor.QueryString(q => q.Query(string.Join(' ', nonWildCardWords))
                    .Fields(f => f.Field("firstname^2")
                        .Field(p => "surname^2"))
                    .Type(TextQueryType.MostFields));

            filters.Add(filterBySearchTextContainerKeywords);

            if (personListRequest.PersonType.HasValue)
            {
                var types = personListRequest.PersonType.Value.GetPersonTypes();

                Func<QueryContainerDescriptor<QueryablePerson>, QueryContainer> filterByTypeContainer =
                    (containerDescriptor) => containerDescriptor.QueryString(q => q.Query(string.Join(' ', types))
                        .Fields(f => f.Field("tenures.type"))
                        .Type(TextQueryType.MostFields));

                filters.Add(filterByTypeContainer);
            }

            queryContainer = q.Bool(bq => bq.Filter(filters.ToArray()));

            return queryContainer;
        }
    }
}
