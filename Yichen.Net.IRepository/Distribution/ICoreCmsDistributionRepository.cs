using System.Threading.Tasks;
using Yichen.Comm.IRepository;
using Yichen.Comm.Model.ViewModels.Basics;
using Yichen.Net.Model.DTO.Distribution;
using Yichen.Net.Model.Entities;

namespace Yichen.Net.IRepository
{
    /// <summary>
    /// 分销商表 工厂接口
    /// </summary>
    public interface ICoreCmsDistributionRepository : IBaseRepository<CoreCmsDistribution>
    {
        /// <summary>
        ///     根据条件查询分页数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="typeId">类型</param>
        /// <returns></returns>
        Task<IPageList<CoreCmsDistributionOrder>> QueryOrderPageAsync(int userId, int pageIndex = 1,
            int pageSize = 20, int typeId = 0);


        /// <summary>
        ///     获取代理商排行
        /// </summary>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <returns></returns>
        Task<IPageList<DistributionRankingDTO>> QueryRankingPageAsync(int pageIndex = 1, int pageSize = 20);

    }
}
