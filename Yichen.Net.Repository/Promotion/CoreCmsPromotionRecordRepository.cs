/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Repository;
using Yichen.Net.IRepository;
using Yichen.Net.Model.Entities;
namespace Yichen.Net.Repository
{
    /// <summary>
    /// 促销活动记录表 接口实现
    /// </summary>
    public class CoreCmsPromotionRecordRepository : BaseRepository<CoreCmsPromotionRecord>, ICoreCmsPromotionRecordRepository
    {
        public CoreCmsPromotionRecordRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
