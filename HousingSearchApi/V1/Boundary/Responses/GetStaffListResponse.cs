using Hackney.Shared.HousingSearch.Domain.Staff;
using System.Collections.Generic;

namespace HousingSearchApi.V1.Boundary.Responses
{
    public class GetStaffListResponse
    {
        private long _total;

        public List<Staff> Staff { get; set; }

        public GetStaffListResponse()
        {
            Staff = new List<Staff>();
        }

        public void SetTotal(long total)
        {
            _total = total < 0 ? 0 : total;
        }

        public long Total()
        {
            return _total;
        }
    }
}
