using Hackney.Shared.HousingSearch.Domain.Tenure;
using System.Collections.Generic;

namespace HousingSearchApi.V1.Boundary.Responses
{
    public class GetTenureListResponse
    {
        private long _total;

        public List<Tenure> Tenures { get; set; }

        public GetTenureListResponse()
        {
            Tenures = new List<Tenure>();
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
