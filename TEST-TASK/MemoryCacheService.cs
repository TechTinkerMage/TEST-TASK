using Microsoft.Extensions.Caching.Memory;

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _cache;

    public MemoryCacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task AddAsync<T>(string key, T value, TimeSpan expiration)
    {
        _cache.Set(key, value, expiration);
        await Task.CompletedTask;
    }

    public async Task<T> GetAsync<T>(string key)
    {
        return await Task.FromResult(_cache.Get<T>(key));
    }

    public async Task RemoveAsync(string key)
    {
        _cache.Remove(key);
        await Task.CompletedTask;
    }
}