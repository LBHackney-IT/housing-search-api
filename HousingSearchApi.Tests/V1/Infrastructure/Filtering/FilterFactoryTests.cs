using FluentAssertions;
using Hackney.Shared.HousingSearch.Gateways.Models.Transactions;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Infrastructure.Filtering;
using HousingSearchApi.V1.Interfaces.Filtering;
using Xunit;

namespace HousingSearchApi.Tests.V1.Infrastructure.Filtering
{
    public class FilterFactoryTests
    {
        private readonly FilterFactory _filterFactory;

        public FilterFactoryTests()
        {
            _filterFactory = new FilterFactory();
        }

        [Fact]
        public void CreateQueryableTransactionTypeReturnsTransactionsFilter()
        {
            var request = new GetTenureListRequest();
            var transactionFilter = _filterFactory.Create<QueryableTransaction, GetTenureListRequest>(request);
            transactionFilter.Should().BeOfType<TransactionsFilter>();
        }

        [Fact]
        public void CreateNotQueryableTransactionReturnsDefaultFilter()
        {
            var request = new GetTenureListRequest();
            var transactionFilter = _filterFactory.Create<QueryableSender, GetTenureListRequest>(request);
            transactionFilter.Should().BeOfType<DefaultFilter<QueryableSender>>();
        }
    }
}
