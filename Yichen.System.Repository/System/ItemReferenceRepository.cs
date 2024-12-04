/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.Yichen.Net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2023/10/30 21:18:24
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

using static SKIT.FlurlHttpClient.Wechat.Api.Models.CardCreateRequest.Types.GrouponCard.Types.Base.Types;
using System.Diagnostics.Metrics;

namespace Yichen.System.Repository
{
    /// <summary>
    /// 用户课程范围 接口实现
    /// </summary>
    public class ItemReferenceRepository : BaseRepository<comm_item_reference>, IItemReferenceRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public ItemReferenceRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> InsertAsync(comm_item_reference entity)
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
        public  async Task<WebApiCallBack> UpdateAsync(comm_item_reference entity)
        {
            var jm = new WebApiCallBack();

            var oldModel = await DbClient.Queryable<comm_item_reference>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            //事物处理过程开始
            oldModel.id = entity.id;
            oldModel.testNO = entity.testNO;
            oldModel.clientNO = entity.clientNO;
            oldModel.instrumentNO = entity.instrumentNO;
            oldModel.sampeTypeNO = entity.sampeTypeNO;
            oldModel.sexNO = entity.sexNO;
            oldModel.ageYearDown = entity.ageYearDown;
            oldModel.ageYearUP = entity.ageYearUP;
            oldModel.ageMothDown = entity.ageMothDown;
            oldModel.ageMothUP = entity.ageMothUP;
            oldModel.ageDayDown = entity.ageDayDown;
            oldModel.ageDayUP = entity.ageDayUP;
            oldModel.valueDown = entity.valueDown;
            oldModel.valueUP = entity.valueUP;
            oldModel.valueDescribe = entity.valueDescribe;
            oldModel.crisisDown = entity.crisisDown;
            oldModel.crisisUP = entity.crisisUP;
            oldModel.state = entity.state;
            oldModel.remark = entity.remark;
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
        public  async Task<WebApiCallBack> UpdateAsync(List<comm_item_reference> entity)
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

            var bl = await DbClient.Deleteable<comm_item_reference>(id).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Deleteable<comm_item_reference>().In(ids).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Updateable<comm_item_reference>().SetColumns(p => p.dstate == true).Where(p => p.id == Convert.ToInt32(id)).ExecuteCommandHasChangeAsync();
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
        public async Task<List<comm_item_reference>> GetCaChe()
        {
            var cache = ManualDataCache.Instance.Get<List<comm_item_reference>>(GlobalConstVars.comm_item_reference);
            if (cache != null)
            {
                return cache;
            }
            return await UpdateCaChe();
        }

        /// <summary>
        ///     更新cache
        /// </summary>
        public async Task<List<comm_item_reference>> UpdateCaChe()
        {
            var list = await DbClient.Queryable<comm_item_reference>().With(SqlWith.NoLock).ToListAsync();
            ManualDataCache.Instance.Set(GlobalConstVars.comm_item_reference, list);
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
        public  async Task<IPageList<comm_item_reference>> QueryPageAsync(Expression<Func<comm_item_reference, bool>> predicate,
            Expression<Func<comm_item_reference, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<comm_item_reference> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<comm_item_reference>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new comm_item_reference
                {
                    id = p.id,
                    testNO = p.testNO,
                    clientNO = p.clientNO,
                    instrumentNO = p.instrumentNO,
                    sampeTypeNO = p.sampeTypeNO,
                    sexNO = p.sexNO,
                    ageYearDown = p.ageYearDown,
                    ageYearUP = p.ageYearUP,
                    ageMothDown = p.ageMothDown,
                    ageMothUP = p.ageMothUP,
                    ageDayDown = p.ageDayDown,
                    ageDayUP = p.ageDayUP,
                    valueDown = p.valueDown,
                    valueUP = p.valueUP,
                    valueDescribe = p.valueDescribe,
                    crisisDown = p.crisisDown,
                    crisisUP = p.crisisUP,
                    state = p.state,
                    remark = p.remark,
                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<comm_item_reference>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new comm_item_reference
                {
                    id = p.id,
                    testNO = p.testNO,
                    clientNO = p.clientNO,
                    instrumentNO = p.instrumentNO,
                    sampeTypeNO = p.sampeTypeNO,
                    sexNO = p.sexNO,
                    ageYearDown = p.ageYearDown,
                    ageYearUP = p.ageYearUP,
                    ageMothDown = p.ageMothDown,
                    ageMothUP = p.ageMothUP,
                    ageDayDown = p.ageDayDown,
                    ageDayUP = p.ageDayUP,
                    valueDown = p.valueDown,
                    valueUP = p.valueUP,
                    valueDescribe = p.valueDescribe,
                    crisisDown = p.crisisDown,
                    crisisUP = p.crisisUP,
                    state = p.state,
                    remark = p.remark,

                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<comm_item_reference>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
