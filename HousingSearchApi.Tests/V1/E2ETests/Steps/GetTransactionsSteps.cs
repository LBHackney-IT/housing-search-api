using FluentAssertions;
using HousingSearchApi.Tests.V1.E2ETests.Fixtures;
using HousingSearchApi.Tests.V1.E2ETests.Steps.Base;
using HousingSearchApi.Tests.V1.E2ETests.Steps.ResponseModels;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using HousingSearchApi.V1.Infrastructure;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HousingSearchApi.Tests.V1.E2ETests.Steps
{
    public class GetTransactionsSteps : BaseSteps
    {
        private const string BaseTransactionsRoute = "api/v1/search/transactions";
        public GetTransactionsSteps(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task WhenRequestDoesNotContainSearchString()
        {
            _lastResponse = await _httpClient.GetAsync(new Uri(BaseTransactionsRoute, UriKind.Relative)).ConfigureAwait(false);
        }

        public async Task WhenRequestContainsSearchText(string searchText = null)
        {
            _lastResponse = await _httpClient.GetAsync(new Uri($"{BaseTransactionsRoute}?searchText={searchText}", UriKind.Relative)).ConfigureAwait(false);
        }

        public async Task WhenAPageSizeIsProvided(int pageSize)
        {
            var route = new Uri($"{BaseTransactionsRoute}?searchText={TransactionsFixture.Persons.First().FullName}&pageSize={pageSize}",
                UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }

        public async Task ThenThatTextShouldBeInTheResult(string searchText)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (_lastResponse.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new Exception(resultBody);
            }

            _lastResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var parsedResponse = JsonSerializer.Deserialize<APIResponse<TransactionListDTO>>(resultBody, _jsonOptions);

            parsedResponse.Should().NotBeNull();
            parsedResponse.Results.Should().NotBeNull();
            parsedResponse.Results.Transactions.Should().NotBeNull();

            var transactions = parsedResponse.Results.Transactions;

            transactions.All(t =>
                t.Sender == null || t.Sender.FullName.SafeContains(searchText) ||
                t.TransactionType.ToString().SafeContains(searchText) ||
                t.PaymentReference.SafeContains(searchText) ||
                t.BankAccountNumber.SafeContains(searchText) ||
                t.TransactionAmount.ToString().SafeContains(searchText));
        }

        public async Task ThenTheReturningResultsShouldBeOfThatSize(int pageSize)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var parsedResponse = JsonSerializer.Deserialize<APIResponse<TransactionListDTO>>(resultBody, _jsonOptions);

            parsedResponse.Should().NotBeNull();
            parsedResponse.Results.Should().NotBeNull();
            parsedResponse.Results.Transactions.Should().NotBeNull();

            parsedResponse.Results.Transactions.Count.Should().BeLessOrEqualTo(pageSize);
        }
    }
}
