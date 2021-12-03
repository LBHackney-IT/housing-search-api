using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FluentAssertions;

namespace HousingSearchApi.Tests.V1.E2ETests.Steps.Base
{
    public class BaseSteps
    {
        protected readonly HttpClient _httpClient;

        protected HttpResponseMessage _lastResponse;
        protected readonly JsonSerializerOptions _jsonOptions;

        public BaseSteps(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = CreateJsonOptions();
        }

        protected JsonSerializerOptions CreateJsonOptions()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            options.Converters.Add(new JsonStringEnumConverter());
            return options;
        }

        public async Task ThenTheLastRequestShouldBeBadRequestResult(string expectedErrorMessage = null)
        {
            _lastResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            if (expectedErrorMessage != null)
            {
                var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                resultBody.Should().NotBeNull();
                resultBody.Should().Contain(expectedErrorMessage);
            }
        }

        public async Task ThenTheLastRequestShouldBe200()
        {
            if (_lastResponse.StatusCode != HttpStatusCode.OK)
            {
                var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                resultBody.Should().NotBeNull();
                throw new Exception(resultBody);
            }
            _lastResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
