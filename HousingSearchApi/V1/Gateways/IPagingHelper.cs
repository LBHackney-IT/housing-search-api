namespace HousingSearchApi.V1.Gateways
{
    public interface IPagingHelper
    {
        int GetPageOffset(int pageSize, int currentPage);
    }
}
