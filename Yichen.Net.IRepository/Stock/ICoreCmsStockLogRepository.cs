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
using System.Linq.Expressions;
using System.Threading.Tasks;
using Yichen.Comm.IRepository;
using Yichen.Comm.Model.ViewModels.Basics;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Net.Model.Entities;
namespace Yichen.Net.IRepository
{
    /// <summary>
    ///     库存操作详情表 工厂接口
    /// </summary>
    public interface ICoreCmsStockLogRepository : IBaseRepository<CoreCmsStockLog>
    {
        /// <summary>
        ///     重写根据条件查询分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<IPageList<CoreCmsStockLog>> QueryPageAsync(
            Expression<Func<CoreCmsStockLog, bool>> predicate,
            Expression<Func<CoreCmsStockLog, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);

        #region 重写增删改查操作===========================================================

        /// <summary>
        ///     重写异步插入方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> InsertAsync(CoreCmsStockLog entity);


        /// <summary>
        ///     重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> UpdateAsync(CoreCmsStockLog entity);


        /// <summary>
        ///     重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> UpdateAsync(List<CoreCmsStockLog> entity);


        /// <summary>
        ///     重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> DeleteByIdAsync(object id);


        /// <summary>
        ///     重写删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> DeleteByIdsAsync(int[] ids);

        #endregion
    }
}