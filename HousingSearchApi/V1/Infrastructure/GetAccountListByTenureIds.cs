using System.Collections.Generic;
using HousingSearchApi.V1.Boundary.Requests;

namespace HousingSearchApi.V1.Infrastructure
{
    /// <summary>
    /// Not request model. Will be used in SearchGateway to load a list of accounts for a list of Ids
    /// </summary>
    public class GetAccountListByTenureIds : HousingSearchRequest
    {
        public GetAccountListByTenureIds(IEnumerable<string> tenureIds)
        {
            TenureIds = tenureIds;
        }

        public IEnumerable<string> TenureIds { get; }
    }
}
