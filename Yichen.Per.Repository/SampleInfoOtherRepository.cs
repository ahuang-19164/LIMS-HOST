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
using Yichen.Net.Configuration;
using Yichen.Comm.Model.ViewModels.Basics;
using Yichen.Comm.Model.ViewModels.UI;
using SqlSugar;
using Yichen.Comm.Repository;
using Yichen.Per.Model.table;
using Yichen.Per.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Net.Caching.Manual;

namespace Yichen.Per.Repository
{
    /// <summary>
    ///  接口实现
    /// </summary>
    public class SampleInfoOtherRepository : BaseRepository<SampleInfoOther>, ISampleInfoOtherRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SampleInfoOtherRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 根据录入id查询录入其他信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task<SampleInfoDelete> GetInfoByPerid(int perid)
        {
            return await DbClient.Queryable<SampleInfoDelete>().FirstAsync(p => p.perid == Convert.ToInt32(perid));
        }

        /// <summary>
        /// 插入录入标本信息,返回插入信息id
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public  async Task<int> EntryOtherInfo(Dictionary<string, object> info)
        {
            return await DbClient.Insertable<SampleInfoOther>(info).ExecuteCommandAsync();
        }
        /// <summary>
        /// 修改插入录入标本信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public  async Task<int> EntryOteherInfoEdit(int perid, Dictionary<string, object> info)
        {
            return DbClient.Updateable(info).AS("WorkPer.SampleInfoOther").Where($"perid={perid}").ExecuteCommand();
        }

        #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> InsertAsync(SampleInfoOther entity)
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
        public  async Task<WebApiCallBack> UpdateAsync(SampleInfoOther entity)
        {
            var jm = new WebApiCallBack();

            var oldModel = await DbClient.Queryable<SampleInfoOther>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
            jm.msg = "不存在此信息";
            return jm;
            }
            //事物处理过程开始
        	oldModel.id = entity.id;
            oldModel.barcode = entity.barcode;
            
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
        public  async Task<WebApiCallBack> UpdateAsync(List<SampleInfoOther> entity)
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

            var bl = await DbClient.Deleteable<SampleInfoOther>(id).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Deleteable<SampleInfoOther>().In(ids).ExecuteCommandHasChangeAsync();
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

        //    var bl = await DbClient.Updateable<SampleInfoOther>().SetColumns(p => p.dstate == true).Where(p => p.id == Convert.ToInt32(id)).ExecuteCommandHasChangeAsync();
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
        //public async Task<List<SampleInfoOther>> GetCaChe()
        //{
        //    var cache = ManualDataCache.Instance.Get<List<SampleInfoOther>>(GlobalConstVars.CacheSampleInfoOther);
        //    if (cache != null)
        //    {
        //        return cache;
        //    }
        //    return await UpdateCaChe();
        //}

        ///// <summary>
        /////     更新cache
        ///// </summary>
        //public async Task<List<SampleInfoOther>> UpdateCaChe()
        //{
        //    var list = await DbClient.Queryable<SampleInfoOther>().With(SqlWith.NoLock).ToListAsync();
        //    ManualDataCache.Instance.Set(GlobalConstVars.CacheSampleInfoOther, list);
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
        public  async Task<IPageList<SampleInfoOther>> QueryPageAsync(Expression<Func<SampleInfoOther, bool>> predicate,
            Expression<Func<SampleInfoOther, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<SampleInfoOther> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<SampleInfoOther>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new SampleInfoOther
                {
                      id = p.id,
                barcode = p.barcode,
                
                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<SampleInfoOther>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new SampleInfoOther
                {
                      id = p.id,
                barcode = p.barcode,
                
                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<SampleInfoOther>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
