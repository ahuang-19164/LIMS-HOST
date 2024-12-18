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
using Yichen.Comm.Model.ViewModels.Basics;
using Yichen.Comm.Model.ViewModels.Echarts;
using Yichen.Comm.Model.ViewModels.UI;
namespace Yichen.Net.IRepository
{
    /// <summary>
    ///     报表通用返回 工厂接口
    /// </summary>
    public interface ICoreCmsReportsRepository : IBaseRepository<GetOrdersReportsDbSelectOut>
    {
        /// <summary>
        ///     获取订单销量查询返回结果
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="filter"></param>
        /// <param name="filterSed"></param>
        /// <param name="thesort"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<IPageList<GoodsSalesVolume>> GetGoodsSalesVolumes(string start, string end, string filter,
            string filterSed,
            string thesort, int pageIndex = 1, int pageSize = 5000);


        /// <summary>
        ///     获取商品收藏查询返回结果
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="thesort"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<IPageList<GoodsCollection>> GetGoodsCollections(string start, string end, string thesort,
            int pageIndex = 1, int pageSize = 5000);
    }
}