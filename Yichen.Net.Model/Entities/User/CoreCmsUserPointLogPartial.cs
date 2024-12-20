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
    ///     用户积分
    /// </summary>
    public partial class CoreCmsUserPointLog
    {
        /// <summary>
        ///     类型说明
        /// </summary>
        [Display(Name = "类型说明")]
        [SugarColumn(IsIgnore = true)]

        public string? typeName { get; set; }

    }
}