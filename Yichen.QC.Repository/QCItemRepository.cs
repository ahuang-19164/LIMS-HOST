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
    public class QCItemRepository : BaseRepository<QCItem>, IQCItemRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public QCItemRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> InsertAsync(QCItem entity)
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
        public  async Task<WebApiCallBack> UpdateAsync(QCItem entity)
        {
            var jm = new WebApiCallBack();

            var oldModel = await DbClient.Queryable<QCItem>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
            jm.msg = "不存在此信息";
            return jm;
            }
            //事物处理过程开始
        	oldModel.id = entity.id;
            oldModel.no = entity.no;
            oldModel.workNO = entity.workNO;
            oldModel.groupNO = entity.groupNO;
            oldModel.itemNO = entity.itemNO;
            oldModel.qcgroupNO = entity.qcgroupNO;
            oldModel.planNO = entity.planNO;
            oldModel.levelNO = entity.levelNO;
            oldModel.gradeNO = entity.gradeNO;
            oldModel.batchNO = entity.batchNO;
            oldModel.avgValue = entity.avgValue;
            oldModel.sdValue = entity.sdValue;
            oldModel.cvValue = entity.cvValue;
            oldModel.remark = entity.remark;
            oldModel.state = entity.state;
            oldModel.dstate = entity.dstate;
            oldModel.sort = entity.sort;
            oldModel.creater = entity.creater;
            oldModel.createTime = entity.createTime;
            
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
        public  async Task<WebApiCallBack> UpdateAsync(List<QCItem> entity)
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

            var bl = await DbClient.Deleteable<QCItem>(id).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Deleteable<QCItem>().In(ids).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Updateable<QCItem>().SetColumns(p => p.dstate == true).Where(p => p.id == Convert.ToInt32(id)).ExecuteCommandHasChangeAsync();
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
        public async Task<List<QCItem>> GetCaChe()
        {
            var cache = ManualDataCache.Instance.Get<List<QCItem>>(GlobalConstVars.CacheQCItem);
            if (cache != null)
            {
                return cache;
            }
            return await UpdateCaChe();
        }

        /// <summary>
        ///     更新cache
        /// </summary>
        public async Task<List<QCItem>> UpdateCaChe()
        {
            var list = await DbClient.Queryable<QCItem>().With(SqlWith.NoLock).ToListAsync();
            ManualDataCache.Instance.Set(GlobalConstVars.CacheQCItem, list);
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
        public  async Task<IPageList<QCItem>> QueryPageAsync(Expression<Func<QCItem, bool>> predicate,
            Expression<Func<QCItem, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<QCItem> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<QCItem>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new QCItem
                {
                      id = p.id,
                no = p.no,
                workNO = p.workNO,
                groupNO = p.groupNO,
                itemNO = p.itemNO,
                qcgroupNO = p.qcgroupNO,
                planNO = p.planNO,
                levelNO = p.levelNO,
                gradeNO = p.gradeNO,
                batchNO = p.batchNO,
                avgValue = p.avgValue,
                sdValue = p.sdValue,
                cvValue = p.cvValue,
                remark = p.remark,
                state = p.state,
                dstate = p.dstate,
                sort = p.sort,
                creater = p.creater,
                createTime = p.createTime,
                
                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<QCItem>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new QCItem
                {
                      id = p.id,
                no = p.no,
                workNO = p.workNO,
                groupNO = p.groupNO,
                itemNO = p.itemNO,
                qcgroupNO = p.qcgroupNO,
                planNO = p.planNO,
                levelNO = p.levelNO,
                gradeNO = p.gradeNO,
                batchNO = p.batchNO,
                avgValue = p.avgValue,
                sdValue = p.sdValue,
                cvValue = p.cvValue,
                remark = p.remark,
                state = p.state,
                dstate = p.dstate,
                sort = p.sort,
                creater = p.creater,
                createTime = p.createTime,
                
                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<QCItem>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
