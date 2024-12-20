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
    public class ClientRecordRepository : BaseRepository<ClientRecord>, IClientRecordRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public ClientRecordRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> InsertAsync(ClientRecord entity)
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
        public  async Task<WebApiCallBack> UpdateAsync(ClientRecord entity)
        {
            var jm = new WebApiCallBack();

            var oldModel = await DbClient.Queryable<ClientRecord>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
            jm.msg = "不存在此信息";
            return jm;
            }
            //事物处理过程开始
        	oldModel.id = entity.id;
            oldModel.perid = entity.perid;
            oldModel.testid = entity.testid;
            oldModel.barcode = entity.barcode;
            oldModel.applyCodes = entity.applyCodes;
            oldModel.applyNames = entity.applyNames;
            oldModel.submitItemCodes = entity.submitItemCodes;
            oldModel.submitItemNames = entity.submitItemNames;
            oldModel.recordTypeNO = entity.recordTypeNO;
            oldModel.recordValue = entity.recordValue;
            oldModel.contact = entity.contact;
            oldModel.contactMode = entity.contactMode;
            oldModel.creater = entity.creater;
            oldModel.createTime = entity.createTime;
            oldModel.handleResult = entity.handleResult;
            oldModel.handleTypeNO = entity.handleTypeNO;
            oldModel.pleasLevel = entity.pleasLevel;
            oldModel.handler = entity.handler;
            oldModel.handleTime = entity.handleTime;
            oldModel.remark = entity.remark;
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
        public  async Task<WebApiCallBack> UpdateAsync(List<ClientRecord> entity)
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

            var bl = await DbClient.Deleteable<ClientRecord>(id).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Deleteable<ClientRecord>().In(ids).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Updateable<ClientRecord>().SetColumns(p => p.dstate == true).Where(p => p.id == Convert.ToInt32(id)).ExecuteCommandHasChangeAsync();
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
        //public async Task<List<ClientRecord>> GetCaChe()
        //{
        //    var cache = ManualDataCache.Instance.Get<List<ClientRecord>>(GlobalConstVars.CacheClientRecord);
        //    if (cache != null)
        //    {
        //        return cache;
        //    }
        //    return await UpdateCaChe();
        //}

        ///// <summary>
        /////     更新cache
        ///// </summary>
        //public async Task<List<ClientRecord>> UpdateCaChe()
        //{
        //    var list = await DbClient.Queryable<ClientRecord>().With(SqlWith.NoLock).ToListAsync();
        //    ManualDataCache.Instance.Set(GlobalConstVars.CacheClientRecord, list);
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
        public  async Task<IPageList<ClientRecord>> QueryPageAsync(Expression<Func<ClientRecord, bool>> predicate,
            Expression<Func<ClientRecord, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<ClientRecord> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<ClientRecord>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new ClientRecord
                {
                      id = p.id,
                perid = p.perid,
                testid = p.testid,
                barcode = p.barcode,
                applyCodes = p.applyCodes,
                applyNames = p.applyNames,
                submitItemCodes = p.submitItemCodes,
                submitItemNames = p.submitItemNames,
                recordTypeNO = p.recordTypeNO,
                recordValue = p.recordValue,
                contact = p.contact,
                contactMode = p.contactMode,
                creater = p.creater,
                createTime = p.createTime,
                handleResult = p.handleResult,
                handleTypeNO = p.handleTypeNO,
                pleasLevel = p.pleasLevel,
                handler = p.handler,
                handleTime = p.handleTime,
                remark = p.remark,
                state = p.state,
                dstate = p.dstate,
                
                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<ClientRecord>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new ClientRecord
                {
                      id = p.id,
                perid = p.perid,
                testid = p.testid,
                barcode = p.barcode,
                applyCodes = p.applyCodes,
                applyNames = p.applyNames,
                submitItemCodes = p.submitItemCodes,
                submitItemNames = p.submitItemNames,
                recordTypeNO = p.recordTypeNO,
                recordValue = p.recordValue,
                contact = p.contact,
                contactMode = p.contactMode,
                creater = p.creater,
                createTime = p.createTime,
                handleResult = p.handleResult,
                handleTypeNO = p.handleTypeNO,
                pleasLevel = p.pleasLevel,
                handler = p.handler,
                handleTime = p.handleTime,
                remark = p.remark,
                state = p.state,
                dstate = p.dstate,
                
                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<ClientRecord>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
