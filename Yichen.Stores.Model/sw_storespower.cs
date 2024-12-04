
using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Yichen.Stores.Model
{
    /// <summary>
    /// 存储库权限列表
    /// </summary>
    [SugarTable("sw_storespower", TableDescription = "存储库权限列表")]
    public partial class sw_storespower
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public sw_storespower()
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
        /// 用户编号
        /// </summary>
        [Display(Name = "用户编号")]
		
        
        
        [StringLength(maximumLength:255,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String userNo  { get; set; }
        
		
        /// <summary>
        /// 存储库id
        /// </summary>
        [Display(Name = "存储库id")]
		
        
        
        
        
        public System.Int32? storesid  { get; set; }
        
		
        /// <summary>
        /// 状态
        /// </summary>
        [Display(Name = "状态")]
		
        
        
        
        
        public System.Boolean? state  { get; set; }
        
		
        /// <summary>
        /// 创建标本架
        /// </summary>
        [Display(Name = "创建标本架")]
		
        
        
        
        
        public System.Boolean? createShelf  { get; set; }
        
		
        /// <summary>
        /// 编辑标本架
        /// </summary>
        [Display(Name = "编辑标本架")]
		
        
        
        
        
        public System.Boolean? editShelf  { get; set; }
        
		
        /// <summary>
        /// 录入标本
        /// </summary>
        [Display(Name = "录入标本")]
		
        
        
        
        
        public System.Boolean? entrySample  { get; set; }
        
		
        /// <summary>
        /// 修改标本
        /// </summary>
        [Display(Name = "修改标本")]
		
        
        
        
        
        public System.Boolean? editSample  { get; set; }
        
		
        /// <summary>
        /// 删除标本
        /// </summary>
        [Display(Name = "删除标本")]
		
        
        
        
        
        public System.Boolean? delsample  { get; set; }
        
		
        /// <summary>
        /// 处理标本
        /// </summary>
        [Display(Name = "处理标本")]
		
        
        
        
        
        public System.Boolean? handleSample  { get; set; }
        
		
        /// <summary>
        /// 反处理标本
        /// </summary>
        [Display(Name = "反处理标本")]
		
        
        
        
        
        public System.Boolean? rehandleSample  { get; set; }
        
		
        /// <summary>
        /// 查询标本
        /// </summary>
        [Display(Name = "查询标本")]
		
        
        
        
        
        public System.Boolean? searchSample  { get; set; }
        
		
        /// <summary>
        /// 取消录入
        /// </summary>
        [Display(Name = "取消录入")]
		
        
        
        
        
        public System.Boolean? cancelSample  { get; set; }
        
		
    }
}
