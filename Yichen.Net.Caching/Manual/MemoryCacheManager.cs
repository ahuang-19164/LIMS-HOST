﻿using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Yichen.Comm.Model;
using Yichen.Net.Caching.Manual;
using Yichen.Net.Caching;

namespace Yichen.Net.Caching.MemoryCache
{
    public class MemoryCacheManager: IManualCacheManager
    {
        private static volatile Microsoft.Extensions.Caching.Memory.MemoryCache _memoryCache = new Microsoft.Extensions.Caching.Memory.MemoryCache((IOptions<MemoryCacheOptions>)new MemoryCacheOptions());

        /// <summary>
        /// 验证缓存项是否存在
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            return _memoryCache.TryGetValue(key, out _);
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
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (expiresIn > 0)
            {
                _memoryCache.Set(key, value,
                    new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(120))
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(expiresIn)));
            }
            else
            {
                _memoryCache.Set(key, value,
                    new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(120))
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(1440)));
            }

            return Exists(key);
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public void Remove(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            _memoryCache.Remove(key);
        }

        /// <summary>
        /// 批量删除缓存
        /// </summary>
        /// <returns></returns>
        public void RemoveAll(IEnumerable<string> keys)
        {
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));

            keys.ToList().ForEach(item => _memoryCache.Remove(item));
        }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            return _memoryCache.Get<T>(key);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public object Get(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            return _memoryCache.Get(key);
        }

        /// <summary>
        /// 获取缓存集合
        /// </summary>
        /// <param name="keys">缓存Key集合</param>
        /// <returns></returns>
        public IDictionary<string, object> GetAll(IEnumerable<string> keys)
        {
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));

            var dict = new Dictionary<string, object>();
            keys.ToList().ForEach(item => dict.Add(item, _memoryCache.Get(item)));
            return dict;
        }


        /// <summary>
        /// 删除所有缓存
        /// </summary>
        public void RemoveCacheAll()
        {
            var l = GetCacheKeys();
            foreach (var s in l)
            {
                Remove(s);
            }
        }

        /// <summary>
        /// 删除匹配到的缓存
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public void RemoveCacheRegex(string pattern)
        {
            IList<string> l = SearchCacheRegex(pattern);
            foreach (var s in l)
            {
                Remove(s);
            }
        }

        /// <summary>
        /// 搜索 匹配到的缓存
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public IList<string> SearchCacheRegex(string pattern)
        {
            var cacheKeys = GetCacheKeys();
            var l = cacheKeys.Where(k => Regex.IsMatch(k, pattern)).ToList();
            return l.AsReadOnly();
        }

        ///// <summary>
        ///// 判断表是否存在，不存在刷新表
        ///// </summary>
        ///// <param name="data"></param>
        ///// <returns></returns>
        //public List<T> LIMSGetKeyValue(CachInfos<T> keyinfo)
        //{

        //    if (keyinfo == null)
        //        throw new ArgumentNullException(nameof(keyinfo));
        //    bool state = ManualDataCache<T>.MemoryCache.Exists(keyinfo.disname);
        //    if (state)
        //        return keyinfo.data;
        //    var group_test = SqlSugarHelper.SqlSugarClientCreate().Queryable<T>().ToList();
        //    string grouptestkey = keyinfo.key + "-" + Guid.NewGuid().ToString("N");
        //    ManualDataCache<T>.MemoryCache.Set(grouptestkey, group_test);
        //    CachInfos<T> cachInfo3 = new CachInfos<T>();
        //    keyinfo.key = keyinfo.key;
        //    keyinfo.tablename = keyinfo.tablename;
        //    keyinfo.disname = grouptestkey;
        //    keyinfo.data = group_test;
        //    keyinfo = cachInfo3;
        //    return keyinfo.data;

        //}



        /// <summary>
        /// 获取所有缓存键
        /// </summary>
        /// <returns></returns>
        public List<string> GetCacheKeys()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = _memoryCache.GetType().GetField("_entries", flags).GetValue(_memoryCache);
            var cacheItems = entries as IDictionary;
            var keys = new List<string>();
            if (cacheItems == null) return keys;
            foreach (DictionaryEntry cacheItem in cacheItems)
            {
                keys.Add(cacheItem.Key.ToString());
            }
            return keys;
        }
    }
}
