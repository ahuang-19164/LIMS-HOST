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
using SqlSugar;
using Yichen.Comm.Repository;
using Yichen.System.Model;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.System.IRepository;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Comm.Model.ViewModels.Basics;

namespace Yichen.System.Repository
{
    /// <summary>
    /// 用户课程范围 接口实现
    /// </summary>
    public class UserRepository : BaseRepository<sys_user>, IUserRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<sys_user> UserLogin(string userid, string userpwd)
        {
            var oldModel = await DbClient.Queryable<sys_user>().Where(p=>p.userNO==userid &&p.pwd==userpwd&&p.dstate==false).FirstAsync();
            return oldModel;
        }


        #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> InsertAsync(sys_user entity)
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
        public  async Task<WebApiCallBack> UpdateAsync(sys_user entity)
        {
            var jm = new WebApiCallBack();

            var oldModel = await DbClient.Queryable<sys_user>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            //事物处理过程开始
            oldModel.id = entity.id;
            oldModel.userNO = entity.userNO;
            oldModel.names = entity.names;
            oldModel.no = entity.no;
            oldModel.shortNames = entity.shortNames;
            oldModel.sex = entity.sex;
            oldModel.sort = entity.sort;
            oldModel.state = entity.state;
            oldModel.companyNO = entity.companyNO;
            oldModel.clientList = entity.clientList;
            oldModel.customCode = entity.customCode;
            oldModel.birthday = entity.birthday;
            oldModel.departmentNO = entity.departmentNO;
            oldModel.email = entity.email;
            oldModel.phone = entity.phone;
            oldModel.remark = entity.remark;
            oldModel.roleNO = entity.roleNO;
            oldModel.webRoleNO = entity.webRoleNO;
            oldModel.webstate = entity.webstate;
            oldModel.weChat = entity.weChat;
            oldModel.workPhone = entity.workPhone;

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
        public  async Task<WebApiCallBack> UpdateAsync(List<sys_user> entity)
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

            var bl = await DbClient.Deleteable<sys_user>(id).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Deleteable<sys_user>().In(ids).ExecuteCommandHasChangeAsync();
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
        public async Task<List<sys_user>> GetCaChe()
        {
            var cache = ManualDataCache.Instance.Get<List<sys_user>>(GlobalConstVars.sys_userinfo);
            if (cache != null)
            {
                return cache;
            }
            return await UpdateCaChe();
        }

        /// <summary>
        ///     更新cache
        /// </summary>
        public async Task<List<sys_user>> UpdateCaChe()
        {
            var list = await DbClient.Queryable<sys_user>().With(SqlWith.NoLock).ToListAsync();
            ManualDataCache.Instance.Set(GlobalConstVars.sys_userinfo, list);
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
        public  async Task<IPageList<sys_user>> QueryPageAsync(Expression<Func<sys_user, bool>> predicate,
            Expression<Func<sys_user, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<sys_user> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<sys_user>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new sys_user
                {
                    id = p.id,
                    userNO = p.userNO,
                    names = p.names,
                    no = p.no,
                    shortNames = p.shortNames,
                    sex = p.sex,
                    sort = p.sort,
                    state = p.state,
                    companyNO = p.companyNO,
                    clientList = p.clientList,
                    customCode = p.customCode,
                    birthday = p.birthday,
                    departmentNO = p.departmentNO,
                    email = p.email,
                    phone = p.phone,
                    remark = p.remark,
                    roleNO = p.roleNO,
                    webRoleNO = p.webRoleNO,
                    webstate = p.webstate,
                    weChat = p.weChat,
                    workPhone = p.workPhone

                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<sys_user>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new sys_user
                {
                    id = p.id,
                    userNO = p.userNO,
                    names = p.names,
                    no = p.no,
                    shortNames = p.shortNames,
                    sex = p.sex,
                    sort = p.sort,
                    state = p.state,
                    companyNO = p.companyNO,
                    clientList = p.clientList,
                    customCode = p.customCode,
                    birthday = p.birthday,
                    departmentNO = p.departmentNO,
                    email = p.email,
                    phone = p.phone,
                    remark = p.remark,
                    roleNO = p.roleNO,
                    webRoleNO = p.webRoleNO,
                    webstate = p.webstate,
                    weChat = p.weChat,
                    workPhone = p.workPhone

                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<sys_user>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
