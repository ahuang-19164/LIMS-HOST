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
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Net.Model.Entities;

namespace Yichen.Net.IRepository
{
    /// <summary>
    ///     提货单表 工厂接口
    /// </summary>
    public interface ICoreCmsBillLadingRepository : IBaseRepository<CoreCmsBillLading>
    {
        /// <summary>
        ///     添加提货单
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> AddData(string orderId, int storeId, string name, string mobile);
    }
}