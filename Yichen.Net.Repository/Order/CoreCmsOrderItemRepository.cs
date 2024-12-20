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
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Repository;
using Yichen.Net.Configuration;
using Yichen.Net.IRepository;
using Yichen.Net.Model.Entities;
namespace Yichen.Net.Repository
{
    /// <summary>
    /// 订单明细表 接口实现
    /// </summary>
    public class CoreCmsOrderItemRepository : BaseRepository<CoreCmsOrderItem>, ICoreCmsOrderItemRepository
    {
        public CoreCmsOrderItemRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        #region 算订单的商品退了多少个(未发货的退货数量，已发货的退货不算)

        /// <summary>
        /// 算订单的商品退了多少个(未发货的退货数量，已发货的退货不算)
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="sn"></param>
        /// <returns></returns>
        public int GetaftersalesNums(string orderId, string sn)
        {
            var sum = DbClient.Queryable<CoreCmsBillAftersalesItem, CoreCmsBillAftersales>((item, parent) =>
                    new object[]
                    {
                        JoinType.Inner, item.aftersalesId == parent.aftersalesId
                    }).Where((item, parent) => parent.orderId == orderId)
                .Where((item, parent) => parent.status == (int)GlobalEnumVars.OrderStatus.Complete)
                .Where((item, parent) => item.sn == sn)
                .Where((item, parent) => parent.type == (int)GlobalEnumVars.BillAftersalesStatus.WaitAudit)
                .Sum((item, parent) => item.nums);
            return sum;
        }
        #endregion
    }
}
