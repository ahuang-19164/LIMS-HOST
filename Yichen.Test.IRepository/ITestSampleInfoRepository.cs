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

using System.Data;
using Yichen.Test.Model.table;
using Yichen.Per.Model.table;

namespace Yichen.Per.IRepository
{
	/// <summary>
    ///  工厂接口
    /// </summary>
    public interface ITestSampleInfoRepository : IBaseRepository<test_sampleInfo>
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
        Task<test_sampleInfo> GetByBarcode(string barcode);

        /// <summary>
        /// 修改test表中的信息
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        Task<int> TestInfoEdit(string perid,Dictionary<string,object> pairs);
        /// <summary>
        /// 查询录入样本信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<DataTable> GetEntryInfo(DateTime startTime, DateTime endTime, string userName);



        /// <summary>
        /// 删除录入条码信息
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        Task<bool> BarcodeDelete(test_sampleInfo sampleinfo);

        /// <summary>
        /// 审核插入检验表样本信息
        /// </summary>
        /// <param name="DRsampleInfo">录入样本信息表</param>
        /// <param name="receiveTime">接受时间</param>
        /// <param name="GroupCodes">同一个工作组组合项目编号集合</param>
        /// <param name="GgroupNames">同一个工作组组合项目名称集合</param>
        /// <param name="sampleID">唯一编号</param>
        /// <param name="GroupNO">专业组编号</param>
        /// <param name="groupFlowNO">流程编号</param>
        /// <param name="delGroupState">委托状态</param>
        /// <param name="delGroupClientNO">委托单位</param>
        /// <param name="UserName">操作用户</param>
        /// <param name="ReceiveState">是否需要接收操作 true 为是   false 不用</param>
        /// <returns>返回testid</returns>
        Task<int> InsertTestSampleInfo(per_sampleInfo perinfo, string sampleID, DateTime receiveTime, string GroupCodes, string GroupNames,string GroupNO, string groupFlowNO, bool delGroupState, string delGroupClientNO, string UserName, bool ReceiveState = true);










        #region 重写增删改查操作===========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<WebApiCallBack> InsertAsync(test_sampleInfo entity);



        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<WebApiCallBack> UpdateAsync(test_sampleInfo entity);


        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<WebApiCallBack> UpdateAsync(List<test_sampleInfo> entity);


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
        //Task<List<test_sampleInfo>> GetCaChe();

        ///// <summary>
        /////     更新cache
        ///// </summary>
        //Task<List<test_sampleInfo>> UpdateCaChe();

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
        //Task<List<test_sampleInfo>> QueryAsync(
        //    Expression<Func<test_sampleInfo, bool>> predicate,
        //    Expression<Func<test_sampleInfo, object>> orderByExpression, OrderByType orderByType,bool blUseNoLock = false);

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
        Task<IPageList<test_sampleInfo>> QueryPageAsync(
            Expression<Func<test_sampleInfo, bool>> predicate,
            Expression<Func<test_sampleInfo, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);

    }
}
