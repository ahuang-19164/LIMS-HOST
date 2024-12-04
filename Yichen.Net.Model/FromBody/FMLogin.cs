/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/


namespace Yichen.Net.Model.FromBody
{
    /// <summary>
    ///     用户登录验证实体
    /// </summary>
    public class FMLogin
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        public string? userNo { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string? password { get; set; }

    }


    /// <summary>
    ///     用户登录验证实体
    /// </summary>
    public class FMEditLoginUserPassWord
    {

        public string? oldPassword { get; set; }


        public string? password { get; set; }


        public string? repassword { get; set; }

    }
}