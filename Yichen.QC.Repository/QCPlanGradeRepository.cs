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
    public class QCPlanGradeRepository : BaseRepository<QCPlanGrade>, IQCPlanGradeRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public QCPlanGradeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> InsertAsync(QCPlanGrade entity)
        {
            var jm = new WebApiCallBack();

            var bl = await DbClient.Insertable(entity).ExecuteReturnIdentityAsync() > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;
            if (bl)
            {
                await UpdateCaChe();
            }

            return jm;
        }

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> UpdateAsync(QCPlanGrade entity)
        {
            var jm = new WebApiCallBack();

            var oldModel = await DbClient.Queryable<QCPlanGrade>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
            jm.msg = "不存在此信息";
            return jm;
            }
            //事物处理过程开始
        	oldModel.id = entity.id;
            oldModel.gradeid = entity.gradeid;
            oldModel.gradeCode = entity.gradeCode;
            oldModel.gradeName = entity.gradeName;
            oldModel.planItemid = entity.planItemid;
            oldModel.itemNO = entity.itemNO;
            oldModel.levelNO = entity.levelNO;
            oldModel.producer = entity.producer;
            oldModel.produceTime = entity.produceTime;
            oldModel.validityTime = entity.validityTime;
            oldModel.avgValue = entity.avgValue;
            oldModel.sdValue = entity.sdValue;
            oldModel.cvValue = entity.cvValue;
            oldModel.error = entity.error;
            oldModel.errorTEa = entity.errorTEa;
            oldModel.factoryX = entity.factoryX;
            oldModel.factoryRange = entity.factoryRange;
            oldModel.remark = entity.remark;
            oldModel.sort = entity.sort;
            oldModel.state = entity.state;
            oldModel.dstate = entity.dstate;
            
            //事物处理过程结束
            var bl = await DbClient.Updateable(oldModel).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            if (bl)
            {
                await UpdateCaChe();
            }

            return jm;
        }

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> UpdateAsync(List<QCPlanGrade> entity)
        {
            var jm = new WebApiCallBack();

            var bl = await DbClient.Updateable(entity).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            if (bl)
            {
                await UpdateCaChe();
            }

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

            var bl = await DbClient.Deleteable<QCPlanGrade>(id).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            if (bl)
            {
                await UpdateCaChe();
            }

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

            var bl = await DbClient.Deleteable<QCPlanGrade>().In(ids).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            if (bl)
            {
                await UpdateCaChe();
            }

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

            var bl = await DbClient.Updateable<QCPlanGrade>().SetColumns(p => p.dstate == true).Where(p => p.id == Convert.ToInt32(id)).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            if (bl)
            {
                await UpdateCaChe();
            }

            return jm;
        }


        #endregion

       #region 获取缓存的所有数据==========================================================

        /// <summary>
        /// 获取缓存的所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<QCPlanGrade>> GetCaChe()
        {
            var cache = ManualDataCache.Instance.Get<List<QCPlanGrade>>(GlobalConstVars.CacheQCPlanGrade);
            if (cache != null)
            {
                return cache;
            }
            return await UpdateCaChe();
        }

        /// <summary>
        ///     更新cache
        /// </summary>
        public async Task<List<QCPlanGrade>> UpdateCaChe()
        {
            var list = await DbClient.Queryable<QCPlanGrade>().With(SqlWith.NoLock).ToListAsync();
            ManualDataCache.Instance.Set(GlobalConstVars.CacheQCPlanGrade, list);
            return list;
        }

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
        public  async Task<IPageList<QCPlanGrade>> QueryPageAsync(Expression<Func<QCPlanGrade, bool>> predicate,
            Expression<Func<QCPlanGrade, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<QCPlanGrade> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<QCPlanGrade>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new QCPlanGrade
                {
                      id = p.id,
                gradeid = p.gradeid,
                gradeCode = p.gradeCode,
                gradeName = p.gradeName,
                planItemid = p.planItemid,
                itemNO = p.itemNO,
                levelNO = p.levelNO,
                producer = p.producer,
                produceTime = p.produceTime,
                validityTime = p.validityTime,
                avgValue = p.avgValue,
                sdValue = p.sdValue,
                cvValue = p.cvValue,
                error = p.error,
                errorTEa = p.errorTEa,
                factoryX = p.factoryX,
                factoryRange = p.factoryRange,
                remark = p.remark,
                sort = p.sort,
                state = p.state,
                dstate = p.dstate,
                
                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<QCPlanGrade>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new QCPlanGrade
                {
                      id = p.id,
                gradeid = p.gradeid,
                gradeCode = p.gradeCode,
                gradeName = p.gradeName,
                planItemid = p.planItemid,
                itemNO = p.itemNO,
                levelNO = p.levelNO,
                producer = p.producer,
                produceTime = p.produceTime,
                validityTime = p.validityTime,
                avgValue = p.avgValue,
                sdValue = p.sdValue,
                cvValue = p.cvValue,
                error = p.error,
                errorTEa = p.errorTEa,
                factoryX = p.factoryX,
                factoryRange = p.factoryRange,
                remark = p.remark,
                sort = p.sort,
                state = p.state,
                dstate = p.dstate,
                
                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<QCPlanGrade>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
