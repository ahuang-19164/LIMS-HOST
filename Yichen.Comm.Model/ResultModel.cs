using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yichen.Comm.Model
{
    public class ResultModel
    {
        /// <summary>
        /// 信息编号
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 信息状态
        /// </summary>

        public bool state { get; set; }
        /// <summary>
        /// 信息被指
        /// </summary>
        public string msg { get; set; }
    }
}
