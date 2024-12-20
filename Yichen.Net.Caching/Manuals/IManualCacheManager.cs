﻿using System.Collections.Generic;
using Yichen.Comm.Model;

namespace Yichen.Net.Caching.Manuals
{
    /// <summary>
    /// 手动缓存操作接口
    /// </summary>
    public interface IManualCacheManager<T>
    {

        /// <summary>
        /// 验证缓存项是否存在
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        bool Exists(string key);


        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresIn">缓存时长(分钟)</param>
        /// <returns></returns>
        bool Set(string key, object value, int expiresIn = 0);


        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        void Remove(string key);


        /// <summary>
        /// 批量删除缓存
        /// </summary>
        /// <returns></returns>
        void RemoveAll(IEnumerable<string> keys);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        T Get<T>(string key);


        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        object Get(string key);

        /// <summary>
        /// 获取缓存集合
        /// </summary>
        /// <param name="keys">缓存Key集合</param>
        /// <returns></returns>
        IDictionary<string, object> GetAll(IEnumerable<string> keys);

        /// <summary>
        /// 删除所有缓存
        /// </summary>
        void RemoveCacheAll();

        /// <summary>
        /// 删除匹配到的缓存
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        void RemoveCacheRegex(string pattern);


        /// <summary>
        /// 搜索 匹配到的缓存
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        IList<string> SearchCacheRegex(string pattern);




        /// <summary>
        /// 判断表是否存在，不存在刷新表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        List<T> LIMSGetKeyValue(CachInfos<T> keyinfo);

    }
}
