/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using Yichen.Comm.IRepository;
using Yichen.System.Model;
using Yichen.System.Model.Entities;

namespace Yichen.System.IRepository
{
    /// <summary>
    ///     用户日志 工厂接口
    /// </summary>
    public interface IUserLogRepository : IBaseRepository<sys_user_log>
    {
    }
}