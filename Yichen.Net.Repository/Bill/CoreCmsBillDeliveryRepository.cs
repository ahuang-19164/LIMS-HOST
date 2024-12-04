/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Comm.Repository;
using Yichen.Net.IRepository;
using Yichen.Net.Model.Entities;
namespace Yichen.Net.Repository
{
    /// <summary>
    /// 发货单表 接口实现
    /// </summary>
    public class CoreCmsBillDeliveryRepository : BaseRepository<CoreCmsBillDelivery>, ICoreCmsBillDeliveryRepository
    {
        public CoreCmsBillDeliveryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// 发货单统计7天统计
        /// </summary>
        /// <returns></returns>
        public async Task<List<StatisticsOut>> Statistics()
        {
            var dt = DateTime.Now.AddDays(-8);

            var list = await DbClient.Queryable<CoreCmsBillDelivery>()
                .Where(p => p.createTime >= dt)
                .Select(it => new
                {
                    it.deliveryId,
                    createTime = it.createTime.Date
                })
                .MergeTable()
                .GroupBy(it => it.createTime)
                .Select(it => new StatisticsOut { day = it.createTime.ToString("yyyy-MM-dd"), nums = SqlFunc.AggregateCount(it.deliveryId) })
                .ToListAsync();

            return list;
        }

    }
}
