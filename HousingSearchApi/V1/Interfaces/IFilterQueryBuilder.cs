using Hackney.Core.ElasticSearch.Interfaces;
using Nest;
using System.Collections.Generic;

namespace HousingSearchApi.V1.Interfaces
{
    public interface IFilterQueryBuilder<T> : IQueryBuilder<T> where T : class
    {
        public IFilterQueryBuilder<T> WithMultipleFilterQuery(string commaSeparatedFilters, List<string> fields);
        public new QueryContainer Build(QueryContainerDescriptor<T> containerDescriptor);


    }
}
