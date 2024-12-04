/***********************************************************************
 *            Project: Yichen.Net                                     *
 *                Web: https://Yichen.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *           Versions: 1.0                                             *
 *         CreateTime: 2020-02-02 14:43:16
 *          NameSpace: Yichen.Net.Framework.Caching
 *           FileName: DataCache
 *   ClassDescription: 
 ***********************************************************************/


using Yichen.Net.Caching.Manuals;
using Yichen.Net.Caching.MemoryCache;
using Yichen.Net.Caching.Redis;
using Yichen.Net.Configuration;

namespace Yichen.Net.Caching.Manuals
{
    /// <summary>
    /// 手动缓存调用
    /// </summary>
    public static partial class ManualDataCache<T>
    {
        private static IManualCacheManager<T> _instance = null;
        private static IManualCacheManager<T> _UseRedis = null;
        private static IManualCacheManager<T> _UseMemory = null;
        /// <summary>
        /// 根据配置选择缓存存储位置
        /// </summary>
        public static IManualCacheManager<T> Instance
        {
            get
            {
                if (_instance == null)
                {
                    if (AppSettingsConstVars.RedisUseCache)
                    {
                        _instance = new RedisCacheManager<T>();
                    }
                    else
                    {
                        _instance = new MemoryCacheManager<T>();

                    }
                }
                return _instance;
            }
        }
        /// <summary>
        /// 使用Redis存储
        /// </summary>
        public static IManualCacheManager<T> RedisCache
        {
            get
            {
                if (_UseRedis == null)
                {
                    _UseRedis = new RedisCacheManager<T>();
                }
                return _UseRedis;
            }
        }

        /// <summary>
        /// 使用内存存储
        /// </summary>
        public static IManualCacheManager<T> MemoryCache
        {
            get
            {
                if (_UseMemory == null)
                {
                    _UseMemory = new MemoryCacheManager<T>();
                }
                return _UseMemory;
            }
        }
    }
}
