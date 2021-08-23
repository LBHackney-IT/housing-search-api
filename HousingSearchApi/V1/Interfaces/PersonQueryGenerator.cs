using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models;
using HousingSearchApi.V1.Infrastructure;
using Nest;

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
            if (request is GetPersonListRequest personListRequest)
            {
                QueryContainer queryContainer = new QueryContainer();

                var types = personListRequest.PersonType.GetPersonTypes();

                if (string.IsNullOrWhiteSpace(request.SearchText))
                {
                    queryContainer = q.Bool(bq => bq
                       .Filter(f => f.QueryString(q => q.Query(string.Join(' ', types))
                       .Fields(f => f.Field("tenures.type")).Type(TextQueryType.MostFields))));

                    return queryContainer;
                }

                var listOfWildCardedWords = _wildCardAppenderAndPrepender.Process(personListRequest.SearchText);


                queryContainer = q.Bool(bq => bq
                .Filter(filter => filter.QueryString(q => q.Query(string.Join(' ', listOfWildCardedWords))
                        .Fields(f => f.Field("*"))
                        .Type(TextQueryType.MostFields))
                        &&
                        filter.QueryString(q => q.Query(string.Join(' ', types))
                        .Fields(f => f.Field("tenures.type"))
                        .Type(TextQueryType.MostFields))));

                return queryContainer;
            }
            else
            {
                return null;
            }
        }
    }
}
