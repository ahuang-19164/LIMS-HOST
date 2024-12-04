/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.Yichen.Net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2023-11-16 11:56:59
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
    /// [SugarTable("Common.RoleInfo", TableDescription = "角色信息表")]
    [SugarTable("sw_stores", TableDescription = "存储标本记录")]
    public partial class sw_stores
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public sw_stores()
        {

            saveDay = 7;
            state = true;
            dstate = false;
            createTime= DateTime.Now;
        }
		
        /// <summary>
        /// id
        /// </summary>
        [Display(Name = "id")]
		
        [SugarColumn(IsPrimaryKey = true)]
        
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
        /// 拼音缩写
        /// </summary>
        [Display(Name = "拼音缩写")]
		
        
        
        [StringLength(maximumLength:255,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String shortNames  { get; set; }
        
		
        /// <summary>
        /// 自定编码
        /// </summary>
        [Display(Name = "自定编码")]
		
        
        
        [StringLength(maximumLength:255,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String customCode  { get; set; }
        
		
        /// <summary>
        /// 存储地址
        /// </summary>
        [Display(Name = "存储地址")]
		
        
        
        [StringLength(maximumLength:255,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String address  { get; set; }
        
		
        /// <summary>
        /// 储存天数
        /// </summary>
        [Display(Name = "储存天数")]
		
        
        
        
        
        public System.Int32? saveDay  { get; set; }
        
		
        /// <summary>
        /// 默认行数
        /// </summary>
        [Display(Name = "默认行数")]
		
        
        
        
        
        public System.Int32? shoresRow  { get; set; }
        
		
        /// <summary>
        /// 默认列数
        /// </summary>
        [Display(Name = "默认列数")]
		
        
        
        
        
        public System.Int32? shoresCell  { get; set; }
        
		
        /// <summary>
        /// 存储样本类型
        /// </summary>
        [Display(Name = "存储样本类型")]
		
        
        
        [StringLength(maximumLength:255,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String sampleType  { get; set; }
        
		
        /// <summary>
        /// 备注信息
        /// </summary>
        [Display(Name = "备注信息")]
		
        
        
        [StringLength(maximumLength:255,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String remark  { get; set; }
        
		
        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
		
        
        
        
        
        public System.Int32? sore  { get; set; }
        
		
        /// <summary>
        /// 创建人
        /// </summary>
        [Display(Name = "创建人")]
		
        
        
        [StringLength(maximumLength:255,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String creater  { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]


        public System.DateTime createTime { get; set; } = DateTime.Now;




        /// <summary>
        /// 是否启用
        /// </summary>
        [Display(Name = "是否启用")]
		
        
        
        
        
        public System.Boolean? state  { get; set; }
        
		
        /// <summary>
        /// 是否删除
        /// </summary>
        [Display(Name = "是否删除")]
		
        
        
        
        
        public System.Boolean? dstate  { get; set; }
        
		
    }
}
