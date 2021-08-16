using HousingSearchApi.V1.Boundary.Requests;

namespace HousingSearchApi.V1.Interfaces.Sorting
{
    public interface IPersonListSortFactory
    {
        IPersonListSort Create(HousingSearchRequest request);
    }
}
