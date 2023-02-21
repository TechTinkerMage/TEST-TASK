using Microsoft.Extensions.Caching.Memory;
using System.Text;

public class SearchService : ISearchService
{
    private readonly ISearchServiceProvider[] _searchServices;
    private readonly IMemoryCache _cache;

    public SearchService(IServiceProvider provider, IMemoryCache cache)
    {
        _searchServices = provider.GetServices<ISearchServiceProvider>().ToArray();
        _cache = cache;
    }

    public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
    {
        var tasks = _searchServices.Select(s => s.IsAvailableAsync(cancellationToken));

        var results = await Task.WhenAll(tasks);

        return results.Any(r => r);
    }

    public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
    {
        var cacheKey = GetCacheKey(request);
        
        if (request.Filters?.OnlyCached == true)
        {
            if (_cache.TryGetValue(cacheKey, out SearchResponse resp))
            {
                return resp;
            }
        }
        
        var tasks = _searchServices.Select(s => s.SearchAsync(request, cancellationToken));
        var responses = await Task.WhenAll(tasks);

        var responseResult = new SearchResponse()
        {
            Routes = responses.SelectMany(x => x.Routes).ToArray(),
            MinPrice = responses.Min(x => x.MinPrice),
            MaxPrice = responses.Max(x => x.MaxPrice),
            MinMinutesRoute = responses.Min(x => x.MinMinutesRoute),
            MaxMinutesRoute = responses.Max(x => x.MaxMinutesRoute)
        };

        var cacheOptions = new MemoryCacheEntryOptions()
    .SetSlidingExpiration(TimeSpan.FromMinutes(5));
        _cache.Set(cacheKey, responseResult, cacheOptions);

        return responseResult;
    }

    private string GetCacheKey(SearchRequest request)
    {
        var cacheKey = new StringBuilder()
            .Append(request.Origin)
            .ToString();

        return cacheKey;
    }
}
