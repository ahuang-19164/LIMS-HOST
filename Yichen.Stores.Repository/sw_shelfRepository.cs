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
    /// 标本架信息 接口实现
    /// </summary>
    public class sw_shelfRepository : BaseRepository<sw_shelf>, Isw_shelfRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public sw_shelfRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> InsertAsync(sw_shelf entity)
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
        public  async Task<WebApiCallBack> UpdateAsync(sw_shelf entity)
        {
            var jm = new WebApiCallBack();

            var oldModel = await DbClient.Queryable<sw_shelf>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
            jm.msg = "不存在此信息";
            return jm;
            }
            //事物处理过程开始
        	oldModel.id = entity.id;
            oldModel.no = entity.no;
            oldModel.names = entity.names;
            oldModel.shortNames = entity.shortNames;
            oldModel.customCode = entity.customCode;
            oldModel.storesNO = entity.storesNO;
            oldModel.saveNo = entity.saveNo;
            oldModel.saveDay = entity.saveDay;
            oldModel.shelfRow = entity.shelfRow;
            oldModel.shelfCell = entity.shelfCell;
            oldModel.shelfTypeNO = entity.shelfTypeNO;
            oldModel.state = entity.state;
            oldModel.remark = entity.remark;
            oldModel.sampleCount = entity.sampleCount;
            oldModel.createTime = entity.createTime;
            oldModel.sort = entity.sort;
            
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
        public  async Task<WebApiCallBack> UpdateAsync(List<sw_shelf> entity)
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

            var bl = await DbClient.Deleteable<sw_shelf>(id).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Deleteable<sw_shelf>().In(ids).ExecuteCommandHasChangeAsync();
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
        //public new async Task<WebApiCallBack> HideByIdAsync(object id)
        //{
        //    var jm = new WebApiCallBack();

        //    var bl = await DbClient.Updateable<sw_shelf>().SetColumns(p => p.dstate == true).Where(p => p.id == Convert.ToInt32(id)).ExecuteCommandHasChangeAsync();
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

        ///// <summary>
        ///// 获取缓存的所有数据
        ///// </summary>
        ///// <returns></returns>
        //public async Task<List<sw_shelf>> GetCaChe()
        //{
        //    var cache = ManualDataCache.Instance.Get<List<sw_shelf>>(GlobalConstVars.Cachesw_shelf);
        //    if (cache != null)
        //    {
        //        return cache;
        //    }
        //    return await UpdateCaChe();
        //}

        ///// <summary>
        /////     更新cache
        ///// </summary>
        //public async Task<List<sw_shelf>> UpdateCaChe()
        //{
        //    var list = await DbClient.Queryable<sw_shelf>().With(SqlWith.NoLock).ToListAsync();
        //    ManualDataCache.Instance.Set(GlobalConstVars.Cachesw_shelf, list);
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
        public  async Task<IPageList<sw_shelf>> QueryPageAsync(Expression<Func<sw_shelf, bool>> predicate,
            Expression<Func<sw_shelf, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<sw_shelf> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<sw_shelf>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new sw_shelf
                {
                      id = p.id,
                no = p.no,
                names = p.names,
                shortNames = p.shortNames,
                customCode = p.customCode,
                storesNO = p.storesNO,
                saveNo = p.saveNo,
                saveDay = p.saveDay,
                shelfRow = p.shelfRow,
                shelfCell = p.shelfCell,
                shelfTypeNO = p.shelfTypeNO,
                state = p.state,
                remark = p.remark,
                sampleCount = p.sampleCount,
                createTime = p.createTime,
                sort = p.sort,
                
                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<sw_shelf>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new sw_shelf
                {
                      id = p.id,
                no = p.no,
                names = p.names,
                shortNames = p.shortNames,
                customCode = p.customCode,
                storesNO = p.storesNO,
                saveNo = p.saveNo,
                saveDay = p.saveDay,
                shelfRow = p.shelfRow,
                shelfCell = p.shelfCell,
                shelfTypeNO = p.shelfTypeNO,
                state = p.state,
                remark = p.remark,
                sampleCount = p.sampleCount,
                createTime = p.createTime,
                sort = p.sort,
                
                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<sw_shelf>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
