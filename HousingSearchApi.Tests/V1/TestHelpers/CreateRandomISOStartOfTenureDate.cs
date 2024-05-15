using AutoFixture;
using System;
using QueryableTenure = Hackney.Shared.HousingSearch.Gateways.Models.Tenures.QueryableTenure;

namespace HousingSearchApi.Tests.V1.TestHelpers
{
    public class CreateRandomISOStartOfTenureDate : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<QueryableTenure>(x =>
                x.With(t => t.StartOfTenureDate, GenerateRandomTenureStartDate()));
        }

        private string GenerateRandomTenureStartDate()
        {
            //pick a sensible random day in the past (today inclusive)
            var addDays = new Random().Next(TenureStartDateAddDaysMinValue, TenureStartDateAddDaysMaxValue);

            return DateTime.UtcNow.AddDays(addDays).ToString("yyyy-MM-ddTHH:mm:ssZ");
        }
        public static int TenureStartDateAddDaysMinValue
        {
            get { return -3000; }
        }

        public static int TenureStartDateAddDaysMaxValue
        {
            get { return 1; }
        }
    }
}
