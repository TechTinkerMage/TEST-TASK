namespace TEST_TASK.ProviderOne
{
    public class ProviderOneSearchService : ISearchServiceProvider
    {
        private readonly HttpClient _httpClient;

        public ProviderOneSearchService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("TestTask");
        }

        public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
        {
            var providerOneRequest = new ProviderOneSearchRequest
            {
                From = request.Origin,
                To = request.Destination,
                DateFrom = request.OriginDateTime,
                DateTo = request.Filters?.DestinationDateTime,
                MaxPrice = request.Filters?.MaxPrice
            };

            var routes = RouteRepository.GetAll()
                .Where(r => r.Origin == providerOneRequest.From);
            
            return new SearchResponse
            {
                Routes = routes.ToArray(),
                MinPrice = routes.Min(r => r.Price),
                MaxPrice = routes.Max(r => r.Price),
                MinMinutesRoute = routes.Min(r => (int)r.DestinationDateTime.Subtract(r.OriginDateTime).TotalMinutes),
                MaxMinutesRoute = routes.Max(r => (int)r.DestinationDateTime.Subtract(r.OriginDateTime).TotalMinutes)
            };
        }

        public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync("https://www.google.com", cancellationToken);

            return response.IsSuccessStatusCode;
        }
    }
}
