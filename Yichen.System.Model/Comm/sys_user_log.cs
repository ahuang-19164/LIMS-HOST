/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com
 *         CreateTime: 2021-06-08 22:14:59
 *        Description: 暂无
***********************************************************************/
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Yichen.System.Model
{
    /// <summary>
    /// 用户日志
    /// </summary>
    //[SugarTable("WorkComm.sys_userlog", TableDescription = "用户日志")]
    [SugarTable("sys_userlog", TableDescription = "用户日志")]
    public partial class sys_user_log
    {
        /// <summary>
        /// 用户日志
        /// </summary>
        public sys_user_log()
        {
        }

        /// <summary>
        /// id
        /// </summary>
        [Display(Name = "id")]
        [SugarColumn(ColumnDescription = "id", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        [Display(Name = "用户id")]
        [SugarColumn(ColumnDescription = "用户id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int userId { get; set; }


        ///// <summary>
        ///// 用户账号
        ///// </summary>
        //[Display(Name = "用户账号")]
        //[SugarColumn(ColumnDescription = "用户账号")]
        //[Required(ErrorMessage = "请输入{0}")]
        //public int userNo { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        [Display(Name = "用户名称")]
        [SugarColumn(ColumnDescription = "用户名称")]
        [Required(ErrorMessage = "请输入{0}")]
        public string? names { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [Display(Name = "状态")]
        [SugarColumn(ColumnDescription = "状态", IsNullable = true)]
        public int state { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        [Display(Name = "参数")]
        [SugarColumn(ColumnDescription = "参数", IsNullable = true)]
        [StringLength(200, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string? parameters { get; set; }
        /// <summary>
        /// ip地址
        /// </summary>
        [Display(Name = "ip地址")]
        [SugarColumn(ColumnDescription = "ip地址", IsNullable = true)]
        [StringLength(15, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string? ip { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间", IsNullable = true)]
        public DateTime? createTime { get; set; }
    }
}