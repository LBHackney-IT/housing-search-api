using System;
using System.Collections.Generic;
using Hackney.Core.ElasticSearch.Interfaces;
using Hackney.Shared.HousingSearch.Gateways.Models.Tenures;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Interfaces;
using HousingSearchApi.V1.Interfaces.Factories;
using Nest;

namespace HousingSearchApi.V1.Infrastructure.Factories
{
    public class TenureQueryGenerator : IQueryGenerator<QueryableTenure>
    {
        private readonly IQueryBuilder<QueryableTenure> _queryBuilder;
        private readonly IFilterQueryBuilder<QueryableTenure> _queryFilterBuilder;

        public TenureQueryGenerator(IQueryBuilder<QueryableTenure> queryBuilder,
            IFilterQueryBuilder<QueryableTenure> queryFilterBuilder)
        {
            _queryBuilder = queryBuilder;
            _queryFilterBuilder = queryFilterBuilder;
        }

        public QueryContainer Create<TRequest>(TRequest request, QueryContainerDescriptor<QueryableTenure> q)
        {
            if (!(request is GetTenureListRequest tenureListRequest))
                throw new ArgumentNullException($"{nameof(request)} shouldn't be null.");

            if (string.IsNullOrEmpty(tenureListRequest.SearchText) && string.IsNullOrEmpty(tenureListRequest.Uprn))
            {
                throw new ArgumentNullException($"{nameof(request).ToString()} should include seachText or Uprn.");
            }

            if (tenureListRequest.SearchText != null && tenureListRequest.SearchText.Length > 0)
            {

                return _queryFilterBuilder
                    .WithMultipleFilterQuery(tenureListRequest.IsTemporaryAccommodation, new List<string> { "tenuredAsset.isTemporaryAccommodation" })
                    .WithFilterQuery(tenureListRequest.BookingStatus, new List<string> { "tempAccommodationInfo.bookingStatus" })
                    .WithWildstarQuery(tenureListRequest.SearchText,
                        new List<string>
                        {
                            "paymentReference",
                            "tenuredAsset.fullAddress^3",
                            "householdMembers",
                            "householdMembers.fullName^3",
                        })
                    .Build(q);
            }
            else
            {
                return _queryBuilder
                .WithExactQuery(tenureListRequest.Uprn,
                new List<string>
                {
                    "tenuredAsset.uprn",
                })
                .Build(q);
            }
        }
    }
}
