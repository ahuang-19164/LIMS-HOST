
using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Yichen.QC.Model
{
    /// <summary>
    /// 质控项目
    /// </summary>
    [SugarTable("QC.QCItem", TableDescription = "")]
    public partial class QCItem
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public QCItem()
        {
        }
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.Int32 id  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        
        
        public System.Int32? no  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String workNO  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String groupNO  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String itemNO  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String qcgroupNO  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String planNO  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String levelNO  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String gradeNO  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String batchNO  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        
        
        public float? avgValue  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        
        
        public float? sdValue  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        
        
        public float? cvValue  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        
        
        public System.String remark  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        
        
        public System.Boolean? state  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        
        
        public System.Boolean? dstate  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        
        
        public System.Int32? sort  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        [StringLength(maximumLength:25,ErrorMessage = "{0}不能超过{1}字")]
        
        
        public System.String creater  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        
        
        public System.DateTime? createTime  { get; set; }
        
		
    }
}
