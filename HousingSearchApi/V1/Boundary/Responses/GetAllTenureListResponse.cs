namespace HousingSearchApi.V1.Boundary.Responses
{
    public class GetAllTenureListResponse : GetTenureListResponse
    {
        private string _lastHitId;
        private string _lastHitTenureStartDate;

        public void SetLastHitId(string lastHitId)
        {
            _lastHitId = lastHitId;
        }

        public string LastHitId()
        {
            return _lastHitId;
        }

        public void SetLastHitTenureStartDate(string tenureStartDate)
        {
            _lastHitTenureStartDate = tenureStartDate;
        }

        public string LastHitTenureStartDate()
        {
            return _lastHitTenureStartDate;
        }
    }
}
