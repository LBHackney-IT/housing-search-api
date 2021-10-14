using System;
using Hackney.Core.ElasticSearch.Interfaces;

namespace HousingSearchApi.V1.Interfaces
{
    public class ExactSearchQuerystringProcessor : IExactSearchQuerystringProcessor
    {
        public string Process(string searchText)
        {
            searchText = searchText.Trim();

            if (searchText.Split(" ", StringSplitOptions.RemoveEmptyEntries).Length == 2)
            {
                return searchText.Replace(" ", " AND ");
            }

            return searchText;
        }
    }
}
