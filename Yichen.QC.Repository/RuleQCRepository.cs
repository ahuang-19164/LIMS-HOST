/***********************************************************************
 *            Project: Yichen
 *        ProjectName: 屹辰智禾管理系统                                
 *                Web: https://www.zui51.com                 
 *             Author: 屹辰                                       
 *              Email: 499715561@qq.com                              
 *         CreateTime: 
 *        Description: 暂无
 ***********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Yichen.Net.Caching.Manual;
using Yichen.Net.Configuration;
using Yichen.QC.Model;
using Yichen.QC.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Repository;

using Yichen.Comm.Model.ViewModels.Basics;
using Yichen.Comm.Model.ViewModels.UI;
using SqlSugar;

namespace Yichen.QC.Repository
{
    /// <summary>
    ///  接口实现
    /// </summary>
    public class RuleQCRepository : BaseRepository<RuleQC>, IRuleQCRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public RuleQCRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> InsertAsync(RuleQC entity)
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
        public  async Task<WebApiCallBack> UpdateAsync(RuleQC entity)
        {
            var jm = new WebApiCallBack();

            var oldModel = await DbClient.Queryable<RuleQC>().In(entity.id).SingleAsync();
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
            oldModel.classNO = entity.classNO;
            oldModel.explain = entity.explain;
            oldModel.nValue = entity.nValue;
            oldModel.mValue = entity.mValue;
            oldModel.xValue = entity.xValue;
            oldModel.yValue = entity.yValue;
            oldModel.rangeNO = entity.rangeNO;
            oldModel.errorNO = entity.errorNO;
            oldModel.criteriaNO = entity.criteriaNO;
            oldModel.remark = entity.remark;
            oldModel.state = entity.state;
            oldModel.dstate = entity.dstate;
            oldModel.sort = entity.sort;
            oldModel.creater = entity.creater;
            oldModel.createTime = entity.createTime;
            
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
        public  async Task<WebApiCallBack> UpdateAsync(List<RuleQC> entity)
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

            var bl = await DbClient.Deleteable<RuleQC>(id).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Deleteable<RuleQC>().In(ids).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Updateable<RuleQC>().SetColumns(p => p.dstate == true).Where(p => p.id == Convert.ToInt32(id)).ExecuteCommandHasChangeAsync();
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
        public async Task<List<RuleQC>> GetCaChe()
        {
            var cache = ManualDataCache.Instance.Get<List<RuleQC>>(GlobalConstVars.CacheRuleQC);
            if (cache != null)
            {
                return cache;
            }
            return await UpdateCaChe();
        }

        /// <summary>
        ///     更新cache
        /// </summary>
        public async Task<List<RuleQC>> UpdateCaChe()
        {
            var list = await DbClient.Queryable<RuleQC>().With(SqlWith.NoLock).ToListAsync();
            ManualDataCache.Instance.Set(GlobalConstVars.CacheRuleQC, list);
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
        public  async Task<IPageList<RuleQC>> QueryPageAsync(Expression<Func<RuleQC, bool>> predicate,
            Expression<Func<RuleQC, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<RuleQC> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<RuleQC>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new RuleQC
                {
                      id = p.id,
                no = p.no,
                names = p.names,
                shortNames = p.shortNames,
                customCode = p.customCode,
                classNO = p.classNO,
                explain = p.explain,
                nValue = p.nValue,
                mValue = p.mValue,
                xValue = p.xValue,
                yValue = p.yValue,
                rangeNO = p.rangeNO,
                errorNO = p.errorNO,
                criteriaNO = p.criteriaNO,
                remark = p.remark,
                state = p.state,
                dstate = p.dstate,
                sort = p.sort,
                creater = p.creater,
                createTime = p.createTime,
                
                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<RuleQC>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new RuleQC
                {
                      id = p.id,
                no = p.no,
                names = p.names,
                shortNames = p.shortNames,
                customCode = p.customCode,
                classNO = p.classNO,
                explain = p.explain,
                nValue = p.nValue,
                mValue = p.mValue,
                xValue = p.xValue,
                yValue = p.yValue,
                rangeNO = p.rangeNO,
                errorNO = p.errorNO,
                criteriaNO = p.criteriaNO,
                remark = p.remark,
                state = p.state,
                dstate = p.dstate,
                sort = p.sort,
                creater = p.creater,
                createTime = p.createTime,
                
                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<RuleQC>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
