
using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Yichen.System.Model
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("WorkComm.MicrobeGroupList", TableDescription = "")]
    public partial class MicrobeGroupList
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MicrobeGroupList()
        {
        }
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public Int32 id  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        
        
        public Int32? no  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:100,ErrorMessage = "{0}不能超过{1}字")]
        
        public String names  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:100,ErrorMessage = "{0}不能超过{1}字")]
        
        public String shortNames  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:100,ErrorMessage = "{0}不能超过{1}字")]
        
        public String customCode  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:100,ErrorMessage = "{0}不能超过{1}字")]
        
        public String groupCode  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        
        
        public String listinfo  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        
        
        public String remark  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        
        
        public Int32? sort  { get; set; }
        
		
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
        
		
    }
}
