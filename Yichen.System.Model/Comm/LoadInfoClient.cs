
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Yichen.System.Model
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("Common.LoadInfoClient", TableDescription = "")]
    public partial class LoadInfoClient
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public LoadInfoClient()
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
		
        
        
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超过{1}字")]
        
        public String no  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超过{1}字")]
        
        public String names  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:500,ErrorMessage = "{0}不能超过{1}字")]
        
        public String token  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        
        
        public DateTime? createTime  { get; set; }
        
		
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        
        
        public DateTime? endTime  { get; set; }
        
		
    }
}
