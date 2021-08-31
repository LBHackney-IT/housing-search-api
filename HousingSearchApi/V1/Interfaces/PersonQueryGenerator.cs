using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models.Persons;
using HousingSearchApi.V1.Infrastructure;
using Nest;
using System;
using System.Collections.Generic;

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

            Func<QueryContainerDescriptor<QueryablePerson>, QueryContainer> filterBySearchTextContainer =
                (containerDescriptor) => containerDescriptor.QueryString(q => q.Query(string.Join(' ', listOfWildCardedWords))
                    .Fields(f => f.Field("*"))
                    .Type(TextQueryType.MostFields));

            filters.Add(filterBySearchTextContainer);

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
