
using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Yichen.QC.Model
{
    /// <summary>
    /// 质控处理
    /// </summary>
    [SugarTable("QC.HandleRecord", TableDescription = "")]
    public partial class HandleRecord
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public HandleRecord()
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
		
        
        
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String planid  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String planGradeid  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String planItemid  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:200,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String planName  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:200,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String planGradeName  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:200,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String planItemName  { get; set; }
        
		
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
        
        public System.String resultid  { get; set; }
        
		
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
        
        public System.String levelName  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        
        
        public System.DateTime? qcTime  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String qcSort  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:500,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String gradeNo  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:500,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String result  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:500,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String resultRule  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:500,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String resultReason  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:500,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String handleRecord  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:500,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String remark  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        [StringLength(maximumLength:25,ErrorMessage = "{0}不能超过{1}字")]
        
        
        public System.String handler  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:500,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String handleTime  { get; set; }
        
		
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
        
		
    }
}
