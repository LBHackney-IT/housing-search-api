using FluentAssertions;
using HousingSearchApi.V1.Infrastructure;
using Xunit;

namespace HousingSearchApi.Tests.V1.Infrastructure
{
    public class WildCardAppenderAndPrependerTests
    {
        private WildCardAppenderAndPrepender _sut;

        public WildCardAppenderAndPrependerTests()
        {
            _sut = new WildCardAppenderAndPrepender();
        }

        [Theory]
        [InlineData("a")]
        [InlineData("a b")]
        [InlineData("a be cee")]
        public void GivenAPhraseWhenProcessedShouldAppendWildCardBeforeAndAfterEveryWord(string phrase)
        {
            // given + when
            var result = _sut.Process(phrase);

            // then
            var phraseArray = phrase.Split(" ");
            result.Count.Should().Be(result.Count);

            foreach (string word in phraseArray)
            {
                result.Contains("*" + word + "*").Should().BeTrue();
            }
        }
    }
}
