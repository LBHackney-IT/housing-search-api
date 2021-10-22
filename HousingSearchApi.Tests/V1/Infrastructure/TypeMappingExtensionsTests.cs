using FluentAssertions;
using Hackney.Shared.HousingSearch.Domain;
using HousingSearchApi.V1.Infrastructure;
using System;
using System.Collections.Generic;
using Xunit;

namespace HousingSearchApi.Tests.V1.Infrastructure
{
    public class TypeMappingExtensionsTests
    {
        [Fact]
        public void ShouldReturnListWithTenurePersonTypes()
        {
            var expected = new List<string>(23)
            {
                "Asylum Seeker",
                "Commercial Let",
                "Introductory",
                "License Temp Ac",
                "Lse 100% Stair",
                "Mesne Profit Ac",
                "Non-Secure",
                "Private Garage",
                "Private Sale LH",
                "RenttoMortgage",
                "Secure",
                "Shared Equity",
                "Shared Owners",
                "Short Life Lse",
                "Temp Annex",
                "Temp B&B",
                "Temp Decant",
                "Temp Hostel",
                "Temp Hostel Lse",
                "Temp Private Lt",
                "Temp Traveller",
                "Tenant Acc Flat",
                "Tenant Garage"
            };

            var personTypes = PersonType.Tenant.GetPersonTypes();

            personTypes.Should().NotBeNullOrEmpty().And.HaveCount(23);
            personTypes.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ShouldReturnListWithLeaseholderPersonTypes()
        {
            var expected = new List<string>(4)
            {
                "Freehold",
                "Freehold (Serv)",
                "Leasehold (RTB)",
                "Registered Social Landlord"
            };

            var personTypes = PersonType.Leaseholder.GetPersonTypes();

            personTypes.Should().NotBeNullOrEmpty().And.HaveCount(4);
            personTypes.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ShouldThrowsArgumentException()
        {
            // We need to check for the value that is NOT in enum
            PersonType type = (PersonType) 2;

            Action act = () => type.GetPersonTypes();

            act.Should().Throw<ArgumentException>();
        }
    }
}
