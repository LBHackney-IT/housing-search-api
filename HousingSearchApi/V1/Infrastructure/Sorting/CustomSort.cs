using HousingSearchApi.V1.Interfaces.Sorting;
using Nest;
using System.Linq;

namespace HousingSearchApi.V1.Infrastructure.Sorting
{
    public class CustomSort<T> : ISort<T> where T : class
    {
        private readonly string _field;
        private readonly string[] _values;

        public CustomSort(string field, params string[] values)
        {
            _field = field;
            _values = values;
        }

        public SortDescriptor<T> GetSortDescriptor(SortDescriptor<T> descriptor)
        {
            var sortScriptParams = _values
                .Select((value, index) => new { value, index })
                .ToDictionary(pair => pair.value, pair => (object) pair.index);

            return descriptor
                .Script(script => script
                    .Type("number")
                    .Ascending()
                    .Script(sc => sc
                      .Source($"if(params.order.containsKey(doc['{_field}.keyword'].value)) {{ return params.order[doc['{_field}.keyword'].value];}} return 100000;")
                      .Params(p => p.Add("order", sortScriptParams))));
        }
    }
}
