/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.Yichen.Net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2023-11-23 14:45:44
 *        Description: 暂无
 ***********************************************************************/

using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Yichen.Stores.Model
{
    /// <summary>
    /// 存储标本记录
    /// </summary>
    [SugarTable("sw_record", TableDescription = "存储标本记录")]
    public partial class sw_record
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public sw_record()
        {
        }
		
        /// <summary>
        /// id
        /// </summary>
        [Display(Name = "id")]
		
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.Int32 id  { get; set; }
        
		
        /// <summary>
        /// 条码号
        /// </summary>
        [Display(Name = "条码号")]
		
        
        
        [StringLength(maximumLength:255,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String barcode  { get; set; }
        
		
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
		
        
        
        
        
        public System.DateTime? createTime  { get; set; }
        
		
        /// <summary>
        /// 过期时间
        /// </summary>
        [Display(Name = "过期时间")]
		
        
        
        
        
        public System.DateTime? outTime  { get; set; }
        
		
        /// <summary>
        /// 存储时间
        /// </summary>
        [Display(Name = "存储时间")]
		
        
        
        [StringLength(maximumLength:255,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String saveDay  { get; set; }
        
		
        /// <summary>
        /// 存储库编号
        /// </summary>
        [Display(Name = "存储库编号")]
		
        
        
        [StringLength(maximumLength:255,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String storesNO  { get; set; }
        
		
        /// <summary>
        /// 标本架编号
        /// </summary>
        [Display(Name = "标本架编号")]
		
        
        
        [StringLength(maximumLength:255,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String shelfNO  { get; set; }
        
		
        /// <summary>
        /// 标本架位置编号
        /// </summary>
        [Display(Name = "标本架位置编号")]
		
        
        
        [StringLength(maximumLength:255,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String saveNO  { get; set; }
        
		
        /// <summary>
        /// 是否有效
        /// </summary>
        [Display(Name = "是否自动更新(0未更新1已更新2用户更新)")]
        public System.Int32? state  { get; set; }
        
		
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
		
        
        
        [StringLength(maximumLength:255,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String remark  { get; set; }
        
		
        /// <summary>
        /// 标本状态(1正常2已处理3已过期4其他)
        /// </summary>
        [Display(Name = "标本状态(1正常2已处理3已过期4其他)")]
		
        
        
        
        
        public System.Int32? recordTypeNO  { get; set; }
        
		
    }
}
