﻿using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Yichen.System.Model
{
    ///<summary>
    ///组套项目信息
    /// </summary>
    //[SugarTable("WorkComm.comm_item_apply", TableDescription = "组套项目信息")]
    [SugarTable("WorkComm.ItemApply", TableDescription = "组套项目信息")]
    public partial class comm_item_apply
    {
        public comm_item_apply()
        {

            wxstate = true;
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
        public int no { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? names { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? disNames { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? shortNames { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? customCode { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? customNames { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? companyNO { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? clientNO { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? groupItemList { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int sort { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? remark { get; set; }

        /// <summary>
        /// Desc:
        /// Default:1
        /// Nullable:True
        /// </summary>           
        public bool? wxstate { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public bool? state { get; set; }

        /// <summary>
        /// Desc:
        /// Default:0
        /// Nullable:True
        /// </summary>           
        public bool? dstate { get; set; } = false;
    }
}
