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
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Comm.IServices;
using Yichen.Comm.IServices;

namespace Yichen.Net.IServices
{
    /// <summary>
    ///     用户地址表 服务工厂接口
    /// </summary>
    public interface ICoreCmsUserShipServices : IBaseServices<CoreCmsUserShip>
    {
        /// <summary>
        ///     事务重写异步插入方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<WebApiCallBack> InsertAsync(CoreCmsUserShip entity);


        /// <summary>
        ///     重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> UpdateAsync(CoreCmsUserShip entity);
    }
}