using System;
using System.Collections.Generic;
using Hackney.Shared.HousingSearch.Gateways.Models.Assets;
using Hackney.Shared.HousingSearch.Gateways.Models.Tenures;
using Hackney.Shared.HousingSearch.Gateways.Models.Transactions;
using Nest;
using QueryablePerson = Hackney.Shared.HousingSearch.Gateways.Models.Persons.QueryablePerson;

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

            if (type == typeof(QueryableTransaction))
                return Indices.Index(new List<IndexName> { "transactions" });

            throw new NotImplementedException($"No index for type {typeof(T)}");
        }
    }
}
