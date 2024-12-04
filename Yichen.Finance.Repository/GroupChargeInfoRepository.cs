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
    public class GroupChargeInfoRepository : BaseRepository<finance_group_charge>, IGroupChargeInfoRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public GroupChargeInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public  async new Task<WebApiCallBack> InsertAsync(finance_group_charge entity)
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
        public  async new Task<WebApiCallBack> UpdateAsync(finance_group_charge entity)
        {
            var jm = new WebApiCallBack();

            var oldModel = await DbClient.Queryable<finance_group_charge>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
            jm.msg = "不存在此信息";
            return jm;
            }
            //事物处理过程开始
        	oldModel.id = entity.id;
            oldModel.agentNO = entity.agentNO;
            oldModel.hospitalNO = entity.hospitalNO;
            oldModel.patientTypeNO = entity.patientTypeNO;
            oldModel.department = entity.department;
            oldModel.groupCode = entity.groupCode;
            oldModel.chargeLevelNO = entity.chargeLevelNO;
            oldModel.standardCharge = entity.standardCharge;
            oldModel.settlementCharge = entity.settlementCharge;
            oldModel.discountState = entity.discountState;
            oldModel.startTime = entity.startTime;
            oldModel.endTime = entity.endTime;
            oldModel.creater = entity.creater;
            oldModel.createTime = entity.createTime;
            oldModel.state = entity.state;
            
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
        public  async new Task<WebApiCallBack> UpdateAsync(List<finance_group_charge> entity)
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
        public  async new Task<WebApiCallBack> DeleteByIdAsync(object id)
        {
            var jm = new WebApiCallBack();

            var bl = await DbClient.Deleteable<finance_group_charge>(id).ExecuteCommandHasChangeAsync();
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
        public  async new Task<WebApiCallBack> DeleteByIdsAsync(int[] ids)
        {
            var jm = new WebApiCallBack();

            var bl = await DbClient.Deleteable<finance_group_charge>().In(ids).ExecuteCommandHasChangeAsync();
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
        //public new async new Task<WebApiCallBack> HideByIdAsync(object id)
        //{
        //    var jm = new WebApiCallBack();

        //    var bl = await DbClient.Updateable<finance_group_charge>().SetColumns(p => p.dstate == true).Where(p => p.id == Convert.ToInt32(id)).ExecuteCommandHasChangeAsync();
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
        public async new Task<List<finance_group_charge>> GetCaChe()
        {
            var cache = ManualDataCache.Instance.Get<List<finance_group_charge>>(GlobalConstVars.CacheGroupChargeInfo);
            if (cache != null)
            {
                return cache;
            }
            return await UpdateCaChe();
        }

        /// <summary>
        ///     更新cache
        /// </summary>
        public async new Task<List<finance_group_charge>> UpdateCaChe()
        {
            var list = await DbClient.Queryable<finance_group_charge>().With(SqlWith.NoLock).ToListAsync();
            ManualDataCache.Instance.Set(GlobalConstVars.CacheGroupChargeInfo, list);
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
        public  async new Task<IPageList<finance_group_charge>> QueryPageAsync(Expression<Func<finance_group_charge, bool>> predicate,
            Expression<Func<finance_group_charge, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<finance_group_charge> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<finance_group_charge>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new finance_group_charge
                {
                      id = p.id,
                agentNO = p.agentNO,
                hospitalNO = p.hospitalNO,
                patientTypeNO = p.patientTypeNO,
                department = p.department,
                groupCode = p.groupCode,
                chargeLevelNO = p.chargeLevelNO,
                standardCharge = p.standardCharge,
                settlementCharge = p.settlementCharge,
                discountState = p.discountState,
                startTime = p.startTime,
                endTime = p.endTime,
                creater = p.creater,
                createTime = p.createTime,
                state = p.state,
                
                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<finance_group_charge>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new finance_group_charge
                {
                      id = p.id,
                agentNO = p.agentNO,
                hospitalNO = p.hospitalNO,
                patientTypeNO = p.patientTypeNO,
                department = p.department,
                groupCode = p.groupCode,
                chargeLevelNO = p.chargeLevelNO,
                standardCharge = p.standardCharge,
                settlementCharge = p.settlementCharge,
                discountState = p.discountState,
                startTime = p.startTime,
                endTime = p.endTime,
                creater = p.creater,
                createTime = p.createTime,
                state = p.state,
                
                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<finance_group_charge>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
