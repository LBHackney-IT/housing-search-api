using FluentAssertions;
using Hackney.Shared.HousingSearch.Gateways.Models.Transactions;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Interfaces.Filtering;
using Nest;
using System;
using Xunit;

namespace HousingSearchApi.Tests.V1.Infrastructure.Filtering
{
    public class TransactionsFilterTests
    {
        private readonly TransactionsFilter _transactionDateRange;
        QueryContainerDescriptor<QueryableTransaction> _queryContainerDescriptor;
        public TransactionsFilterTests()
        {
            _transactionDateRange = new TransactionsFilter();
            _queryContainerDescriptor = new QueryContainerDescriptor<QueryableTransaction>();
        }

        [Fact]
        public void GetDescriptorIncorrectRequestTypeThrowsException()
        {
            var request = new GetTenureListRequest();
            Action act = () => _transactionDateRange.GetDescriptor(_queryContainerDescriptor, request);
            
            act.Should().Throw<ArgumentNullException>().WithMessage($"Value cannot be null. (Parameter '{nameof(request)}')");
        }

        [Fact]
        public void GetDescriptorStartEndDatesNullReturnsNull()
        {
            var request = new GetTransactionListRequest()
            {
                StartDate = null,
                EndDate = null
            };

            var queryContainerDescriptor = _transactionDateRange.GetDescriptor(_queryContainerDescriptor, request);
            queryContainerDescriptor.Should().BeNull();
        }

        [Fact]
        public void GetDescriptorStartEndDatesNotNullReturnsNotNull()
        {
            var request = new GetTransactionListRequest()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            };

            var queryContainerDescriptor = _transactionDateRange.GetDescriptor(_queryContainerDescriptor, request);
            queryContainerDescriptor.Should().NotBeNull();
        }

        [Fact]
        public void GetDescriptorStartDateNullReturnsNotNull()
        {
            var request = new GetTransactionListRequest()
            {
                StartDate = null,
                EndDate = DateTime.Now
            };

            var queryContainerDescriptor = _transactionDateRange.GetDescriptor(_queryContainerDescriptor, request);
            queryContainerDescriptor.Should().NotBeNull();
        }

        [Fact]
        public void GetDescriptorEndDateNullReturnsNotNull()
        {
            var request = new GetTransactionListRequest()
            {
                StartDate = DateTime.Now,
                EndDate = null
            };

            var queryContainerDescriptor = _transactionDateRange.GetDescriptor(_queryContainerDescriptor, request);
            queryContainerDescriptor.Should().NotBeNull();
        }
    }
}
