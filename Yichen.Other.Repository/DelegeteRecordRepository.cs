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
using Yichen.Other.Model.table;
using Yichen.Other.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;

namespace Yichen.Other.Repository
{
    /// <summary>
    ///  接口实现
    /// </summary>
    public class DelegeteRecordRepository : BaseRepository<DelegeteRecord>, IDelegeteRecordRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public DelegeteRecordRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       public async Task<int> InsertDelegeteRecord(int testid, List<DelegeteRecord> pairs)
        {
            pairs.ForEach(p =>
            {
                p.testid=testid;
                
            });
            return await DbClient.Insertable<DelegeteRecord>(pairs).ExecuteCommandAsync();
        }

       #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> InsertAsync(DelegeteRecord entity)
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
        public  async Task<WebApiCallBack> UpdateAsync(DelegeteRecord entity)
        {
            var jm = new WebApiCallBack();

            var oldModel = await DbClient.Queryable<DelegeteRecord>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
            jm.msg = "不存在此信息";
            return jm;
            }
            //事物处理过程开始
        	oldModel.id = entity.id;
            oldModel.perid = entity.perid;
            oldModel.testid = entity.testid;
            oldModel.sampleID = entity.sampleID;
            oldModel.barcode = entity.barcode;
            oldModel.delegateStateNO = entity.delegateStateNO;
            oldModel.delegateClientNO = entity.delegateClientNO;
            oldModel.itemCodes = entity.itemCodes;
            oldModel.itemNames = entity.itemNames;
            oldModel.reason = entity.reason;
            oldModel.creater = entity.creater;
            oldModel.createTime = entity.createTime;
            oldModel.record = entity.record;
            oldModel.checker = entity.checker;
            oldModel.checkTime = entity.checkTime;
            oldModel.delePerson = entity.delePerson;
            oldModel.state = entity.state;
            oldModel.dstate = entity.dstate;
            oldModel.remark = entity.remark;
            
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
        public  async Task<WebApiCallBack> UpdateAsync(List<DelegeteRecord> entity)
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

            var bl = await DbClient.Deleteable<DelegeteRecord>(id).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Deleteable<DelegeteRecord>().In(ids).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Updateable<DelegeteRecord>().SetColumns(p => p.dstate == true).Where(p => p.id == Convert.ToInt32(id)).ExecuteCommandHasChangeAsync();
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
        //public async Task<List<DelegeteRecord>> GetCaChe()
        //{
        //    var cache = ManualDataCache.Instance.Get<List<DelegeteRecord>>(GlobalConstVars.CacheDelegeteRecord);
        //    if (cache != null)
        //    {
        //        return cache;
        //    }
        //    return await UpdateCaChe();
        //}

        ///// <summary>
        /////     更新cache
        ///// </summary>
        //public async Task<List<DelegeteRecord>> UpdateCaChe()
        //{
        //    var list = await DbClient.Queryable<DelegeteRecord>().With(SqlWith.NoLock).ToListAsync();
        //    ManualDataCache.Instance.Set(GlobalConstVars.CacheDelegeteRecord, list);
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
        public  async Task<IPageList<DelegeteRecord>> QueryPageAsync(Expression<Func<DelegeteRecord, bool>> predicate,
            Expression<Func<DelegeteRecord, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<DelegeteRecord> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<DelegeteRecord>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new DelegeteRecord
                {
                      id = p.id,
                perid = p.perid,
                testid = p.testid,
                sampleID = p.sampleID,
                barcode = p.barcode,
                delegateStateNO = p.delegateStateNO,
                delegateClientNO = p.delegateClientNO,
                itemCodes = p.itemCodes,
                itemNames = p.itemNames,
                reason = p.reason,
                creater = p.creater,
                createTime = p.createTime,
                record = p.record,
                checker = p.checker,
                checkTime = p.checkTime,
                delePerson = p.delePerson,
                state = p.state,
                dstate = p.dstate,
                remark = p.remark,
                
                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<DelegeteRecord>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new DelegeteRecord
                {
                      id = p.id,
                perid = p.perid,
                testid = p.testid,
                sampleID = p.sampleID,
                barcode = p.barcode,
                delegateStateNO = p.delegateStateNO,
                delegateClientNO = p.delegateClientNO,
                itemCodes = p.itemCodes,
                itemNames = p.itemNames,
                reason = p.reason,
                creater = p.creater,
                createTime = p.createTime,
                record = p.record,
                checker = p.checker,
                checkTime = p.checkTime,
                delePerson = p.delePerson,
                state = p.state,
                dstate = p.dstate,
                remark = p.remark,
                
                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<DelegeteRecord>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
