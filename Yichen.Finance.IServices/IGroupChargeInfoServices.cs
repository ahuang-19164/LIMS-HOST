/***********************************************************************
 *            Project: Yichen
 *        ProjectName: 屹辰智禾管理系统                                
 *                Web: https://www.zui51.com                 
 *             Author: 屹辰                                       
 *              Email: 499715561@qq.com                              
 *         CreateTime: 
 *        Description: 暂无
 ***********************************************************************/

using System.Linq.Expressions;
using Yichen.Comm.Model.ViewModels.Basics;
using Yichen.Comm.Model.ViewModels.UI;
using SqlSugar;
using Yichen.Comm.IServices;
using Yichen.Finance.Model.table;

namespace Yichen.Finance.IServices
{
	/// <summary>
    ///  服务工厂接口
    /// </summary>
    public interface IGroupChargeInfoServices : IBaseServices<finance_group_charge>
    {
        #region 重写增删改查操作===========================================================


         /// <summary>
        /// 重写异步查询方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        new Task<WebApiCallBack> SelectAsync();

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        new Task<WebApiCallBack> InsertAsync(finance_group_charge entity);

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        new Task<WebApiCallBack> UpdateAsync(finance_group_charge entity);

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        new Task<WebApiCallBack> UpdateAsync(List<finance_group_charge> entity);

        /// <summary>
        /// 重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        new Task<WebApiCallBack> DeleteByIdAsync(object id);

        /// <summary>
        /// 重写删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        new Task<WebApiCallBack> DeleteByIdsAsync(int[] ids);

        ///// <summary>
        ///// 隐藏指定ID的数据
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //new Task<WebApiCallBack> HideByIdAsync(object id);

        #endregion

        
        #region 获取缓存的所有数据==========================================================

        /// <summary>
        /// 获取缓存的所有数据
        /// </summary>
        /// <returns></returns>
        new Task<List<finance_group_charge>> GetCaChe();

        /// <summary>
        ///     更新cache
        /// </summary>
        new Task<List<finance_group_charge>> UpdateCaChe();

        #endregion

        #region 重写根据条件查询分页数据
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
        new Task<IPageList<finance_group_charge>> QueryPageAsync(
            Expression<Func<finance_group_charge, bool>> predicate,
            Expression<Func<finance_group_charge, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);
        #endregion
    }
}
