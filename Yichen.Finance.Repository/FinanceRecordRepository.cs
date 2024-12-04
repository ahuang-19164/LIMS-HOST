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
using Yichen.Finance.IRepository;
using Yichen.Finance.Model.table;
using Yichen.Comm.IRepository.UnitOfWork;

namespace Yichen.Finance.Repository
{
    /// <summary>
    ///  接口实现
    /// </summary>
    public class FinanceRecordRepository : BaseRepository<finance_record>, IFinanceRecordRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public FinanceRecordRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public  async new Task<WebApiCallBack> InsertAsync(finance_record entity)
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
        public  async new Task<WebApiCallBack> UpdateAsync(finance_record entity)
        {
            var jm = new WebApiCallBack();

            var oldModel = await DbClient.Queryable<finance_record>().In(entity.id).SingleAsync();
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
            oldModel.operatType = entity.operatType;
            oldModel.record = entity.record;
            oldModel.reason = entity.reason;
            oldModel.operater = entity.operater;
            oldModel.createTime = entity.createTime;
            oldModel.clientShow = entity.clientShow;
            
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
        public  async new Task<WebApiCallBack> UpdateAsync(List<finance_record> entity)
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
        public  async new Task<WebApiCallBack> DeleteByIdAsync(object id)
        {
            var jm = new WebApiCallBack();

            var bl = await DbClient.Deleteable<finance_record>(id).ExecuteCommandHasChangeAsync();
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
        public  async new Task<WebApiCallBack> DeleteByIdsAsync(int[] ids)
        {
            var jm = new WebApiCallBack();

            var bl = await DbClient.Deleteable<finance_record>().In(ids).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            //if (bl)
            //{
            //    await UpdateCaChe();
            //}

            return jm;
        }

        ///// <summary>
        ///// 隐藏指定ID的数据
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public new async new Task<WebApiCallBack> HideByIdAsync(object id)
        //{
        //    var jm = new WebApiCallBack();

        //    var bl = await DbClient.Updateable<Record>().SetColumns(p => p.dstate == true).Where(p => p.id == Convert.ToInt32(id)).ExecuteCommandHasChangeAsync();
        //    jm.code = bl ? 0 : 1;
        //    jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
        //    //if (bl)
        //    //{
        //    //    await UpdateCaChe();
        //    //}

        //    return jm;
        //}


        #endregion

        #region 获取缓存的所有数据==========================================================

        ///// <summary>
        ///// 获取缓存的所有数据
        ///// </summary>
        ///// <returns></returns>
        //public async new Task<List<Record>> GetCaChe()
        //{
        //    var cache = ManualDataCache.Instance.Get<List<Record>>(GlobalConstVars.CacheFinanceRecord);
        //    if (cache != null)
        //    {
        //        return cache;
        //    }
        //    return await UpdateCaChe();
        //}

        ///// <summary>
        /////     更新cache
        ///// </summary>
        //public async new Task<List<Record>> UpdateCaChe()
        //{
        //    var list = await DbClient.Queryable<Record>().With(SqlWith.NoLock).ToListAsync();
        //    ManualDataCache.Instance.Set(GlobalConstVars.CacheFinanceRecord, list);
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
        public  async new Task<IPageList<finance_record>> QueryPageAsync(Expression<Func<finance_record, bool>> predicate,
            Expression<Func<finance_record, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<finance_record> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<finance_record>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new finance_record
                {
                      id = p.id,
                perid = p.perid,
                testid = p.testid,
                barcode = p.barcode,
                operatType = p.operatType,
                record = p.record,
                reason = p.reason,
                operater = p.operater,
                    createTime = p.createTime,
                clientShow = p.clientShow,
                
                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<finance_record>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new finance_record
                {
                      id = p.id,
                perid = p.perid,
                testid = p.testid,
                barcode = p.barcode,
                operatType = p.operatType,
                record = p.record,
                reason = p.reason,
                operater = p.operater,
                    createTime = p.createTime,
                clientShow = p.clientShow,
                
                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<finance_record>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
