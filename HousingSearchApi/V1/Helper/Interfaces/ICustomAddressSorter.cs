using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;

namespace HousingSearchApi.V1.Helper.Interfaces
{
    public interface ICustomAddressSorter
    {
        void FilterResponse(HousingSearchRequest searchModel, GetAllAssetListResponse content);
    }
}
