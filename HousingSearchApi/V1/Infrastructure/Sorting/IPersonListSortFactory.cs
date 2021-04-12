using HousingSearchApi.V1.Domain;

namespace HousingSearchApi.V1.Infrastructure.Sorting
{
    public interface IPersonListSortFactory
    {
        IPersonListSort Create(GetPersonListRequest request);
    }
}
