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
using Yichen.Net.Caching.Manual;
using Yichen.Net.Configuration;
using Yichen.Comm.Model.ViewModels.Basics;
using Yichen.Comm.Model.ViewModels.UI;
using SqlSugar;
using Yichen.Comm.Repository;
using Yichen.System.Model;
using Yichen.System.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;

namespace Yichen.System.Repository
{
    /// <summary>
    ///  接口实现
    /// </summary>
    public class GroupPowerRepository : BaseRepository<GroupPower>, IGroupPowerRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public GroupPowerRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> InsertAsync(GroupPower entity)
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
        public  async Task<WebApiCallBack> UpdateAsync(GroupPower entity)
        {
            var jm = new WebApiCallBack();

            var oldModel = await DbClient.Queryable<GroupPower>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
            jm.msg = "不存在此信息";
            return jm;
            }
            //事物处理过程开始
        	oldModel.id = entity.id;
            oldModel.workNO = entity.workNO;
            oldModel.testNO = entity.testNO;
            oldModel.userNO = entity.userNO;
            oldModel.sampleEdit = entity.sampleEdit;
            oldModel.sampleSave = entity.sampleSave;
            oldModel.editCheck = entity.editCheck;
            oldModel.sampleReceive = entity.sampleReceive;
            oldModel.sampleReturn = entity.sampleReturn;
            oldModel.sampleSelect = entity.sampleSelect;
            oldModel.testReturn = entity.testReturn;
            oldModel.saveResult = entity.saveResult;
            oldModel.saveReResult = entity.saveReResult;
            oldModel.checkeds = entity.checkeds;
            oldModel.reCheck = entity.reCheck;
            oldModel.batch = entity.batch;
            oldModel.batchReturn = entity.batchReturn;
            oldModel.batchSave = entity.batchSave;
            oldModel.batchReSave = entity.batchReSave;
            oldModel.batchCheck = entity.batchCheck;
            oldModel.batchReCheck = entity.batchReCheck;
            oldModel.reportReturn = entity.reportReturn;
            oldModel.state = entity.state;
            oldModel.sort = entity.sort;
            
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
        public  async Task<WebApiCallBack> UpdateAsync(List<GroupPower> entity)
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

            var bl = await DbClient.Deleteable<GroupPower>(id).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Deleteable<GroupPower>().In(ids).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            if (bl)
            {
                await UpdateCaChe();
            }

            return jm;
        }

        ///// <summary>
        ///// 隐藏指定ID的数据
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public new async Task<WebApiCallBack> HideByIdAsync(object id)
        //{
        //    var jm = new WebApiCallBack();

        //    var bl = await DbClient.Updateable<GroupPower>().SetColumns(p => p.dstate == true).Where(p => p.id == Convert.ToInt32(id)).ExecuteCommandHasChangeAsync();
        //    jm.code = bl ? 0 : 1;
        //    jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
        //    if (bl)
        //    {
        //        await UpdateCaChe();
        //    }

        //    return jm;
        //}


        #endregion

       #region 获取缓存的所有数据==========================================================

        /// <summary>
        /// 获取缓存的所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<GroupPower>> GetCaChe()
        {
            var cache = ManualDataCache.Instance.Get<List<GroupPower>>(GlobalConstVars.CacheGroupPower);
            if (cache != null)
            {
                return cache;
            }
            return await UpdateCaChe();
        }

        /// <summary>
        ///     更新cache
        /// </summary>
        public async Task<List<GroupPower>> UpdateCaChe()
        {
            var list = await DbClient.Queryable<GroupPower>().With(SqlWith.NoLock).ToListAsync();
            ManualDataCache.Instance.Set(GlobalConstVars.CacheGroupPower, list);
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
        public  async Task<IPageList<GroupPower>> QueryPageAsync(Expression<Func<GroupPower, bool>> predicate,
            Expression<Func<GroupPower, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<GroupPower> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<GroupPower>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new GroupPower
                {
                      id = p.id,
                workNO = p.workNO,
                testNO = p.testNO,
                userNO = p.userNO,
                sampleEdit = p.sampleEdit,
                sampleSave = p.sampleSave,
                editCheck = p.editCheck,
                sampleReceive = p.sampleReceive,
                sampleReturn = p.sampleReturn,
                sampleSelect = p.sampleSelect,
                testReturn = p.testReturn,
                saveResult = p.saveResult,
                saveReResult = p.saveReResult,
                checkeds = p.checkeds,
                reCheck = p.reCheck,
                batch = p.batch,
                batchReturn = p.batchReturn,
                batchSave = p.batchSave,
                batchReSave = p.batchReSave,
                batchCheck = p.batchCheck,
                batchReCheck = p.batchReCheck,
                reportReturn = p.reportReturn,
                state = p.state,
                sort = p.sort,
                
                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<GroupPower>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new GroupPower
                {
                      id = p.id,
                workNO = p.workNO,
                testNO = p.testNO,
                userNO = p.userNO,
                sampleEdit = p.sampleEdit,
                sampleSave = p.sampleSave,
                editCheck = p.editCheck,
                sampleReceive = p.sampleReceive,
                sampleReturn = p.sampleReturn,
                sampleSelect = p.sampleSelect,
                testReturn = p.testReturn,
                saveResult = p.saveResult,
                saveReResult = p.saveReResult,
                checkeds = p.checkeds,
                reCheck = p.reCheck,
                batch = p.batch,
                batchReturn = p.batchReturn,
                batchSave = p.batchSave,
                batchReSave = p.batchReSave,
                batchCheck = p.batchCheck,
                batchReCheck = p.batchReCheck,
                reportReturn = p.reportReturn,
                state = p.state,
                sort = p.sort,
                
                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<GroupPower>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
