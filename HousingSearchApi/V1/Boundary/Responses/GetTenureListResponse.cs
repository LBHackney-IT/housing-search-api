using System.Collections.Generic;

namespace HousingSearchApi.V1.Boundary.Responses
{
    public class GetTenureListResponse
    {
        private long _total;

        public List<Domain.Tenure.Tenure> Tenures { get; set; }

        public GetTenureListResponse()
        {
            Tenures = new List<Domain.Tenure.Tenure>();
        }

        public void SetTotal(long total)
        {
            _total = total;
        }

        public long Total()
        {
            return _total;
        }
    }
}
