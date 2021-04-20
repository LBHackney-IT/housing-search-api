using HousingSearchApi.V1.Domain;
using HousingSearchApi.V1.Domain.ES;
using Nest;

namespace HousingSearchApi.V1.Interfaces.Sorting
{
    public class DefaultSort : IPersonListSort
    {
        public SortDescriptor<QueryablePerson> Get(SortDescriptor<QueryablePerson> descriptor)
        {
            return descriptor;
        }
    }
}
