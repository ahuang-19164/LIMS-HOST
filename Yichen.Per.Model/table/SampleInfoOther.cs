
using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Yichen.Per.Model.table
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("WorkPer.SampleInfoOther", TableDescription = "")]
    public partial class SampleInfoOther
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SampleInfoOther()
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
        [Required(ErrorMessage = "请输入{0}")]



        public System.Int32 perid { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
		
        
        
        [StringLength(maximumLength:500,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String barcode  { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
        [Required(ErrorMessage = "请输入{0}")]



        public System.Boolean state { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
        [Required(ErrorMessage = "请输入{0}")]



        public System.DateTime createTime { get; set; }



    }
}
