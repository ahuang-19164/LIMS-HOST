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
using Yichen.Comm.IRepository;
using Yichen.Per.Model.table;
using System.Data;

namespace Yichen.Per.IRepository
{
	/// <summary>
    ///  工厂接口
    /// </summary>
    public interface ISampleInfoOtherRepository : IBaseRepository<SampleInfoOther>
    {



        /// <summary>
        /// 根据录入id查询录入其他信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<SampleInfoDelete> GetInfoByPerid(int perid);


        /// <summary>
        /// 插入录入标本信息,返回插入信息id
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<int> EntryOtherInfo(Dictionary<string, object> info);

        /// <summary>
        /// 修改插入录入标本信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<int> EntryOteherInfoEdit(int perid, Dictionary<string, object> info);


        #region 重写增删改查操作===========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<WebApiCallBack> InsertAsync(SampleInfoOther entity);


        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<WebApiCallBack> UpdateAsync(SampleInfoOther entity);


        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<WebApiCallBack> UpdateAsync(List<SampleInfoOther> entity);


        /// <summary>
        /// 重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WebApiCallBack> DeleteByIdAsync(object id);


        /// <summary>
        /// 重写删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<WebApiCallBack> DeleteByIdsAsync(int[] ids);


        ///// <summary>
        ///// 隐藏指定ID的数据
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //Task<WebApiCallBack> HideByIdAsync(object id);

        #endregion

        #region 获取缓存的所有数据==========================================================

        ///// <summary>
        ///// 获取缓存的所有数据
        ///// </summary>
        ///// <returns></returns>
        //Task<List<SampleInfoOther>> GetCaChe();

        ///// <summary>
        /////     更新cache
        ///// </summary>
        //Task<List<SampleInfoOther>> UpdateCaChe();

        #endregion


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
        Task<IPageList<SampleInfoOther>> QueryPageAsync(
            Expression<Func<SampleInfoOther, bool>> predicate,
            Expression<Func<SampleInfoOther, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);

    }
}
