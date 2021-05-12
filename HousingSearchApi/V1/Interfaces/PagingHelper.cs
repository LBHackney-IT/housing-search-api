namespace HousingSearchApi.V1.Interfaces
{
    public class PagingHelper : IPagingHelper
    {
        public int GetPageOffset(int pageSize, int currentPage)
        {
            return pageSize * (currentPage == 0 ? 0 : currentPage - 1);
        }
    }
}
