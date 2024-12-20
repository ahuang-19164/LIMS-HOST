/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.Threading.Tasks;
using Yichen.Net.Model.Entities;
using Yichen.Comm.IServices;

namespace Yichen.Net.IServices
{
    /// <summary>
    ///     用户表 服务工厂接口
    /// </summary>
    public interface ISysUserServices : IBaseServices<SysUser>
    {
        Task<string> GetUserRoleNameStr(string loginName, string loginPwd);
    }
}