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
using Yichen.Report.Model.table;
using Yichen.Report.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;

namespace Yichen.Report.Repository
{
    /// <summary>
    ///  接口实现
    /// </summary>
    public class BindClientInfoRepository : BaseRepository<BindClientInfo>, IBindClientInfoRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public BindClientInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> InsertAsync(BindClientInfo entity)
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
        public  async Task<WebApiCallBack> UpdateAsync(BindClientInfo entity)
        {
            var jm = new WebApiCallBack();

            var oldModel = await DbClient.Queryable<BindClientInfo>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
            jm.msg = "不存在此信息";
            return jm;
            }
            //事物处理过程开始
        	oldModel.id = entity.id;
            oldModel.no = entity.no;
            oldModel.names = entity.names;
            oldModel.workNO = entity.workNO;
            oldModel.groupNO = entity.groupNO;
            oldModel.groupCodes = entity.groupCodes;
            oldModel.clientNO = entity.clientNO;
            oldModel.paperKindNO = entity.paperKindNO;
            oldModel.reportLandscape = entity.reportLandscape;
            oldModel.reportTileNO = entity.reportTileNO;
            oldModel.reportInfoNO = entity.reportInfoNO;
            oldModel.reportViewNO = entity.reportViewNO;
            oldModel.reportOtherNO = entity.reportOtherNO;
            oldModel.remark = entity.remark;
            oldModel.createTime = entity.createTime;
            oldModel.state = entity.state;
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
        public  async Task<WebApiCallBack> UpdateAsync(List<BindClientInfo> entity)
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

            var bl = await DbClient.Deleteable<BindClientInfo>(id).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Deleteable<BindClientInfo>().In(ids).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Updateable<BindClientInfo>().SetColumns(p => p.dstate == true).Where(p => p.id == Convert.ToInt32(id)).ExecuteCommandHasChangeAsync();
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
        public async Task<List<BindClientInfo>> GetCaChe()
        {
            var cache = ManualDataCache.Instance.Get<List<BindClientInfo>>(GlobalConstVars.CacheBindClientInfo);
            if (cache != null)
            {
                return cache;
            }
            return await UpdateCaChe();
        }

        /// <summary>
        ///     更新cache
        /// </summary>
        public async Task<List<BindClientInfo>> UpdateCaChe()
        {
            var list = await DbClient.Queryable<BindClientInfo>().With(SqlWith.NoLock).ToListAsync();
            ManualDataCache.Instance.Set(GlobalConstVars.CacheBindClientInfo, list);
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
        public  async Task<IPageList<BindClientInfo>> QueryPageAsync(Expression<Func<BindClientInfo, bool>> predicate,
            Expression<Func<BindClientInfo, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<BindClientInfo> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<BindClientInfo>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new BindClientInfo
                {
                      id = p.id,
                no = p.no,
                names = p.names,
                workNO = p.workNO,
                groupNO = p.groupNO,
                groupCodes = p.groupCodes,
                clientNO = p.clientNO,
                paperKindNO = p.paperKindNO,
                reportLandscape = p.reportLandscape,
                reportTileNO = p.reportTileNO,
                reportInfoNO = p.reportInfoNO,
                reportViewNO = p.reportViewNO,
                reportOtherNO = p.reportOtherNO,
                remark = p.remark,
                createTime = p.createTime,
                state = p.state,
                dstate = p.dstate,
                
                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<BindClientInfo>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new BindClientInfo
                {
                      id = p.id,
                no = p.no,
                names = p.names,
                workNO = p.workNO,
                groupNO = p.groupNO,
                groupCodes = p.groupCodes,
                clientNO = p.clientNO,
                paperKindNO = p.paperKindNO,
                reportLandscape = p.reportLandscape,
                reportTileNO = p.reportTileNO,
                reportInfoNO = p.reportInfoNO,
                reportViewNO = p.reportViewNO,
                reportOtherNO = p.reportOtherNO,
                remark = p.remark,
                createTime = p.createTime,
                state = p.state,
                dstate = p.dstate,
                
                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<BindClientInfo>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
