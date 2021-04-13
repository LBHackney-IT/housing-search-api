namespace HousingSearchApi.V1.Interfaces
{
    public interface IPagingHelper
    {
        int GetPageOffset(int pageSize, int currentPage);
    }
}
