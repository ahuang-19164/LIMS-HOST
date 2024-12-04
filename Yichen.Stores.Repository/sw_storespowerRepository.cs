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
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Stores.Model;
using Yichen.Stores.IRepository;
using Yichen.Comm.Model;

namespace Yichen.Stores.Repository
{
    /// <summary>
    /// 存储库权限列表 接口实现
    /// </summary>
    public class sw_storespowerRepository : BaseRepository<sw_storespower>, Isw_storespowerRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public sw_storespowerRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> InsertAsync(sw_storespower entity)
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
        public  async Task<WebApiCallBack> UpdateAsync(sw_storespower entity)
        {
            var jm = new WebApiCallBack();

            var oldModel = await DbClient.Queryable<sw_storespower>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
            jm.msg = "不存在此信息";
            return jm;
            }
            //事物处理过程开始
        	oldModel.id = entity.id;
            oldModel.userNo = entity.userNo;
            oldModel.storesid = entity.storesid;
            oldModel.state = entity.state;
            oldModel.createShelf = entity.createShelf;
            oldModel.editShelf = entity.editShelf;
            oldModel.entrySample = entity.entrySample;
            oldModel.editSample = entity.editSample;
            oldModel.delsample = entity.delsample;
            oldModel.handleSample = entity.handleSample;
            oldModel.rehandleSample = entity.rehandleSample;
            oldModel.searchSample = entity.searchSample;
            oldModel.cancelSample = entity.cancelSample;
            
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
        public  async Task<WebApiCallBack> UpdateAsync(List<sw_storespower> entity)
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

            var bl = await DbClient.Deleteable<sw_storespower>(id).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Deleteable<sw_storespower>().In(ids).ExecuteCommandHasChangeAsync();
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

        //    var bl = await DbClient.Updateable<sw_storespower>().SetColumns(p => p.dstate == true).Where(p => p.id == Convert.ToInt32(id)).ExecuteCommandHasChangeAsync();
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
        public async Task<List<sw_storespower>> GetCaChe()
        {
            var cache = ManualDataCache.Instance.Get<List<sw_storespower>>(GlobalConstVars.Cachesw_storespower);
            if (cache != null)
            {
                return cache;
            }
            return await UpdateCaChe();
        }

        /// <summary>
        ///     更新cache
        /// </summary>
        public async Task<List<sw_storespower>> UpdateCaChe()
        {
            var list = await DbClient.Queryable<sw_storespower>().With(SqlWith.NoLock).ToListAsync();
            ManualDataCache.Instance.Set(GlobalConstVars.Cachesw_storespower, list);
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
        public  async Task<IPageList<sw_storespower>> QueryPageAsync(Expression<Func<sw_storespower, bool>> predicate,
            Expression<Func<sw_storespower, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<sw_storespower> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<sw_storespower>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new sw_storespower
                {
                      id = p.id,
                userNo = p.userNo,
                storesid = p.storesid,
                state = p.state,
                createShelf = p.createShelf,
                editShelf = p.editShelf,
                entrySample = p.entrySample,
                editSample = p.editSample,
                delsample = p.delsample,
                handleSample = p.handleSample,
                rehandleSample = p.rehandleSample,
                searchSample = p.searchSample,
                cancelSample = p.cancelSample,
                
                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<sw_storespower>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new sw_storespower
                {
                      id = p.id,
                userNo = p.userNo,
                storesid = p.storesid,
                state = p.state,
                createShelf = p.createShelf,
                editShelf = p.editShelf,
                entrySample = p.entrySample,
                editSample = p.editSample,
                delsample = p.delsample,
                handleSample = p.handleSample,
                rehandleSample = p.rehandleSample,
                searchSample = p.searchSample,
                cancelSample = p.cancelSample,
                
                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<sw_storespower>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
