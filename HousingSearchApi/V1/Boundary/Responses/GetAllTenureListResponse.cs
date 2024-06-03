namespace HousingSearchApi.V1.Boundary.Responses
{
    public class GetAllTenureListResponse : GetTenureListResponse
    {
        private string _lastHitTenureStartDate;
        private string _lastHitId;

        internal string LastHitTenureStartDate
        {
            get => _lastHitTenureStartDate;
            set => _lastHitTenureStartDate = value;
        }

        internal string LastHitId
        {
            get => _lastHitId;
            set => _lastHitId = value;
        }
    }
}
