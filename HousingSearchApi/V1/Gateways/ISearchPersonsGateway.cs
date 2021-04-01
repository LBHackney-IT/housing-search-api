using System.Linq;
using System.Threading.Tasks;
using HousingSearchApi.V1.Domain;

namespace HousingSearchApi.V1.Gateways
{
    public interface ISearchPersonsGateway
    {
        Task<GetPersonListResponse> GetListOfPersons(GetPersonListRequest query);
    }

    public class SearchPersonsGateway : ISearchPersonsGateway
    {
        private readonly ISearchPersonESHelper _esHelper;

        public SearchPersonsGateway(ISearchPersonESHelper esHelper)
        {
            _esHelper = esHelper;
        }

        public async Task<GetPersonListResponse> GetListOfPersons(GetPersonListRequest request)
        {
            var searchResponse = await _esHelper.Search(request);
            var personListResponse = new GetPersonListResponse();

            personListResponse.Persons.AddRange(searchResponse.Documents.Select(queryablePerson =>
                Person.Create(queryablePerson)
            ));

            return personListResponse;
        }
    }
}
