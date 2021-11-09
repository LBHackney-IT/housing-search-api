using System;
using System.Collections.Generic;
using Hackney.Shared.HousingSearch.Gateways.Models.Accounts;
using Hackney.Shared.HousingSearch.Gateways.Models.Assets;
using Hackney.Shared.HousingSearch.Gateways.Models.Persons;
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

            if (type == typeof(QueryableAccount))
                return Indices.Index(new List<IndexName> { "accounts" });

            throw new NotImplementedException($"No index for type {typeof(T)}");
        }
    }
}
