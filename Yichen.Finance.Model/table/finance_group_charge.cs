using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Yichen.Finance.Model.table
{

    ///<summary>
    ///组合项目备案收费价格表
    ///</summary>
    [SugarTable("Finance.GroupChargeInfo", TableDescription = "组合项目备案收费价格")]
    public partial class finance_group_charge
    {
        public finance_group_charge()
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
        /// Desc:代理商编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? agentNO { get; set; }

        /// <summary>
        /// Desc:客户编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? hospitalNO { get; set; }

        /// <summary>
        /// Desc:人员类型
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? patientTypeNO { get; set; }

        /// <summary>
        /// Desc:科室
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? department { get; set; }

        /// <summary>
        /// Desc:组合项目编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? groupCode { get; set; }

        /// <summary>
        /// Desc:收费等级
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? chargeLevelNO { get; set; }

        /// <summary>
        /// Desc:标准价格
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? standardCharge { get; set; }

        /// <summary>
        /// Desc:收费价格
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? settlementCharge { get; set; }

        /// <summary>
        /// Desc:是否参与折扣
        /// Default:
        /// Nullable:True
        /// </summary>           
        public bool? discountState { get; set; }

        /// <summary>
        /// Desc:开始时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? startTime { get; set; }

        /// <summary>
        /// Desc:结束时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? endTime { get; set; }

        /// <summary>
        /// Desc:创建人
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? creater { get; set; }

        /// <summary>
        /// Desc:创建时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? createTime { get; set; }

        /// <summary>
        /// Desc:是否启用
        /// Default:
        /// Nullable:True
        /// </summary>           
        public bool? state { get; set; }

    }
}
