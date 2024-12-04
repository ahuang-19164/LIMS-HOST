/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.Collections.Generic;
using System.Security.Claims;

namespace Yichen.Net.Auth.HttpContextUser
{
    /// <summary>
    /// 读取context中的用户信息
    /// </summary>
    public interface IHttpContextUser
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 用户id
        /// </summary>
        int ID { get; }
        /// <summary>
        /// 用户账号
        /// </summary>

        string UserNo { get; }

        /// <summary>
        /// 用户角色
        /// </summary>
        int Role { get; }
        /// <summary>
        /// 用户web角色
        /// </summary>

        int WebRole { get; }
        /// <summary>
        /// 权限是否有效
        /// </summary>
        /// <returns></returns>
        bool IsAuthenticated();
        /// <summary>
        /// 附加信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<Claim> GetClaimsIdentity();
        /// <summary>
        /// 获取Claims中的指定信息
        /// </summary>
        /// <param name="ClaimType"></param>
        /// <returns></returns>
        List<string> GetClaimValueByTypes(string ClaimType);

        /// <summary>
        /// 获取Claims中的指定信息
        /// </summary>
        /// <param name="ClaimType"></param>
        /// <returns></returns>
        string GetClaimValueByType(string ClaimType);
        /// <summary>
        /// 获取用户Token
        /// </summary>
        /// <returns></returns>
        string GetToken();
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="ClaimType"></param>
        /// <returns></returns>
        List<string> GetUserInfoFromTokens(string ClaimType);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ClaimType"></param>
        /// <returns></returns>
        string GetUserInfoFromToken(string ClaimType);
    }
}
