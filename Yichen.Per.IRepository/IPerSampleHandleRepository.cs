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
    public interface IPerSampleHandleRepository : IBaseRepository<per_sampleInfo>
    {

        /// <summary>
        /// 条码号是否存在
        /// </summary>
        /// <param name="brcode"></param>
        /// <returns></returns>
        Task<bool> BarcodeExist(string brcode);
        /// <summary>
        /// 根据条码获取样本信息
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        Task<per_sampleInfo> GetByBarcode(string barcode);
        /// <summary>
        /// 查询录入样本信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<DataTable> GetEntryInfo(DateTime startTime, DateTime endTime, string userName);
        /// <summary>
        /// 插入录入标本信息,返回插入信息id
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<int> EntryInfo(Dictionary<string,object> info);

        /// <summary>
        /// 修改插入录入标本信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<int> EntryInfoEdit(int perid, Dictionary<string,object> info);


        /// <summary>
        /// 录入信息审核状态修改
        /// </summary>
        /// <param name="perid">录入信息id</param>
        /// <param name="reciveTime">接收时间</param>
        /// <param name="blendConut">样本信息数量</param>
        /// <returns></returns>
        Task<int> PerCheckInfo(int perid, DateTime reciveTime,string userName,int blendConut=1);



        /// <summary>
        /// 删除录入条码信息
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        Task<bool> BarcodeDelete(per_sampleInfo sampleinfo);

        #region 重写增删改查操作===========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<WebApiCallBack> InsertAsync(per_sampleInfo entity);



        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<WebApiCallBack> UpdateAsync(per_sampleInfo entity);


        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<WebApiCallBack> UpdateAsync(List<per_sampleInfo> entity);


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


        /// <summary>
        /// 隐藏指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WebApiCallBack> HideByIdAsync(object id);

        #endregion

        #region 获取缓存的所有数据==========================================================

        ///// <summary>
        ///// 获取缓存的所有数据
        ///// </summary>
        ///// <returns></returns>
        //Task<List<per_sampleInfo>> GetCaChe();

        ///// <summary>
        /////     更新cache
        ///// </summary>
        //Task<List<per_sampleInfo>> UpdateCaChe();

        #endregion


        ///// <summary>
        /////重写根据条件查询数据
        /////var wheres = PredicateBuilder.True<CoreCmsGoodsParams>();
        /////wheres = wheres.And(p => p.id == id);
        /////Expression<Func<CoreCmsGoodsParams, object>> orderEx;
        /////orderEx = p => p.id;
        /////OrderByType.Asc,
        ///// </summary>
        ///// <param name="predicate">判断集合</param>
        ///// <param name="orderByType">排序方式</param>
        ///// <param name="pageIndex">当前页面索引</param>
        ///// <param name="pageSize">分布大小</param>
        ///// <param name="orderByExpression"></param>
        ///// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        ///// <returns></returns>
        //Task<List<per_sampleInfo>> QueryAsync(
        //    Expression<Func<per_sampleInfo, bool>> predicate,
        //    Expression<Func<per_sampleInfo, object>> orderByExpression, OrderByType orderByType,bool blUseNoLock = false);

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
        Task<IPageList<per_sampleInfo>> QueryPageAsync(
            Expression<Func<per_sampleInfo, bool>> predicate,
            Expression<Func<per_sampleInfo, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);

    }
}
