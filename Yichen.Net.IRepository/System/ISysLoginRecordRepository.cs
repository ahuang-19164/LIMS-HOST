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
using Yichen.Net.Model.Entities;
namespace Yichen.Net.IRepository
{
    /// <summary>
    ///     登录日志表 工厂接口
    /// </summary>
    public interface ISysLoginRecordRepository : IBaseRepository<SysLoginRecord>
    {
    }
}