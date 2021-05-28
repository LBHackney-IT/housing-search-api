using System.Collections.Generic;

namespace HousingSearchApi.V1.Infrastructure
{
    public interface IWildCardAppenderAndPrepender
    {
        List<string> Process(string phrase);
    }
}
