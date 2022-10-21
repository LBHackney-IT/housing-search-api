using Hackney.Core.ElasticSearch.Interfaces;
using Nest;
using System.Collections.Generic;

namespace HousingSearchApi.V1.Interfaces
{
    public interface IFilterQueryBuilder<T> : IQueryBuilder<T> where T : class
    {
        public IFilterQueryBuilder<T> WithMultipleFilterQuery(string commaSeparatedFilters, List<string> fields);
        public new QueryContainer Build(QueryContainerDescriptor<T> containerDescriptor);

        public new IQueryBuilder<T> WithWildstarQuery(string searchText, List<string> fields, TextQueryType textQueryType = TextQueryType.MostFields);

        public new IQueryBuilder<T> WithFilterQuery(string commaSeparatedFilters, List<string> fields, TextQueryType textQueryType = TextQueryType.MostFields);

        public new IQueryBuilder<T> WithExactQuery(string searchText, List<string> fields, IExactSearchQuerystringProcessor processor = null, TextQueryType textQueryType = TextQueryType.MostFields);
    }
}
