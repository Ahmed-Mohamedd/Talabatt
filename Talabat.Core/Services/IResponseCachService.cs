namespace Talabat.Core.Services
{
    public interface IResponseCachService
    {
        Task CacheResponseAsync(string key, object response, TimeSpan timeToLife);
        Task<string> GetCacheResponseAsync(string cacheKey);
    }
}
