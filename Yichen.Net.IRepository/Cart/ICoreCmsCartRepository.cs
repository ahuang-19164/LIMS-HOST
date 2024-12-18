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
using Yichen.Comm.IRepository;
using Yichen.Net.Model.Entities;

namespace Yichen.Net.IRepository
{
    /// <summary>
    ///     购物车表 工厂接口
    /// </summary>
    public interface ICoreCmsCartRepository : IBaseRepository<CoreCmsCart>
    {
        /// <summary>
        ///     获取购物车用户数据总数
        /// </summary>
        /// <returns></returns>
        Task<int> GetCountAsync(int userId);
    }
}