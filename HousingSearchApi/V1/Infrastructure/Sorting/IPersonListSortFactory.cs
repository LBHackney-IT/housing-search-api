using HousingSearchApi.V1.Boundary.Requests;

namespace HousingSearchApi.V1.Infrastructure.Sorting
{
    public interface IPersonListSortFactory
    {
        IPersonListSort Create(GetPersonListRequest request);
    }
}
