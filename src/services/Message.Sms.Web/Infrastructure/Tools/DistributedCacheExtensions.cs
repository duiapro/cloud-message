using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Text;

namespace Microsoft.Extensions.Caching.Distributed
{
    public static class DistributedCacheExtensions
    {
        public static async Task<T?> GetAsync<T>(this IDistributedCache distributedCache, string key)
        {
            var bytesObject = distributedCache.Get(key);
            if (bytesObject == null)
            {
                return default(T);
            }

            var stringObject = Encoding.UTF8.GetString(bytesObject);

            return JsonConvert.DeserializeObject<T>(stringObject, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }

        public static async Task<string?> GetStringAsync(this IDistributedCache distributedCache, string key)
        {
            return await distributedCache.GetAsync<string>(key);
        }

        public static async Task SetAsync(this IDistributedCache distributedCache, string key, object value,
                       DistributedCacheEntryOptions? options = null)
        {
            var stringObject = JsonConvert.SerializeObject(value, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.All
            });

            var bytesObject = Encoding.UTF8.GetBytes(stringObject);

            await distributedCache.SetAsync(key, bytesObject, options ?? default);//将字节数组存入Redis
            await distributedCache.RefreshAsync(key);//刷新Redis
        }

        public static async Task<T> GetOrCreateAsync<T>(this IDistributedCache distributedCache, string key, Func<DistributedCacheEntryOptions, Task<T>> factory)
        {
            T data = default(T)!;
            byte[]? bytesObject = await distributedCache.GetAsync(key);
            if (bytesObject is null)
            {
                var options = new DistributedCacheEntryOptions();
                data = await factory.Invoke(options);
                await distributedCache.SetAsync(key, data!, options);
            }
            else
            {
                var stringObject = Encoding.UTF8.GetString(bytesObject);
                data = JsonConvert.DeserializeObject<T>(stringObject, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All
                })!;
            }
            return data;
        }
    }
}
