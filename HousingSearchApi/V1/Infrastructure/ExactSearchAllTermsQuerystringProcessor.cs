using Hackney.Core.ElasticSearch.Interfaces;

namespace HousingSearchApi.V1.Infrastructure
{
    public class ExactSearchAllTermsQuerystringProcessor : IExactSearchQuerystringProcessor
    {
        public string Process(string searchText) => searchText.Trim().Replace(" ", " AND ");
    }
}
