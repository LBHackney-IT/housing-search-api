using System;
using System.Collections.Generic;
using Hackney.Shared.HousingSearch.Gateways.Models.Assets;
using Hackney.Shared.HousingSearch.Gateways.Models.Persons;
using Hackney.Shared.HousingSearch.Gateways.Models.Tenures;
using Nest;
using QueryableTenure = Hackney.Shared.HousingSearch.Gateways.Models.Tenures.QueryableTenure;

namespace HousingSearchApi.V1.Interfaces
{
    public class IndexSelector : IIndexSelector
    {
        public Indices.ManyIndices Create<T>()
        {
            var type = typeof(T);

            if (type == typeof(QueryablePerson))
                return Indices.Index(new List<IndexName> { "persons" });

            if (type == typeof(QueryableTenure))
                return Indices.Index(new List<IndexName> { "tenures" });

            if (type == typeof(QueryableAsset))
                return Indices.Index(new List<IndexName> { "assets" });

            throw new NotImplementedException($"No index for type {typeof(T)}");
        }
    }
}
