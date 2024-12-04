/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.Yichen.Net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2023-11-22 1:07:52
 *        Description: 暂无
 ***********************************************************************/

using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Yichen.Stores.Model
{
    /// <summary>
    /// 标本架信息
    /// </summary>
    [SugarTable("sw_shelf", TableDescription = "标本架信息")]
    public partial class sw_shelf
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public sw_shelf()
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
        /// 编号
        /// </summary>
        [Display(Name = "编号")]
		
        
        
        [StringLength(maximumLength:255,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String no  { get; set; }
        
		
        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "名称")]
		
        
        
        [StringLength(maximumLength:255,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String names  { get; set; }
        
		
        /// <summary>
        /// 拼英简称
        /// </summary>
        [Display(Name = "拼英简称")]
		
        
        
        [StringLength(maximumLength:255,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String shortNames  { get; set; }
        
		
        /// <summary>
        /// 自定编码
        /// </summary>
        [Display(Name = "自定编码")]
		
        
        
        [StringLength(maximumLength:255,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String customCode  { get; set; }
        
		
        /// <summary>
        /// 存储编号
        /// </summary>
        [Display(Name = "存储编号")]
		
        
        
        [StringLength(maximumLength:255,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String storesNO  { get; set; }
        
		
        /// <summary>
        /// 储存位置编号
        /// </summary>
        [Display(Name = "储存位置编号")]
		
        
        
        [StringLength(maximumLength:255,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String saveNo  { get; set; }
        
		
        /// <summary>
        /// 保存天数
        /// </summary>
        [Display(Name = "保存天数")]
		
        
        
        [StringLength(maximumLength:255,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String saveDay  { get; set; }
        
		
        /// <summary>
        /// 标本架行
        /// </summary>
        [Display(Name = "标本架行")]
		
        
        
        [StringLength(maximumLength:255,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String shelfRow  { get; set; }
        
		
        /// <summary>
        /// 标本架列
        /// </summary>
        [Display(Name = "标本架列")]
		
        
        
        [StringLength(maximumLength:255,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String shelfCell  { get; set; }
        
		
        /// <summary>
        /// 储存状态//1.正常 2.存在过期 3.到存储时间 4.已处理 5.异常
        /// </summary>
        [Display(Name = "储存状态//1.正常 2.存在过期 3.到存储时间 4.已处理 5.异常")]
		
        
        
        
        
        public System.Int32? shelfTypeNO  { get; set; }
        
		
        /// <summary>
        /// 是否启用
        /// </summary>
        [Display(Name = "是否启用")]
		
        
        
        
        
        public System.Boolean? state  { get; set; }
        
		
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
		
        
        
        [StringLength(maximumLength:255,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String remark  { get; set; }
        
		
        /// <summary>
        /// 样本数量
        /// </summary>
        [Display(Name = "样本数量")]
		
        
        
        
        
        public System.Int32? sampleCount  { get; set; }
        
		
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
		
        
        
        
        
        public System.DateTime? createTime  { get; set; }
        
		
        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
		
        
        
        
        
        public System.Int32? sort  { get; set; }
        
		
    }
}
