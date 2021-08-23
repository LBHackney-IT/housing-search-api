using System.Collections.Generic;

namespace HousingSearchApi.V1.Infrastructure
{
    public class WildCardAppenderAndPrepender : IWildCardAppenderAndPrepender
    {
        public List<string> Process(string phrase)
        {
            var listOfWildcardWords = new List<string>();

            foreach (var word in phrase.Split(' '))
            {
                listOfWildcardWords.Add($"*{word}*");
            }

            return listOfWildcardWords;
        }

        public List<string> Process(IEnumerable<string> phrases)
        {
            var listOfWildcardWords = new List<string>();

            foreach (var phrase in phrases)
            {
                listOfWildcardWords.Add($"*{phrase}*");
            }

            return listOfWildcardWords;
        }
    }
}
