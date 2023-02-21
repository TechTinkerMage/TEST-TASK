public interface ICacheService
{
    Task AddAsync<T>(string key, T value, TimeSpan expiration);
    Task<T> GetAsync<T>(string key);
    Task RemoveAsync(string key);
}
