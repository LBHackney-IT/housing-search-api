using Hackney.Shared.HousingSearch.Domain.Process;
using System.Collections.Generic;

namespace HousingSearchApi.V1.Boundary.Responses
{
    public class GetProcessListResponse
    {
        private long _total;

        public List<Process> Processes { get; set; }

        public GetProcessListResponse()
        {
            Processes = new List<Process>();
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
