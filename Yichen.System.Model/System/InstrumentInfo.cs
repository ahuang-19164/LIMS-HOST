
using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Yichen.System.Model
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("WorkComm.InstrumentInfo", TableDescription = "")]
    public partial class InstrumentInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public InstrumentInfo()
        {
        }
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public int id  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        
        
        public int? no  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:500,ErrorMessage = "{0}不能超过{1}字")]
        
        public string? names  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
        [StringLength(maximumLength:100,ErrorMessage = "{0}不能超过{1}字")]
        public string? shortNames  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:100,ErrorMessage = "{0}不能超过{1}字")]
        
        public string customCode  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超过{1}字")]
        
        public String groupNO  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:100,ErrorMessage = "{0}不能超过{1}字")]
        
        public String remark  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
        public int? sort  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]

        public Boolean? state  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        
        
        public Boolean? dstate  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        
        
        public DateTime? createTime  { get; set; }
        
		
    }
}
