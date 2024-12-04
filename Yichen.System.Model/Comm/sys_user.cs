﻿using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Yichen.System.Model
{
    /// <summary>
    /// 用户表
    /// </summary>
    //[SugarTable("WorkComm.sys_userinfo", TableDescription = "用户表")]
    [SugarTable("Common.UserInfo", TableDescription = "用户表")]
    public class sys_user
    {
        public sys_user()
        {


        }
        /// <summary>
        /// Desc:用户ID
        /// Default:
        /// Nullable:False
        /// </summary>  
        [Display(Name = "用户ID")]
        [SugarColumn(ColumnDescription = "用户ID", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        /// Desc:用户编号
        /// Default:
        /// Nullable:True
        /// </summary>   
        [Display(Name = "用户编号")]
        [SugarColumn(ColumnDescription = "用户编号", IsNullable = false)]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public int no { get; set; }

        /// <summary>
        /// Desc:账号
        /// Default:
        /// Nullable:True
        /// </summary>    
        [Display(Name = "账号")]
        [SugarColumn(ColumnDescription = "账号", IsNullable = false)]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string userNO { get; set; }

        /// <summary>
        /// Desc:用户名称
        /// Default:
        /// Nullable:True
        /// </summary>   
        [Display(Name = "用户名称")]
        [SugarColumn(ColumnDescription = "用户名称", IsNullable = false)]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string names { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>   
        [Display(Name = "用户简称")]
        [SugarColumn(ColumnDescription = "用户简称", IsNullable = true)]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string? shortNames { get; set; }

        /// <summary>
        /// Desc:自定义码
        /// Default:
        /// Nullable:True
        /// </summary>     
        [Display(Name = "自定义码")]
        [SugarColumn(ColumnDescription = "自定义码", IsNullable = true)]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string? customCode { get; set; }

        /// <summary>
        /// Desc:公司编号
        /// Default:
        /// Nullable:True
        /// </summary>  
        [Display(Name = "公司编号")]
        [SugarColumn(ColumnDescription = "公司编号", IsNullable = false)]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string? companyNO { get; set; }

        /// <summary>
        /// Desc:部门编号
        /// Default:
        /// Nullable:True
        /// </summary>  
        [Display(Name = "部门编号")]
        [SugarColumn(ColumnDescription = "部门编号", IsNullable = false)]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string? departmentNO { get; set; }

        /// <summary>
        /// Desc:角色编号
        /// Default:
        /// Nullable:True
        /// </summary>   
        [Display(Name = "角色编号")]
        [SugarColumn(ColumnDescription = "角色编号", IsNullable = false)]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public int roleNO { get; set; }

        /// <summary>
        /// Desc:Web角色编号
        /// Default:
        /// Nullable:True
        /// </summary>   
        [Display(Name = "Web角色编号")]
        [SugarColumn(ColumnDescription = "Web角色编号", IsNullable = false)]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public int webRoleNO { get; set; }

        /// <summary>
        /// Desc:排序
        /// Default:
        /// Nullable:True
        /// </summary>   
        [Display(Name = "排序")]
        [SugarColumn(ColumnDescription = "排序", IsNullable = false)]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string? sort { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>   
        [Display(Name = "密码")]
        [SugarColumn(ColumnDescription = "密码", IsNullable = false)]
        [PasswordPropertyText()]
        public string? pwd { get; set; }

        /// <summary>
        /// Desc:性别
        /// Default:
        /// Nullable:True
        /// </summary>   
        [Display(Name = "性别")]
        [SugarColumn(ColumnDescription = "性别[1男2女3未知]", IsNullable = false)]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string? sex { get; set; }

        /// <summary>
        /// Desc:生日
        /// Default:
        /// Nullable:True
        /// </summary>     
        [Display(Name = "生日")]
        [SugarColumn(ColumnDescription = "生日")]
        [Required(ErrorMessage = "请输入{0}")]
        public DateTime? birthday { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? phone { get; set; }

        /// <summary>
        /// Desc:邮箱
        /// Default:
        /// Nullable:True
        /// </summary>   
        [Display(Name = "邮箱")]
        [SugarColumn(ColumnDescription = "邮箱")]
        [EmailAddress(ErrorMessage = "请输入{0}")]
        public string? email { get; set; }

        /// <summary>
        /// Desc:工作电话
        /// Default:
        /// Nullable:True
        /// </summary>   
        [Display(Name = "工作电话")]
        [SugarColumn(ColumnDescription = "工作电话")]
        [Phone(ErrorMessage = "请输入{0}")]
        public string? workPhone { get; set; }

        /// <summary>
        /// Desc:微信
        /// Default:
        /// Nullable:True
        /// </summary>  
        [Display(Name = "微信")]
        [SugarColumn(ColumnDescription = "微信")]
        //[StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string? weChat { get; set; }

        /// <summary>
        /// Desc:客户列表
        /// Default:
        /// Nullable:True
        /// </summary>           
        [Display(Name = "客户列表")]
        [SugarColumn(ColumnDescription = "客户列表")]
        public string? clientList { get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:
        /// Nullable:True
        /// </summary>  
        [Display(Name = "备注")]
        [SugarColumn(ColumnDescription = "备注")]
        public string? remark { get; set; }

        /// <summary>
        /// Desc:账号状态 0 禁用 1 启用
        /// Default:
        /// Nullable:True
        /// </summary>    
        [Display(Name = "账号状态")]
        [SugarColumn(ColumnDescription = "账号状态")]
        public bool state { get; set; }

        /// <summary>
        /// Desc:Web状态
        /// Default:
        /// Nullable:True
        /// </summary>  
        [Display(Name = "Web状态")]
        [SugarColumn(ColumnDescription = "Web状态")]
        public bool webstate { get; set; }

        /// <summary>
        /// Desc:删除状态
        /// Default:
        /// Nullable:True
        /// </summary>  
        [Display(Name = "删除状态")]
        [SugarColumn(ColumnDescription = "删除状态")]
        public bool? dstate { get; set; } = false;

        /// <summary>
        /// Desc:用户图片
        /// Default:
        /// Nullable:True
        /// </summary>  
        [Display(Name = "用户图片")]
        [SugarColumn(ColumnDescription = "用户图片")]
        public string? headImageUrl { get; set; }

        /// <summary>
        /// Desc:Token
        /// Default:
        /// Nullable:True
        /// </summary>  
        [Display(Name = "Token")]
        [SugarColumn(ColumnDescription = "Token")]
        public string? token { get; set; }

        /// <summary>
        /// Desc:最后登录时间
        /// Default:
        /// Nullable:True
        /// </summary>   
        [Display(Name = "最后登录时间")]
        [SugarColumn(ColumnDescription = "最后登录时间")]
        public DateTime? lastModifyPwdDate { get; set; }

        /// <summary>
        /// Desc:创建时间
        /// Default:
        /// Nullable:True
        /// </summary>   
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间")]
        public DateTime createDate { get; set; }
    }
}
