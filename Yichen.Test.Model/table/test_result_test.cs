using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Yichen.Test.Model.table
{
    /// <summary>
    /// 常规检验结果表
    /// </summary>
    //[SugarTable("test_result_item", TableDescription = "检验信息")]
    [SugarTable("WorkTest.SampleResult", TableDescription = "常规检验结果表")]
    public partial class test_result_item
    {
        public test_result_item()
        {

            delstate = false;
            reportState = true;
            resultNullState = true;
            delegateState = false;
            state = true;
            dstate = false;

        }
        /// <summary>
        /// id
        /// </summary>
        [Display(Name = "id")]
        [SugarColumn(ColumnDescription = "id", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? perid { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? testid { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? barcode { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? groupNO { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? groupCode { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? groupName { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? itemCodes { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? itemNames { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? itemEN { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? itemResult { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? flag { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? unit { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? itemResultLog { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? calculationFormula { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int itemSort { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? instrumentNO { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? channel { get; set; }

        /// <summary>
        /// Desc:精度
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int precision { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? methodNO { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? methodName { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? ReferenceDown { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? ReferenceUp { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? crisisDown { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? crisisUp { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? valueDescribe { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? testTypeNO { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? testFlowNO { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? resultTypeNO { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? delegeteCompanyNO { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? creater { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? createTime { get; set; }

        /// <summary>
        /// Desc:
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public bool delstate { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? delstateClientNO { get; set; }

        /// <summary>
        /// Desc:
        /// Default:1
        /// Nullable:False
        /// </summary>           
        public bool reportState { get; set; }

        /// <summary>
        /// Desc:
        /// Default:1
        /// Nullable:False
        /// </summary>           
        public bool resultNullState { get; set; }

        /// <summary>
        /// Desc:
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public bool delegateState { get; set; }

        /// <summary>
        /// Desc:
        /// Default:1
        /// Nullable:False
        /// </summary>           
        public bool state { get; set; }

        /// <summary>
        /// Desc:
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public bool dstate { get; set; }

    }
}
