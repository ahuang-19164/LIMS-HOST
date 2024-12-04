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
    ///     用户地址表
    /// </summary>
    public partial class CoreCmsUserShip
    {
        /// <summary>
        ///     区域名称
        /// </summary>
        [Display(Name = "区域名称")]
        [SugarColumn(IsIgnore = true)]

        public string? areaName { get; set; }

    }
}