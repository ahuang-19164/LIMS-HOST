
using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Yichen.QC.Model
{
    /// <summary>
    /// 质控评价
    /// </summary>
    [SugarTable("QC.AppraiseRecord", TableDescription = "")]
    public partial class AppraiseRecord
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppraiseRecord()
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





        public System.String planid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]




        public System.String planGradeid { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]





        public System.String planItemid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]


        public System.String planName { get; set; }




        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]


        public System.String planGradeName { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]




        public System.String planItemName { get; set; }

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
		
        
        
        
        
        public System.DateTime? qcStartTime  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        
        
        public System.DateTime? qcEndTime  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:500,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String appraise  { get; set; }
        
		
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
        
        
        public System.String creater  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        
        
        public System.DateTime? createTime  { get; set; }
        
		
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
