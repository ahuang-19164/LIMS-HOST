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
    public class ItemGroupRepository : BaseRepository<comm_item_group>, IItemGroupRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public ItemGroupRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> InsertAsync(comm_item_group entity)
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
        public  async Task<WebApiCallBack> UpdateAsync(comm_item_group entity)
        {
            var jm = new WebApiCallBack();

            var oldModel = await DbClient.Queryable<comm_item_group>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }


            //事物处理过程开始

            oldModel.id = entity.id;
            oldModel.no = entity.no;
            oldModel.names = entity.names;
            oldModel.groupNO = entity.groupNO;
            oldModel.disNames = entity.disNames;
            oldModel.shortNames = entity.shortNames;
            oldModel.customCode = entity.customCode;
            oldModel.customNames = entity.customNames;
            oldModel.workNO = entity.workNO;
            oldModel.companyNO = entity.companyNO;
            oldModel.sampleTypeNO = entity.sampleTypeNO;
            oldModel.containerTypeNO = entity.containerTypeNO;
            oldModel.bloodVolume = entity.bloodVolume;
            oldModel.reportTypeNO = entity.reportTypeNO;
            oldModel.itemTypeNO = entity.itemTypeNO;
            oldModel.sort = entity.sort;
            oldModel.delegeteCompanyNO = entity.delegeteCompanyNO;
            oldModel.timeUse = entity.timeUse;
            oldModel.takeSample = entity.takeSample;
            oldModel.groupFlowNO = entity.groupFlowNO;
            oldModel.reflectionFile = entity.reflectionFile;
            oldModel.reflectionFrm = entity.reflectionFrm;
            oldModel.remark = entity.remark;
            oldModel.delegeteState = entity.delegeteState;
            oldModel.reportState = entity.reportState;
            oldModel.ihcState = entity.ihcState;
            oldModel.state = entity.state;
            oldModel.dstate = entity.dstate;
            oldModel.testItemList = entity.testItemList;


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
        public  async Task<WebApiCallBack> UpdateAsync(List<comm_item_group> entity)
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

            var bl = await DbClient.Deleteable<comm_item_group>(id).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Deleteable<comm_item_group>().In(ids).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Updateable<comm_item_group>().SetColumns(p => p.dstate == true).Where(p => p.id == Convert.ToInt32(id)).ExecuteCommandHasChangeAsync();
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
        public async Task<List<comm_item_group>> GetCaChe()
        {
            var cache = ManualDataCache.Instance.Get<List<comm_item_group>>(GlobalConstVars.comm_item_group);
            if (cache != null)
            {
                return cache;
            }
            return await UpdateCaChe();
        }

        /// <summary>
        ///     更新cache
        /// </summary>
        public async Task<List<comm_item_group>> UpdateCaChe()
        {
            var list = await DbClient.Queryable<comm_item_group>().With(SqlWith.NoLock).ToListAsync();
            ManualDataCache.Instance.Set(GlobalConstVars.comm_item_group, list);
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
        public  async Task<IPageList<comm_item_group>> QueryPageAsync(Expression<Func<comm_item_group, bool>> predicate,
            Expression<Func<comm_item_group, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<comm_item_group> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<comm_item_group>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new comm_item_group
                {
                id = p.id,
                no = p.no,
                names = p.names,
                groupNO = p.groupNO,
                disNames = p.disNames,
                shortNames = p.shortNames,
                customCode = p.customCode,
                customNames = p.customNames,
                workNO = p.workNO,
                companyNO = p.companyNO,
                sampleTypeNO = p.sampleTypeNO,
                containerTypeNO = p.containerTypeNO,
                bloodVolume = p.bloodVolume,
                reportTypeNO = p.reportTypeNO,
                itemTypeNO = p.itemTypeNO,
                sort = p.sort,
                delegeteCompanyNO = p.delegeteCompanyNO,
                timeUse = p.timeUse,
                takeSample = p.takeSample,
                groupFlowNO = p.groupFlowNO,
                reflectionFile = p.reflectionFile,
                reflectionFrm = p.reflectionFrm,
                remark = p.remark,
                delegeteState = p.delegeteState,
                reportState = p.reportState,
                ihcState = p.ihcState,
                state = p.state,
                testItemList = p.testItemList

            }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<comm_item_group>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new comm_item_group
                {
                    id = p.id,
                    no = p.no,
                    names = p.names,
                    groupNO = p.groupNO,
                    disNames = p.disNames,
                    shortNames = p.shortNames,
                    customCode = p.customCode,
                    customNames = p.customNames,
                    workNO = p.workNO,
                    companyNO = p.companyNO,
                    sampleTypeNO = p.sampleTypeNO,
                    containerTypeNO = p.containerTypeNO,
                    bloodVolume = p.bloodVolume,
                    reportTypeNO = p.reportTypeNO,
                    itemTypeNO = p.itemTypeNO,
                    sort = p.sort,
                    delegeteCompanyNO = p.delegeteCompanyNO,
                    timeUse = p.timeUse,
                    takeSample = p.takeSample,
                    groupFlowNO = p.groupFlowNO,
                    reflectionFile = p.reflectionFile,
                    reflectionFrm = p.reflectionFrm,
                    remark = p.remark,
                    delegeteState = p.delegeteState,
                    reportState = p.reportState,
                    ihcState = p.ihcState,
                    state = p.state,
                    testItemList = p.testItemList

                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<comm_item_group>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
