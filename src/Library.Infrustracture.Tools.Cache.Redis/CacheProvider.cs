using Lipar.Core.Contract.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;

namespace Library.Infrustracture.Tools.Cache.Redis
{
    public class CacheProvider : ICacheProvider 
    {
        private readonly IDistributedCache _cache;
        private readonly IJsonService _json;

        public CacheProvider(IDistributedCache cache, IJsonService json)
        {
            _cache = cache;
            _json = json;
        }

        public async Task<T> Get<T>(string key) where T : class
        {
            var cachedResponse = await _cache.GetStringAsync(key);
            return cachedResponse is null ? null : _json.DeserializeObject<T>(cachedResponse);
        }

        public async Task Set<T>(string key, T value, DistributedCacheEntryOptions options) where T : class
        {
            var response = _json.SerializeObject(value);
            await _cache.SetStringAsync(key, response, options);
        }

        public async Task Remove(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }

    public interface ICacheProvider
    {
        Task<T> Get<T>(string key) where T : class;

        Task Set<T>(string key, T value, DistributedCacheEntryOptions options) where T : class;

        Task Remove(string key);
    }
}
