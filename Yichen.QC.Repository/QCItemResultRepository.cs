/***********************************************************************
 *            Project: Yichen
 *        ProjectName: 屹辰智禾管理系统                                
 *                Web: https://www.zui51.com                 
 *             Author: 屹辰                                       
 *              Email: 499715561@qq.com                              
 *         CreateTime: 
 *        Description: 暂无
 ***********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Yichen.Net.Caching.Manual;
using Yichen.Net.Configuration;
using Yichen.QC.Model;
using Yichen.QC.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Repository;

using Yichen.Comm.Model.ViewModels.Basics;
using Yichen.Comm.Model.ViewModels.UI;
using SqlSugar;

namespace Yichen.QC.Repository
{
    /// <summary>
    ///  接口实现
    /// </summary>
    public class QCItemResultRepository : BaseRepository<QCItemResult>, IQCItemResultRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public QCItemResultRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> InsertAsync(QCItemResult entity)
        {
            var jm = new WebApiCallBack();

            var bl = await DbClient.Insertable(entity).ExecuteReturnIdentityAsync() > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;
            //if (bl)
            //{
            //    await UpdateCaChe();
            //}

            return jm;
        }

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> UpdateAsync(QCItemResult entity)
        {
            var jm = new WebApiCallBack();

            var oldModel = await DbClient.Queryable<QCItemResult>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
            jm.msg = "不存在此信息";
            return jm;
            }
            //事物处理过程开始
        	oldModel.id = entity.id;
            oldModel.planid = entity.planid;
            oldModel.planItemid = entity.planItemid;
            oldModel.planGradeid = entity.planGradeid;
            oldModel.itemNO = entity.itemNO;
            oldModel.itemResult = entity.itemResult;
            oldModel.resultState = entity.resultState;
            oldModel.resultType = entity.resultType;
            oldModel.resultRule = entity.resultRule;
            oldModel.zVlaue = entity.zVlaue;
            oldModel.remark = entity.remark;
            oldModel.qcTime = entity.qcTime;
            oldModel.creater = entity.creater;
            oldModel.createTime = entity.createTime;
            oldModel.sort = entity.sort;
            oldModel.state = entity.state;
            oldModel.dstate = entity.dstate;
            
            //事物处理过程结束
            var bl = await DbClient.Updateable(oldModel).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            //if (bl)
            //{
            //    await UpdateCaChe();
            //}

            return jm;
        }

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> UpdateAsync(List<QCItemResult> entity)
        {
            var jm = new WebApiCallBack();

            var bl = await DbClient.Updateable(entity).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            //if (bl)
            //{
            //    await UpdateCaChe();
            //}

            return jm;
        }

        /// <summary>
        /// 重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> DeleteByIdAsync(object id)
        {
            var jm = new WebApiCallBack();

            var bl = await DbClient.Deleteable<QCItemResult>(id).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            //if (bl)
            //{
            //    await UpdateCaChe();
            //}

            return jm;
        }

        /// <summary>
        /// 重写删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> DeleteByIdsAsync(int[] ids)
        {
            var jm = new WebApiCallBack();

            var bl = await DbClient.Deleteable<QCItemResult>().In(ids).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            //if (bl)
            //{
            //    await UpdateCaChe();
            //}

            return jm;
        }

        /// <summary>
        /// 隐藏指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> HideByIdAsync(object id)
        {
            var jm = new WebApiCallBack();

            var bl = await DbClient.Updateable<QCItemResult>().SetColumns(p => p.dstate == true).Where(p => p.id == Convert.ToInt32(id)).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            //if (bl)
            //{
            //    await UpdateCaChe();
            //}

            return jm;
        }


        #endregion

       #region 获取缓存的所有数据==========================================================

        ///// <summary>
        ///// 获取缓存的所有数据
        ///// </summary>
        ///// <returns></returns>
        //public async Task<List<QCItemResult>> GetCaChe()
        //{
        //    var cache = ManualDataCache.Instance.Get<List<QCItemResult>>(GlobalConstVars.CacheQCItemResult);
        //    if (cache != null)
        //    {
        //        return cache;
        //    }
        //    return await UpdateCaChe();
        //}

        ///// <summary>
        /////     更新cache
        ///// </summary>
        //public async Task<List<QCItemResult>> UpdateCaChe()
        //{
        //    var list = await DbClient.Queryable<QCItemResult>().With(SqlWith.NoLock).ToListAsync();
        //    ManualDataCache.Instance.Set(GlobalConstVars.CacheQCItemResult, list);
        //    return list;
        //}

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
        public  async Task<IPageList<QCItemResult>> QueryPageAsync(Expression<Func<QCItemResult, bool>> predicate,
            Expression<Func<QCItemResult, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<QCItemResult> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<QCItemResult>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new QCItemResult
                {
                      id = p.id,
                planid = p.planid,
                planItemid = p.planItemid,
                planGradeid = p.planGradeid,
                itemNO = p.itemNO,
                itemResult = p.itemResult,
                resultState = p.resultState,
                resultType = p.resultType,
                resultRule = p.resultRule,
                zVlaue = p.zVlaue,
                remark = p.remark,
                qcTime = p.qcTime,
                creater = p.creater,
                createTime = p.createTime,
                sort = p.sort,
                state = p.state,
                dstate = p.dstate,
                
                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<QCItemResult>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new QCItemResult
                {
                      id = p.id,
                planid = p.planid,
                planItemid = p.planItemid,
                planGradeid = p.planGradeid,
                itemNO = p.itemNO,
                itemResult = p.itemResult,
                resultState = p.resultState,
                resultType = p.resultType,
                resultRule = p.resultRule,
                zVlaue = p.zVlaue,
                remark = p.remark,
                qcTime = p.qcTime,
                creater = p.creater,
                createTime = p.createTime,
                sort = p.sort,
                state = p.state,
                dstate = p.dstate,
                
                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<QCItemResult>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
