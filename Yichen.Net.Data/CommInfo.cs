using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yichen.Comm.Model;
using Yichen.Finance.Model.table;
using Yichen.Flow.Model;
using Yichen.System.Model;

namespace Yichen.Net.Data
{
    public class CommInfo
    {
        /// <summary>
        /// 客户专业组折扣
        /// </summary>
        public static CachInfos<comm_client_group> clientgroup = new CachInfos<comm_client_group> { key = "clientgroup", disname = "" };
        /// <summary>
        /// 客户折扣信息
        /// </summary>
        public static CachInfos<comm_client_info> clientinfo = new CachInfos<comm_client_info> { key = "clientinfo", disname = "" };
        /// <summary>
        /// 专业组信息
        /// </summary>
        public static CachInfos<comm_group_test> grouptest = new CachInfos<comm_group_test> { key = "grouptest", disname = "" };
        /// <summary>
        /// 项目流程
        /// </summary>
        public static CachInfos<comm_item_flow> itemflow = new CachInfos<comm_item_flow> { key = "itemflow", disname = "" };
        /// <summary>
        /// 项目组套
        /// </summary>
        public static CachInfos<comm_item_apply> itemapply = new CachInfos<comm_item_apply> { key = "temapply", disname = "" };
        /// <summary>
        /// 组合项目
        /// </summary>
        public static CachInfos<comm_item_group> itemgroup = new CachInfos<comm_item_group> { key = "itemgroup", disname = "" };
        /// <summary>
        /// 子项
        /// </summary>
        public static CachInfos<comm_item_test> itemtest = new CachInfos<comm_item_test> { key = "itemtest", disname = "" };
        /// <summary>
        /// 项目参考值
        /// </summary>
        public static CachInfos<comm_item_reference> itemreference = new CachInfos<comm_item_reference> { key = "itemreference", disname = "" };
        /// <summary>
        /// 组合项目收费
        /// </summary>
        public static CachInfos<finance_group_charge> financeGroup = new CachInfos<finance_group_charge> { key = "financeGroup", disname = "" };
    }
}
