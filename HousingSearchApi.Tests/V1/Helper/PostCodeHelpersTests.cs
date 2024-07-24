using HousingSearchApi.V1.Helper;
using Xunit;

namespace HousingSearchApi.Tests.V1.Helper
{
    public class PostCodeHelpersTests
    {
        [Theory]
        // https://ideal-postcodes.co.uk/guides/uk-postcode-format
        [InlineData("AA9A 9AA", true)]
        [InlineData("A9A 9AA", true)]
        [InlineData("A9 9AA", true)]
        [InlineData("A99 9AA", true)]
        [InlineData("AA9 9AA", true)]
        [InlineData("AA99 9AA", true)]
        // https://assets.publishing.service.gov.uk/media/5a81ebbded915d74e6234d42/Appendix_C_ILR_2017_to_2018_v1_Published_28April17.pdf
        [InlineData("M1 1AA", true)]
        [InlineData("M60 1NW", true)]
        [InlineData("CR2 6XH", true)]
        [InlineData("DN55 1PT", true)]
        [InlineData("W1P 1HQ", true)]
        [InlineData("EC1A 1BB", true)]
        // Incorrect spacing but still valid
        [InlineData("AA999AA", true)]
        [InlineData("A a999AA", true)]
        [InlineData("AA 999aA", true)]
        [InlineData("AA9 99Aa", true)]
        [InlineData("Aa999 AA", true)]
        [InlineData("aA999A A", true)]
        [InlineData("eC1A 1bB ", true)]
        [InlineData(" ec1a 1bb", true)]
        // Random correct postcodes
        [InlineData("W1A 0AX", true)]
        [InlineData("M1 1AE", true)]
        [InlineData("B33 8TH", true)]
        [InlineData("EC1A1BB", true)]
        [InlineData("W1A0AX", true)]
        [InlineData("M11AE", true)]
        [InlineData("B338TH", true)]
        [InlineData("CR26XH", true)]
        [InlineData("DN551PT", true)]

        // Invalid postcodes or inputs
        [InlineData(null, false)]
        [InlineData(" ", false)]
        [InlineData("", false)]
        [InlineData("I am a string", false)]
        [InlineData("CT1!WX", false)]
        [InlineData("A&9A 9@A", false)]
        [InlineData("EC1A 1BBX", false)]
        [InlineData("EC1A1BBX", false)]
        [InlineData("EC1A1B", false)]
        [InlineData("INVALID", false)]
        [InlineData("123456", false)]
        public void SearchTextIsValidPostCode_ShouldReturnTrue(string input, bool expected)
        {
            // Act
            var result = PostCodeHelpers.SearchTextIsValidPostCode(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("AA9A9AA", "AA9A 9AA")]
        [InlineData("aa9a9AA", "AA9A 9AA")]
        [InlineData("Aa9a9Aa", "AA9A 9AA")]
        [InlineData(" AA9A9AA", "AA9A 9AA")]
        [InlineData("A A9A9AA", "AA9A 9AA")]
        [InlineData("A9 A9AA", "A9A 9AA")]
        [InlineData("A9A 9AA", "A9A 9AA")]
        [InlineData("A9A9 AA", "A9A 9AA")]
        [InlineData("A9A9A A", "A9A 9AA")]
        [InlineData("A9A9AA ", "A9A 9AA")]
        [InlineData("A9 A9 AA", "A9A 9AA")]
        [InlineData("A 9A9 AA", "A9A 9AA")]
        [InlineData("A9 A 9AA", "A9A 9AA")]
        [InlineData("A9A 9A A", "A9A 9AA")]

        [InlineData("A99AA", "A9 9AA")]
        [InlineData("a99aa", "A9 9AA")]
        [InlineData("a99Aa", "A9 9AA")]
        [InlineData(" A99AA", "A9 9AA")]
        [InlineData("A 99AA", "A9 9AA")]
        [InlineData("A9 9AA", "A9 9AA")]
        [InlineData("A99 AA", "A9 9AA")]
        [InlineData("A99A A", "A9 9AA")]
        [InlineData("A99AA ", "A9 9AA")]
        [InlineData("A9 9A A", "A9 9AA")]
        [InlineData("A99 A A", "A9 9AA")]
        [InlineData("A99A A ", "A9 9AA")]

        [InlineData("AA999AA", "AA99 9AA")]
        [InlineData("aa999aa", "AA99 9AA")]
        [InlineData("aA999Aa", "AA99 9AA")]
        [InlineData("Aa999aA", "AA99 9AA")]
        [InlineData(" AA999AA", "AA99 9AA")]
        [InlineData("A A999AA", "AA99 9AA")]
        [InlineData("AA 999AA", "AA99 9AA")]
        [InlineData("AA9 99AA", "AA99 9AA")]
        [InlineData("AA99 9AA", "AA99 9AA")]
        [InlineData("AA999 AA", "AA99 9AA")]
        [InlineData("AA999A A", "AA99 9AA")]
        [InlineData("AA999AA ", "AA99 9AA")]
        [InlineData("A A999 AA", "AA99 9AA")]
        [InlineData("AA 999 AA", "AA99 9AA")]
        [InlineData("AA9 99A A", "AA99 9AA")]
        [InlineData("AA99 9 AA", "AA99 9AA")]


        public void NormalizePostcode_ShouldReturnNoSpaces(string input, string expected)
        {
            // Act
            var result = PostCodeHelpers.NormalizePostcode(input);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}


