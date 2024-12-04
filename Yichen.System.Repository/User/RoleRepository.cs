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

namespace Yichen.System.Repository
{
    /// <summary>
    /// 用户课程范围 接口实现
    /// </summary>
    public class RoleRepository : BaseRepository<sys_role>, IRoleRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 获取用户角色信息
        /// </summary>
        /// <param name="roleNo"></param>
        /// <returns></returns>
        public  async Task<sys_role> GetUserRole(int roleNo)
        {
            var oldModel = await DbClient.Queryable<sys_role>().Where(p => p.no == roleNo).FirstAsync();
            return oldModel;
        }












        #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> InsertAsync(sys_role entity)
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
        public  async Task<WebApiCallBack> UpdateAsync(sys_role entity)
        {
            var jm = new WebApiCallBack();

            var oldModel = await DbClient.Queryable<sys_role>().In(entity.id).SingleAsync();
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
            oldModel.companyNO = entity.companyNO;
            oldModel.departmentNO = entity.departmentNO;
            oldModel.phone = entity.phone;
            oldModel.fax = entity.fax;
            oldModel.functions = entity.functions;
            oldModel.sort = entity.sort;
            oldModel.powerList = entity.powerList;
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
        public  async Task<WebApiCallBack> UpdateAsync(List<sys_role> entity)
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

            var bl = await DbClient.Deleteable<sys_role>(id).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Deleteable<sys_role>().In(ids).ExecuteCommandHasChangeAsync();
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
        public async Task<List<sys_role>> GetCaChe()
        {
            var cache = ManualDataCache.Instance.Get<List<sys_role>>(GlobalConstVars.sys_roleinfo);
            if (cache != null)
            {
                return cache;
            }
            return await UpdateCaChe();
        }

        /// <summary>
        ///     更新cache
        /// </summary>
        public async Task<List<sys_role>> UpdateCaChe()
        {
            var list = await DbClient.Queryable<sys_role>().With(SqlWith.NoLock).ToListAsync();
            ManualDataCache.Instance.Set(GlobalConstVars.sys_roleinfo, list);
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
        public  async Task<IPageList<sys_role>> QueryPageAsync(Expression<Func<sys_role, bool>> predicate,
            Expression<Func<sys_role, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<sys_role> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<sys_role>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new sys_role
                {
                    id = p.id,
                    no = p.no,
                    names = p.names,
                    shortNames = p.shortNames,
                    customCode = p.customCode,
                    companyNO = p.companyNO,
                    departmentNO = p.departmentNO,
                    phone = p.phone,
                    fax = p.fax,
                    functions = p.functions,
                    sort = p.sort,
                    powerList = p.powerList,
                    state = p.state,


                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<sys_role>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new sys_role
                {
                    id = p.id,
                    no = p.no,
                    names = p.names,
                    shortNames = p.shortNames,
                    customCode = p.customCode,
                    companyNO = p.companyNO,
                    departmentNO = p.departmentNO,
                    phone = p.phone,
                    fax = p.fax,
                    functions = p.functions,
                    sort = p.sort,
                    powerList = p.powerList,
                    state = p.state,

                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<sys_role>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
