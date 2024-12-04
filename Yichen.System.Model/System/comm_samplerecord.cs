using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Yichen.System.Model
{
    /// <summary>
    /// 样本操作记录
    /// </summary>
    //[SugarTable("WorkComm.comm_samplerecord", TableDescription = "检验操作记录")]
    [SugarTable("WorkComm.SampleRecord", TableDescription = "检验操作记录")]
    public class comm_samplerecord
    {
        public comm_samplerecord()
        {


        }
        /// <summary>
        /// id
        /// </summary>
        [Display(Name = "id")]
        [SugarColumn(ColumnDescription = "id", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        /// Desc:申请ID
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? perid { get; set; }

        /// <summary>
        /// Desc:检验ID
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? testid { get; set; }

        /// <summary>
        /// Desc:条码号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? barcode { get; set; }

        /// <summary>
        /// Desc:操作类型
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? operatType { get; set; }

        /// <summary>
        /// Desc:记录内容
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? record { get; set; }

        /// <summary>
        /// Desc:原因
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? reason { get; set; }

        /// <summary>
        /// Desc:操作人
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? operater { get; set; }

        /// <summary>
        /// Desc:创建时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? createTime { get; set; }

        /// <summary>
        /// Desc:是否展示给客户
        /// Default:
        /// Nullable:True
        /// </summary>           
        public bool? clientShow { get; set; }
    }
}
