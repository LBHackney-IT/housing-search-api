using HousingSearchApi.V1.Interfaces;

namespace HousingSearchApi.V1.Infrastructure
{
    public class PagingHelper : IPagingHelper
    {
        public int GetPageOffset(int pageSize, int currentPage)
        {
            return pageSize * (currentPage == 0 ? 0 : currentPage - 1);
        }
    }
}
