using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models.Assets;
using HousingSearchApi.V1.Infrastructure;
using Nest;

namespace HousingSearchApi.V1.Interfaces
{
    public class AssetQueryGenerator : IQueryGenerator<QueryableAsset>
    {
        private readonly IWildCardAppenderAndPrepender _wildCardAppenderAndPrepender;

        public AssetQueryGenerator(IWildCardAppenderAndPrepender wildCardAppenderAndPrepender)
        {
            _wildCardAppenderAndPrepender = wildCardAppenderAndPrepender;
        }

        public QueryContainer Create(HousingSearchRequest request, QueryContainerDescriptor<QueryableAsset> q)
        {
            if (string.IsNullOrWhiteSpace(request.SearchText)) return null;

            var listOfWildCardedWords = _wildCardAppenderAndPrepender.Process(request.SearchText);

            var searchSurnames = q.QueryString(m =>
                m.Query(string.Join(' ', listOfWildCardedWords))
                    .Fields(f => f.Field(p => p.AssetAddress.AddressLine1)
                        .Field(p => p.AssetAddress.PostCode)
                        .Field(p => p.AssetAddress.Uprn))
                    .Type(TextQueryType.MostFields));

            return searchSurnames;
        }
    }
}
