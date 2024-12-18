using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Yichen.Comm.IRepository;
using Yichen.Comm.Model.ViewModels.Basics;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Net.Model.DTO;
using Yichen.Net.Model.Entities;
namespace Yichen.Net.IRepository
{
    /// <summary>
    ///     用户对表的提交记录 工厂接口
    /// </summary>
    public interface ICoreCmsFormSubmitRepository : IBaseRepository<CoreCmsFormSubmit>
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
        Task<IPageList<CoreCmsFormSubmit>> QueryPageAsync(
            Expression<Func<CoreCmsFormSubmit, bool>> predicate,
            Expression<Func<CoreCmsFormSubmit, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);


        /// <summary>
        ///     表单支付
        /// </summary>
        /// <param name="id">序列</param>
        /// <returns></returns>
        Task<WebApiCallBack> Pay(int id);


        /// <summary>
        ///     获取表单的统计数据
        /// </summary>
        /// <param name="formId">表单序列</param>
        /// <param name="day">多少天内的数据</param>
        /// <returns></returns>
        Task<FormStatisticsViewDto> GetStatisticsByFormid(int formId, int day);

        #region 重写增删改查操作===========================================================

        /// <summary>
        ///     重写异步插入方法并返回自增值
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        Task<int> InsertReturnIdentityAsync(CoreCmsFormSubmit entity);

        /// <summary>
        ///     重写异步插入方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> InsertAsync(CoreCmsFormSubmit entity);


        /// <summary>
        ///     重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> UpdateAsync(CoreCmsFormSubmit entity);


        /// <summary>
        ///     重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> UpdateAsync(List<CoreCmsFormSubmit> entity);


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