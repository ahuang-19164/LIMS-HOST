﻿using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yichen.Comm.Model;
using Yichen.Net.Caching.Manuals;
using Yichen.Net.Configuration;
using Yichen.Net.Utility.Extensions;

namespace Yichen.Net.Caching.Manuals
{
    public class RedisCacheManager<T> : IManualCacheManager<T>
    {
        private readonly string _redisConnenctionString;

        public volatile ConnectionMultiplexer RedisConnection;

        private readonly object _redisConnectionLock = new object();

        public RedisCacheManager()
        {
            string redisConfiguration = AppSettingsConstVars.RedisConfigConnectionString;//获取连接字符串

            if (string.IsNullOrWhiteSpace(redisConfiguration))
            {
                throw new ArgumentException("redis config is empty", nameof(redisConfiguration));
            }
            _redisConnenctionString = redisConfiguration;

            RedisConnection = GetRedisConnection();
        }

        /// <summary>
        /// 核心代码，获取连接实例
        /// 通过双if 夹lock的方式，实现单例模式
        /// </summary>
        /// <returns></returns>
        private ConnectionMultiplexer GetRedisConnection()
        {
            //如果已经连接实例，直接返回
            if (RedisConnection != null && RedisConnection.IsConnected)
            {
                return RedisConnection;
            }
            //加锁，防止异步编程中，出现单例无效的问题
            lock (_redisConnectionLock)
            {
                if (RedisConnection != null)
                {
                    //释放redis连接
                    RedisConnection.Dispose();
                }
                try
                {
                    RedisConnection = ConnectionMultiplexer.Connect(_redisConnenctionString);
                }
                catch (Exception)
                {
                    throw new Exception("Redis服务未启用，请开启该服务，并且请注意端口号，Redis默认使用6379端口号。");
                }
            }
            return RedisConnection;
        }


        /// <summary>
        /// 判断key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            return RedisConnection.GetDatabase().KeyExists(key);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresIn">缓存时间</param>
        /// <returns></returns>
        public bool Set(string key, object value, int expiresIn = 0)
        {
            if (value != null)
            {
                //序列化，将object值生成RedisValue
                if (expiresIn > 0)
                {
                    return RedisConnection.GetDatabase().StringSet(key, SerializeExtensions.Serialize(value), TimeSpan.FromMinutes(expiresIn));
                }
                else
                {
                    return RedisConnection.GetDatabase().StringSet(key, SerializeExtensions.Serialize(value));
                }
            }
            return false;
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public void Remove(string key)
        {
            RedisConnection.GetDatabase().KeyDelete(key);
        }

        /// <summary>
        /// 批量删除缓存
        /// </summary>
        /// <returns></returns>
        public void RemoveAll(IEnumerable<string> keys)
        {
            foreach (var key in keys)
            {
                RedisConnection.GetDatabase().KeyDelete(key);
            }
        }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            var value = RedisConnection.GetDatabase().StringGet(key);
            if (value.HasValue)
            {
                //需要用的反序列化，将Redis存储的Byte[]，进行反序列化
                return SerializeExtensions.Deserialize<T>(value);
            }

            return default;
        }

        public object Get(string key)
        {
            return RedisConnection.GetDatabase().StringGet(key);
        }

        public IDictionary<string, object> GetAll(IEnumerable<string> keys)
        {
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));
            var dict = new Dictionary<string, object>();

            keys.ToList().ForEach(item => dict.Add(item, RedisConnection.GetDatabase().StringGet(item)));
            return dict;

        }
        /// <summary>
        /// 情况所有数据
        /// </summary>
        public void RemoveCacheAll()
        {
            foreach (var endPoint in GetRedisConnection().GetEndPoints())
            {
                var server = GetRedisConnection().GetServer(endPoint);
                foreach (var key in server.Keys())
                {
                    RedisConnection.GetDatabase().KeyDelete(key);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        public void RemoveCacheRegex(string pattern)
        {
            var script = "return redis.call('keys',@pattern)";
            var prepared = LuaScript.Prepare(script);
            var redisResult = RedisConnection.GetDatabase().ScriptEvaluate(prepared, new { pattern });
            if (!redisResult.IsNull)
            {
                RedisConnection.GetDatabase().KeyDelete((RedisKey[])redisResult); //删除一组key
            }
        }

        public IList<string> SearchCacheRegex(string pattern)
        {
            var list = new List<String>();
            var script = "return redis.call('keys',@pattern)";
            var prepared = LuaScript.Prepare(script);
            var redisResult = RedisConnection.GetDatabase().ScriptEvaluate(prepared, new { pattern });
            if (!redisResult.IsNull)
            {
                foreach (var key in (RedisKey[])redisResult)
                {
                    list.Add(RedisConnection.GetDatabase().StringGet(key));
                }
            }
            return list;
        }

        public List<T> LIMSGetKeyValue(CachInfos<T> keyinfo)
        {
            if (keyinfo == null)
                throw new ArgumentNullException(nameof(keyinfo));
            bool state = ManualDataCache<T>.MemoryCache.Exists(keyinfo.disname);
            if (state)
                return keyinfo.data;
            var group_test = SqlSugarHelper.SqlSugarClientCreate().Queryable<T>().ToList();
            string grouptestkey = keyinfo.key + "-" + Guid.NewGuid().ToString("N");
            ManualDataCache<T>.MemoryCache.Set(grouptestkey, group_test);
            CachInfos<T> cachInfo3 = new CachInfos<T>();
            keyinfo.key = keyinfo.key;
            keyinfo.tablename = keyinfo.tablename;
            keyinfo.disname = grouptestkey;
            keyinfo.data = group_test;
            keyinfo = cachInfo3;
            return keyinfo.data;
        }
    }
}
