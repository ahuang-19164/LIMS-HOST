/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Yichen.Net.Model.Entities
{
    /// <summary>
    ///     服务消费券
    /// </summary>
    public partial class CoreCmsUserServicesTicket
    {
        /// <summary>
        ///     状态说明
        /// </summary>
        [Display(Name = "状态说明")]
        [SugarColumn(IsIgnore = true)]

        public string? statusStr { get; set; }

    }
}