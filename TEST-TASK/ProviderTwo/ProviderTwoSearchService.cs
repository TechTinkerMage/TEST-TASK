using Microsoft.Extensions.Caching.Memory;

namespace TEST_TASK.ProviderTwo
{
    public class ProviderTwoSearchService : ISearchServiceProvider
    {
        private readonly HttpClient _httpClient;
        
        public ProviderTwoSearchService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("TestTask");
        }

        public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync("https://www.microsoft.com", cancellationToken);

            return response.IsSuccessStatusCode;
        }

        public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
        {
            var providerTwoRequest = new ProviderTwoSearchRequest
            {
                Departure = request.Origin,
                Arrival = request.Destination,
                DepartureDate = request.OriginDateTime,
                MinTimeLimit = request.Filters?.MinTimeLimit
            };

            var routes = RouteRepository.GetAll()
                .Where(r => r.Origin == providerTwoRequest.Departure);


            return new SearchResponse
            {
                Routes = routes.ToArray(),
                MinPrice = routes.Min(r => r.Price),
                MaxPrice = routes.Max(r => r.Price),
                MinMinutesRoute = routes.Min(r => (int)r.DestinationDateTime.Subtract(r.OriginDateTime).TotalMinutes),
                MaxMinutesRoute = routes.Max(r => (int)r.DestinationDateTime.Subtract(r.OriginDateTime).TotalMinutes)
            };
        }
    }
}