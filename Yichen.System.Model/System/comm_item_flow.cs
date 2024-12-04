using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Yichen.System.Model
{
    /////<summary>
    /////检验项目流程定义
    ///// </summary>
    ////[SugarTable("WorkComm.comm_item_flow", TableDescription = "检验项目流程定义")]
    //[SugarTable("WorkComm.ItemFlow", TableDescription = "检验项目流程定义")]
    //public partial class comm_item_flow
    //{

    //    public comm_item_flow()
    //    {

    //        nextFlow = Convert.ToString("0");
    //        state = true;
    //        dstate = false;
    //        frmState = true;

    //    }
    //    /// <summary>
    //    /// id
    //    /// </summary>
    //    [Display(Name = "id")]
    //    [SugarColumn(ColumnDescription = "id", IsPrimaryKey = true, IsIdentity = true)]
    //    [Required(ErrorMessage = "请输入{0}")]
    //    public int id { get; set; }

    //    /// <summary>
    //    /// Desc:
    //    /// Default:
    //    /// Nullable:True
    //    /// </summary>           
    //    public int no { get; set; }

    //    /// <summary>
    //    /// Desc:
    //    /// Default:
    //    /// Nullable:True
    //    /// </summary>           
    //    public string? names { get; set; }

    //    /// <summary>
    //    /// Desc:
    //    /// Default:
    //    /// Nullable:True
    //    /// </summary>           
    //    public string? shortNames { get; set; }

    //    /// <summary>
    //    /// Desc:
    //    /// Default:
    //    /// Nullable:True
    //    /// </summary>           
    //    public string? customCode { get; set; }

    //    /// <summary>
    //    /// Desc:
    //    /// Default:
    //    /// Nullable:True
    //    /// </summary>           
    //    public string? reflectionFile { get; set; }

    //    /// <summary>
    //    /// Desc:
    //    /// Default:
    //    /// Nullable:True
    //    /// </summary>           
    //    public string? reflectionFrm { get; set; }

    //    /// <summary>
    //    /// Desc:
    //    /// Default:
    //    /// Nullable:True
    //    /// </summary>           
    //    public string? dataSource { get; set; }

    //    /// <summary>
    //    /// Desc:
    //    /// Default:
    //    /// Nullable:True
    //    /// </summary>           
    //    public string? imgSource { get; set; }

    //    /// <summary>
    //    /// Desc:
    //    /// Default:0
    //    /// Nullable:True
    //    /// </summary>           
    //    public string? nextFlow { get; set; }

    //    /// <summary>
    //    /// Desc:
    //    /// Default:
    //    /// Nullable:True
    //    /// </summary>           
    //    public int sort { get; set; }

    //    /// <summary>
    //    /// Desc:
    //    /// Default:1
    //    /// Nullable:True
    //    /// </summary>           
    //    public bool? state { get; set; }

    //    /// <summary>
    //    /// Desc:
    //    /// Default:0
    //    /// Nullable:True
    //    /// </summary>           
    //    public bool? dstate { get; set; } = false;

    //    /// <summary>
    //    /// Desc:
    //    /// Default:
    //    /// Nullable:True
    //    /// </summary>           
    //    public string? frmFile { get; set; }

    //    /// <summary>
    //    /// Desc:
    //    /// Default:
    //    /// Nullable:True
    //    /// </summary>           
    //    public string? frmData { get; set; }

    //    /// <summary>
    //    /// Desc:
    //    /// Default:1
    //    /// Nullable:True
    //    /// </summary>           
    //    public bool? frmState { get; set; }

    //}
}
