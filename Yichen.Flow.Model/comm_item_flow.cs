using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Yichen.Flow.Model
{
    ///<summary>
    ///项目流程表
    ///</summary>
    [SugarTable("WorkComm.ItemFlow", TableDescription = "项目流程表")]
    public partial class comm_item_flow
    {
        public comm_item_flow()
        {
            state = true;
            dstate = false;
            frmState = true;
            nextFlow = "0";
        }
        /// <summary>
        /// id
        /// </summary>
        [Display(Name = "id")]
        [SugarColumn(ColumnDescription = "id", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        /// Desc:编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? no { get; set; }

        /// <summary>
        /// Desc:名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string names { get; set; }

        /// <summary>
        /// Desc:简称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string shortNames { get; set; }

        /// <summary>
        /// Desc:自定义码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string customCode { get; set; }

        /// <summary>
        /// Desc:窗体文件
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string reflectionFile { get; set; }

        /// <summary>
        /// Desc:窗体方法
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string reflectionFrm { get; set; }

        /// <summary>
        /// Desc:数据源
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string dataSource { get; set; }

        /// <summary>
        /// Desc:图片源
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string imgSource { get; set; }

        /// <summary>
        /// Desc:下一节点
        /// Default:0
        /// Nullable:True
        /// </summary>           
        public string nextFlow { get; set; }

        /// <summary>
        /// Desc:排序
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? sort { get; set; }

        /// <summary>
        /// Desc:启用
        /// Default:1
        /// Nullable:True
        /// </summary>           
        public bool? state { get; set; }

        /// <summary>
        /// Desc:删除
        /// Default:0
        /// Nullable:True
        /// </summary>           
        public bool? dstate { get; set; }

        /// <summary>
        /// Desc:创建文件地址
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string frmFile { get; set; }

        /// <summary>
        /// Desc:窗体参数
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string frmData { get; set; }

        /// <summary>
        /// Desc:窗体是否启用
        /// Default:1
        /// Nullable:True
        /// </summary>           
        public bool? frmState { get; set; }

    }
}
