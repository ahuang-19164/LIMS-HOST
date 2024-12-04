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


using Yichen.Net.Caching.MemoryCache;
using Yichen.Net.Caching.Redis;
using Yichen.Net.Configuration;

namespace Yichen.Net.Caching.Manual
{
    /// <summary>
    /// 手动缓存调用
    /// </summary>
    public static partial class ManualDataCache
    {
        private static IManualCacheManager _instance = null;
        private static IManualCacheManager _UseRedis = null;
        private static IManualCacheManager _UseMemory = null;
        /// <summary>
        /// 根据配置选择缓存存储位置
        /// </summary>
        public static IManualCacheManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    if (AppSettingsConstVars.RedisUseCache)
                    {
                        _instance = new RedisCacheManager();
                    }
                    else
                    {
                        _instance = new MemoryCacheManager();

                    }
                }
                return _instance;
            }
        }
        /// <summary>
        /// 使用Redis存储
        /// </summary>
        public static IManualCacheManager RedisCache
        {
            get
            {
                if (_UseRedis == null)
                {
                    _UseRedis = new RedisCacheManager();
                }
                return _UseRedis;
            }
        }

        /// <summary>
        /// 使用内存存储
        /// </summary>
        public static IManualCacheManager MemoryCache
        {
            get
            {
                if (_UseMemory == null)
                {
                    _UseMemory = new MemoryCacheManager();
                }
                return _UseMemory;
            }
        }
    }
}
