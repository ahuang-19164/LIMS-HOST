using Microsoft.Extensions.Caching.Memory;
using System;

namespace Yichen.Net.Caching.AutoMate.MemoryCache
{
    /// <summary>
    /// 实例化缓存接口ICaching
    /// </summary>
    public class MemoryCaching : ICachingProvider
    {
        //引用Microsoft.Extensions.Caching.Memory;这个和.net 还是不一样，没有了Httpruntime了
        private readonly IMemoryCache _cache;
        //还是通过构造函数的方法，获取
        public MemoryCaching(IMemoryCache cache)
        {
            _cache = cache;
        }
        /// <summary>
        /// 获取内存中的值
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public object Get(string cacheKey)
        {
            return _cache.Get(cacheKey);
        }
        /// <summary>
        /// 向内存中插入值
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="cacheValue"></param>
        /// <param name="timeSpan"></param>
        public void Set(string cacheKey, object cacheValue, int timeSpan)
        {
            _cache.Set(cacheKey, cacheValue, TimeSpan.FromSeconds(timeSpan * 60));
        }
    }

}
