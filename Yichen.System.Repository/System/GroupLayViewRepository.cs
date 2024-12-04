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
    public class GroupLayViewRepository : BaseRepository<GroupLayView>, IGroupLayViewRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public GroupLayViewRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> InsertAsync(GroupLayView entity)
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
        public  async Task<WebApiCallBack> UpdateAsync(GroupLayView entity)
        {
            var jm = new WebApiCallBack();

            var oldModel = await DbClient.Queryable<GroupLayView>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
            jm.msg = "不存在此信息";
            return jm;
            }
            //事物处理过程开始
        	oldModel.id = entity.id;
            oldModel.workNO = entity.workNO;
            oldModel.testNO = entity.testNO;
            oldModel.controlType = entity.controlType;
            oldModel.controlNames = entity.controlNames;
            oldModel.fieldNames = entity.fieldNames;
            oldModel.captions = entity.captions;
            oldModel.controlVisible = entity.controlVisible;
            oldModel.controlEnabled = entity.controlEnabled;
            oldModel.allFocus = entity.allFocus;
            oldModel.allowEdit = entity.allowEdit;
            oldModel.readOnly = entity.readOnly;
            oldModel.width = entity.width;
            oldModel.bandTable = entity.bandTable;
            oldModel.bandType = entity.bandType;
            oldModel.sort = entity.sort;
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
        public  async Task<WebApiCallBack> UpdateAsync(List<GroupLayView> entity)
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

            var bl = await DbClient.Deleteable<GroupLayView>(id).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Deleteable<GroupLayView>().In(ids).ExecuteCommandHasChangeAsync();
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

        //    var bl = await DbClient.Updateable<GroupLayView>().SetColumns(p => p.dstate == true).Where(p => p.id == Convert.ToInt32(id)).ExecuteCommandHasChangeAsync();
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
        public async Task<List<GroupLayView>> GetCaChe()
        {
            var cache = ManualDataCache.Instance.Get<List<GroupLayView>>(GlobalConstVars.CacheGroupLayView);
            if (cache != null)
            {
                return cache;
            }
            return await UpdateCaChe();
        }

        /// <summary>
        ///     更新cache
        /// </summary>
        public async Task<List<GroupLayView>> UpdateCaChe()
        {
            var list = await DbClient.Queryable<GroupLayView>().With(SqlWith.NoLock).ToListAsync();
            ManualDataCache.Instance.Set(GlobalConstVars.CacheGroupLayView, list);
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
        public  async Task<IPageList<GroupLayView>> QueryPageAsync(Expression<Func<GroupLayView, bool>> predicate,
            Expression<Func<GroupLayView, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<GroupLayView> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<GroupLayView>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new GroupLayView
                {
                      id = p.id,
                workNO = p.workNO,
                testNO = p.testNO,
                controlType = p.controlType,
                controlNames = p.controlNames,
                fieldNames = p.fieldNames,
                captions = p.captions,
                controlVisible = p.controlVisible,
                controlEnabled = p.controlEnabled,
                allFocus = p.allFocus,
                allowEdit = p.allowEdit,
                readOnly = p.readOnly,
                width = p.width,
                bandTable = p.bandTable,
                bandType = p.bandType,
                sort = p.sort,
                state = p.state,
                
                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<GroupLayView>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new GroupLayView
                {
                      id = p.id,
                workNO = p.workNO,
                testNO = p.testNO,
                controlType = p.controlType,
                controlNames = p.controlNames,
                fieldNames = p.fieldNames,
                captions = p.captions,
                controlVisible = p.controlVisible,
                controlEnabled = p.controlEnabled,
                allFocus = p.allFocus,
                allowEdit = p.allowEdit,
                readOnly = p.readOnly,
                width = p.width,
                bandTable = p.bandTable,
                bandType = p.bandType,
                sort = p.sort,
                state = p.state,
                
                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<GroupLayView>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
