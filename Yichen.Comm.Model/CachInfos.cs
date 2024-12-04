﻿namespace Yichen.Comm.Model
{
    public class CachInfos<T>
    {
        /// <summary>
        /// </summary>
        /// key名称
        public string? key { get; set; }
        /// <summary>
        /// 表名称
        /// </summary>
        public string? tablename { get; set; }
        /// <summary>
        /// 缓存中的名称
        /// </summary>
        public string? disname { get; set; }
        /// <summary>
        /// 数据集合
        /// </summary>
        public List<T> data { get; set; }
    }
}
