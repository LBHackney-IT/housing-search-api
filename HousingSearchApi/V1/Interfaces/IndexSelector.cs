using System;
using System.Collections.Generic;
using HousingSearchApi.V1.Gateways.Models.Assets;
using HousingSearchApi.V1.Gateways.Models.Persons;
using HousingSearchApi.V1.Gateways.Models.Tenures;
using Nest;

namespace HousingSearchApi.V1.Interfaces
{
    public class IndexSelector : IIndexSelector
    {
        public Indices.ManyIndices Create<T>()
        {
            var type = typeof(T);

            if (type == typeof(QueryablePerson))
                return Indices.Index(new List<IndexName> { "persons" });

            if (type == typeof(Gateways.Models.Tenures.QueryableTenure))
                return Indices.Index(new List<IndexName> { "tenures" });

            if (type == typeof(QueryableAsset))
                return Indices.Index(new List<IndexName> { "assets" });

            throw new NotImplementedException($"No index for type {typeof(T)}");
        }
    }
}
