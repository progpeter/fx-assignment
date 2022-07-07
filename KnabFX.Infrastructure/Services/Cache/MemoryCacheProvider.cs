using KnabFX.Application.Services.Cache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace KnabFX.Infrastructure.Services.Cache
{
    public class MemoryCacheProvider<T> : ICacheProvider<T>
    {
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;

        public MemoryCacheProvider(IMemoryCache cache, IConfiguration configuration)
        {
            _cache = cache;
            _configuration = configuration;
        }

        public T Get(string key)
        {
            if (_cache.TryGetValue(key, out T value))
            {
                return value;
            }

            return default(T);
        }

        public bool Set(string key, T value, int ttl = 0)
        {
            if (ttl == 0)
            {
                ttl = _configuration.GetValue<int>("Caching:DefaultTTL");
            }

            try
            {
                _cache.Set(key, value, TimeSpan.FromSeconds(ttl));
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Delete(string key)
        {
            try
            {
                _cache.Remove(key);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
