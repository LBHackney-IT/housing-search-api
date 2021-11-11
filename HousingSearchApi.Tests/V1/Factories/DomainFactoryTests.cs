using AutoFixture;
using FluentAssertions;
using Hackney.Shared.HousingSearch.Domain.Transactions;
using HousingSearchApi.V1.Factories;
using Xunit;

namespace HousingSearchApi.Tests.V1.Factories
{
    public class DomainFactoryTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public void CanMapASharedDomainTargetTypeObjectToADomainObject()
        {
            var sharedDomain = _fixture.Create<TargetType>();
            var domain = sharedDomain.ToDomain();
            sharedDomain.Should().BeEquivalentTo(domain);
        }

        [Fact]
        public void CanMapASharedDomainTransactionTypeObjectToADomainObject()
        {
            var sharedDomain = _fixture.Create<TransactionType>();
            var domain = sharedDomain.ToDomain();
            sharedDomain.Should().BeEquivalentTo(domain);
        }

        [Fact]
        public void CanMapASharedDomainSuspenseResolutionInfoObjectToADomainObject()
        {
            var sharedDomain = _fixture.Create<SuspenseResolutionInfo>();
            var domain = sharedDomain.ToDomain();
            sharedDomain.Should().BeEquivalentTo(domain);
        }

        [Fact]
        public void CanMapASharedDomainPersonTypeObjectToADomainObject()
        {
            var sharedDomain = _fixture.Create<Person>();
            var domain = sharedDomain.ToDomain();
            sharedDomain.Should().BeEquivalentTo(domain);
        }
    }
}
