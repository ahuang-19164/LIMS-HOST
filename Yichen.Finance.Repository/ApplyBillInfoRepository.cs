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
using Yichen.Finance.Model.table;
using Yichen.Finance.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;

namespace Yichen.Finance.Repository
{
    /// <summary>
    ///  接口实现
    /// </summary>
    public class ApplyBillInfoRepository : BaseRepository<ApplyBillInfo>, IApplyBillInfoRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public ApplyBillInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 生成标本收费记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async new Task<int> InsertApplyBill(ApplyBillInfo pairs)
        {
            return await DbClient.Insertable<ApplyBillInfo>(pairs).ExecuteCommandAsync();
        }


       #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public  async new Task<WebApiCallBack> InsertAsync(ApplyBillInfo entity)
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
        public  async new Task<WebApiCallBack> UpdateAsync(ApplyBillInfo entity)
        {
            var jm = new WebApiCallBack();

            var oldModel = await DbClient.Queryable<ApplyBillInfo>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
            jm.msg = "不存在此信息";
            return jm;
            }
            //事物处理过程开始
        	oldModel.id = entity.id;
            oldModel.perid = entity.perid;
            oldModel.barcode = entity.barcode;
            oldModel.chargeLevel = entity.chargeLevel;
            oldModel.chargeTypeNO = entity.chargeTypeNO;
            oldModel.standerCharge = entity.standerCharge;
            oldModel.settlementCharge = entity.settlementCharge;
            oldModel.charge = entity.charge;
            oldModel.discount = entity.discount;
            oldModel.personNO = entity.personNO;
            oldModel.operater = entity.operater;
            oldModel.operatTime = entity.operatTime;
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
        public  async new Task<WebApiCallBack> UpdateAsync(List<ApplyBillInfo> entity)
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

            var bl = await DbClient.Deleteable<ApplyBillInfo>(id).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Deleteable<ApplyBillInfo>().In(ids).ExecuteCommandHasChangeAsync();
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
        public  async new Task<WebApiCallBack> HideByIdAsync(object id)
        {
            var jm = new WebApiCallBack();

            var bl = await DbClient.Updateable<ApplyBillInfo>().SetColumns(p => p.dstate == true).Where(p => p.id == Convert.ToInt32(id)).ExecuteCommandHasChangeAsync();
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
        //public async new Task<List<ApplyBillInfo>> GetCaChe()
        //{
        //    var cache = ManualDataCache.Instance.Get<List<ApplyBillInfo>>(GlobalConstVars.CacheApplyBillInfo);
        //    if (cache != null)
        //    {
        //        return cache;
        //    }
        //    return await UpdateCaChe();
        //}

        ///// <summary>
        /////     更新cache
        ///// </summary>
        //public async new Task<List<ApplyBillInfo>> UpdateCaChe()
        //{
        //    var list = await DbClient.Queryable<ApplyBillInfo>().With(SqlWith.NoLock).ToListAsync();
        //    ManualDataCache.Instance.Set(GlobalConstVars.CacheApplyBillInfo, list);
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
        public  async new Task<IPageList<ApplyBillInfo>> QueryPageAsync(Expression<Func<ApplyBillInfo, bool>> predicate,
            Expression<Func<ApplyBillInfo, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<ApplyBillInfo> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<ApplyBillInfo>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new ApplyBillInfo
                {
                      id = p.id,
                perid = p.perid,
                barcode = p.barcode,
                chargeLevel = p.chargeLevel,
                chargeTypeNO = p.chargeTypeNO,
                standerCharge = p.standerCharge,
                settlementCharge = p.settlementCharge,
                charge = p.charge,
                discount = p.discount,
                personNO = p.personNO,
                operater = p.operater,
                operatTime = p.operatTime,
                state = p.state,
                dstate = p.dstate,
                
                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<ApplyBillInfo>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new ApplyBillInfo
                {
                      id = p.id,
                perid = p.perid,
                barcode = p.barcode,
                chargeLevel = p.chargeLevel,
                chargeTypeNO = p.chargeTypeNO,
                standerCharge = p.standerCharge,
                settlementCharge = p.settlementCharge,
                charge = p.charge,
                discount = p.discount,
                personNO = p.personNO,
                operater = p.operater,
                operatTime = p.operatTime,
                state = p.state,
                dstate = p.dstate,
                
                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<ApplyBillInfo>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
