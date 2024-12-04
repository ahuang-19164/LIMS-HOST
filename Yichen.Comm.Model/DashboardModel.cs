using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yichen.Comm.Model
{
    /// <summary>
    /// 首页展示信息对象
    /// </summary>
    public class DashboardModel
    {
        public viewSrouce? cardView { get; set; }
        public viewSrouce? piesView { get; set; }
        public viewSrouce chartView { get; set; }
        public viewSrouce otherView { get; set; }
    }

    public class viewSrouce
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 对应值
        /// </summary>
        public List<viewData> data { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
    }
    public class viewData
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// 对应值
        /// </summary>
        public string number { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
    }

}

