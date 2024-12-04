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
    public class ClientAgentRepository : BaseRepository<ClientAgent>, IClientAgentRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public ClientAgentRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> InsertAsync(ClientAgent entity)
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
        public  async Task<WebApiCallBack> UpdateAsync(ClientAgent entity)
        {
            var jm = new WebApiCallBack();

            var oldModel = await DbClient.Queryable<ClientAgent>().In(entity.id).SingleAsync();
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
            oldModel.disNames = entity.disNames;
            oldModel.contacts = entity.contacts;
            oldModel.contactsPhone = entity.contactsPhone;
            oldModel.address = entity.address;
            oldModel.email = entity.email;
            oldModel.wechat = entity.wechat;
            oldModel.signTime = entity.signTime;
            oldModel.expireTime = entity.expireTime;
            oldModel.personNO = entity.personNO;
            oldModel.passWord = entity.passWord;
            oldModel.invoiceNames = entity.invoiceNames;
            oldModel.invoiceCode = entity.invoiceCode;
            oldModel.invoicePhone = entity.invoicePhone;
            oldModel.invoiceAddress = entity.invoiceAddress;
            oldModel.remark = entity.remark;
            oldModel.sort = entity.sort;
            oldModel.webstate = entity.webstate;
            oldModel.state = entity.state;
            oldModel.dstate = entity.dstate;
            oldModel.powerList = entity.powerList;
            
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
        public  async Task<WebApiCallBack> UpdateAsync(List<ClientAgent> entity)
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

            var bl = await DbClient.Deleteable<ClientAgent>(id).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Deleteable<ClientAgent>().In(ids).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Updateable<ClientAgent>().SetColumns(p => p.dstate == true).Where(p => p.id == Convert.ToInt32(id)).ExecuteCommandHasChangeAsync();
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
        public async Task<List<ClientAgent>> GetCaChe()
        {
            var cache = ManualDataCache.Instance.Get<List<ClientAgent>>(GlobalConstVars.CacheClientAgent);
            if (cache != null)
            {
                return cache;
            }
            return await UpdateCaChe();
        }

        /// <summary>
        ///     更新cache
        /// </summary>
        public async Task<List<ClientAgent>> UpdateCaChe()
        {
            var list = await DbClient.Queryable<ClientAgent>().With(SqlWith.NoLock).ToListAsync();
            ManualDataCache.Instance.Set(GlobalConstVars.CacheClientAgent, list);
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
        public  async Task<IPageList<ClientAgent>> QueryPageAsync(Expression<Func<ClientAgent, bool>> predicate,
            Expression<Func<ClientAgent, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<ClientAgent> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<ClientAgent>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new ClientAgent
                {
                      id = p.id,
                no = p.no,
                names = p.names,
                shortNames = p.shortNames,
                customCode = p.customCode,
                disNames = p.disNames,
                contacts = p.contacts,
                contactsPhone = p.contactsPhone,
                address = p.address,
                email = p.email,
                wechat = p.wechat,
                signTime = p.signTime,
                expireTime = p.expireTime,
                personNO = p.personNO,
                passWord = p.passWord,
                invoiceNames = p.invoiceNames,
                invoiceCode = p.invoiceCode,
                invoicePhone = p.invoicePhone,
                invoiceAddress = p.invoiceAddress,
                remark = p.remark,
                sort = p.sort,
                webstate = p.webstate,
                state = p.state,
                dstate = p.dstate,
                powerList = p.powerList,
                
                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<ClientAgent>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new ClientAgent
                {
                      id = p.id,
                no = p.no,
                names = p.names,
                shortNames = p.shortNames,
                customCode = p.customCode,
                disNames = p.disNames,
                contacts = p.contacts,
                contactsPhone = p.contactsPhone,
                address = p.address,
                email = p.email,
                wechat = p.wechat,
                signTime = p.signTime,
                expireTime = p.expireTime,
                personNO = p.personNO,
                passWord = p.passWord,
                invoiceNames = p.invoiceNames,
                invoiceCode = p.invoiceCode,
                invoicePhone = p.invoicePhone,
                invoiceAddress = p.invoiceAddress,
                remark = p.remark,
                sort = p.sort,
                webstate = p.webstate,
                state = p.state,
                dstate = p.dstate,
                powerList = p.powerList,
                
                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<ClientAgent>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
