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
using Yichen.System.Model.Entities;

namespace Yichen.System.Repository
{
    /// <summary>
    ///  接口实现
    /// </summary>
    public class ClientDelegateInfoRepository : BaseRepository<ClientDelegateInfo>, IClientDelegateInfoRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public ClientDelegateInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> InsertAsync(ClientDelegateInfo entity)
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
        public  async Task<WebApiCallBack> UpdateAsync(ClientDelegateInfo entity)
        {
            var jm = new WebApiCallBack();

            var oldModel = await DbClient.Queryable<ClientDelegateInfo>().In(entity.id).SingleAsync();
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
            oldModel.clientTypeNO = entity.clientTypeNO;
            oldModel.agentNO = entity.agentNO;
            oldModel.province = entity.province;
            oldModel.city = entity.city;
            oldModel.area = entity.area;
            oldModel.reportNames = entity.reportNames;
            oldModel.namesEn = entity.namesEn;
            oldModel.contacts = entity.contacts;
            oldModel.contactsPhone = entity.contactsPhone;
            oldModel.address = entity.address;
            oldModel.qq = entity.qq;
            oldModel.wechat = entity.wechat;
            oldModel.email = entity.email;
            oldModel.personNO = entity.personNO;
            oldModel.signTime = entity.signTime;
            oldModel.expireTime = entity.expireTime;
            oldModel.chargeLevelNO = entity.chargeLevelNO;
            oldModel.discount = entity.discount;
            oldModel.distributionNO = entity.distributionNO;
            oldModel.passWord = entity.passWord;
            oldModel.exceedDay = entity.exceedDay;
            oldModel.invoiceNames = entity.invoiceNames;
            oldModel.invoiceCode = entity.invoiceCode;
            oldModel.invoicePhone = entity.invoicePhone;
            oldModel.invoiceAddress = entity.invoiceAddress;
            oldModel.serialNo = entity.serialNo;
            oldModel.personalize = entity.personalize;
            oldModel.remark = entity.remark;
            oldModel.sort = entity.sort;
            oldModel.webstate = entity.webstate;
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
        public  async Task<WebApiCallBack> UpdateAsync(List<ClientDelegateInfo> entity)
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

            var bl = await DbClient.Deleteable<ClientDelegateInfo>(id).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Deleteable<ClientDelegateInfo>().In(ids).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Updateable<ClientDelegateInfo>().SetColumns(p => p.dstate == true).Where(p => p.id == Convert.ToInt32(id)).ExecuteCommandHasChangeAsync();
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
        public async Task<List<ClientDelegateInfo>> GetCaChe()
        {
            var cache = ManualDataCache.Instance.Get<List<ClientDelegateInfo>>(GlobalConstVars.CacheClientDelegateInfo);
            if (cache != null)
            {
                return cache;
            }
            return await UpdateCaChe();
        }

        /// <summary>
        ///     更新cache
        /// </summary>
        public async Task<List<ClientDelegateInfo>> UpdateCaChe()
        {
            var list = await DbClient.Queryable<ClientDelegateInfo>().With(SqlWith.NoLock).ToListAsync();
            ManualDataCache.Instance.Set(GlobalConstVars.CacheClientDelegateInfo, list);
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
        public  async Task<IPageList<ClientDelegateInfo>> QueryPageAsync(Expression<Func<ClientDelegateInfo, bool>> predicate,
            Expression<Func<ClientDelegateInfo, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<ClientDelegateInfo> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<ClientDelegateInfo>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new ClientDelegateInfo
                {
                      id = p.id,
                no = p.no,
                names = p.names,
                shortNames = p.shortNames,
                customCode = p.customCode,
                clientTypeNO = p.clientTypeNO,
                agentNO = p.agentNO,
                province = p.province,
                city = p.city,
                area = p.area,
                reportNames = p.reportNames,
                namesEn = p.namesEn,
                contacts = p.contacts,
                contactsPhone = p.contactsPhone,
                address = p.address,
                qq = p.qq,
                wechat = p.wechat,
                email = p.email,
                personNO = p.personNO,
                signTime = p.signTime,
                expireTime = p.expireTime,
                chargeLevelNO = p.chargeLevelNO,
                discount = p.discount,
                distributionNO = p.distributionNO,
                passWord = p.passWord,
                exceedDay = p.exceedDay,
                invoiceNames = p.invoiceNames,
                invoiceCode = p.invoiceCode,
                invoicePhone = p.invoicePhone,
                invoiceAddress = p.invoiceAddress,
                serialNo = p.serialNo,
                personalize = p.personalize,
                remark = p.remark,
                sort = p.sort,
                webstate = p.webstate,
                state = p.state,
                dstate = p.dstate,
                
                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<ClientDelegateInfo>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new ClientDelegateInfo
                {
                      id = p.id,
                no = p.no,
                names = p.names,
                shortNames = p.shortNames,
                customCode = p.customCode,
                clientTypeNO = p.clientTypeNO,
                agentNO = p.agentNO,
                province = p.province,
                city = p.city,
                area = p.area,
                reportNames = p.reportNames,
                namesEn = p.namesEn,
                contacts = p.contacts,
                contactsPhone = p.contactsPhone,
                address = p.address,
                qq = p.qq,
                wechat = p.wechat,
                email = p.email,
                personNO = p.personNO,
                signTime = p.signTime,
                expireTime = p.expireTime,
                chargeLevelNO = p.chargeLevelNO,
                discount = p.discount,
                distributionNO = p.distributionNO,
                passWord = p.passWord,
                exceedDay = p.exceedDay,
                invoiceNames = p.invoiceNames,
                invoiceCode = p.invoiceCode,
                invoicePhone = p.invoicePhone,
                invoiceAddress = p.invoiceAddress,
                serialNo = p.serialNo,
                personalize = p.personalize,
                remark = p.remark,
                sort = p.sort,
                webstate = p.webstate,
                state = p.state,
                dstate = p.dstate,
                
                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<ClientDelegateInfo>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
