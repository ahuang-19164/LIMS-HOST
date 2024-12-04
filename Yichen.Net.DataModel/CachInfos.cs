using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yichen.Net.DataModel
{
    public class CachInfos<T>
    {
        /// <summary>
        /// </summary>
        /// key名称
        public string key { get; set; }
        /// <summary>
        /// 表名称
        /// </summary>
        public string tablename { get; set; }
        /// <summary>
        /// 缓存中的名称
        /// </summary>
        public string disname { get; set; }
        /// <summary>
        /// 数据集合
        /// </summary>
        public List<T> data { get; set; }
    }
}
