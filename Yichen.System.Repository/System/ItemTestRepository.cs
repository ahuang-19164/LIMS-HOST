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
    public class ItemTestRepository : BaseRepository<comm_item_test>, IItemTestRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public ItemTestRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> InsertAsync(comm_item_test entity)
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
        public  async Task<WebApiCallBack> UpdateAsync(comm_item_test entity)
        {
            var jm = new WebApiCallBack();

            var oldModel = await DbClient.Queryable<comm_item_test>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            //事物处理过程开始
            oldModel.id = entity.id;
            oldModel.no = entity.no;
            oldModel.groupNO = entity.groupNO;
            oldModel.names = entity.names;
            oldModel.disNames = entity.disNames;
            oldModel.simpleNames = entity.simpleNames;
            oldModel.namesEN = entity.namesEN;
            oldModel.shortNames = entity.shortNames;
            oldModel.customCode = entity.customCode;
            oldModel.cunstomNames = entity.cunstomNames;
            oldModel.sampleTypeNO = entity.sampleTypeNO;
            oldModel.unit = entity.unit;
            oldModel.instrumentNO = entity.instrumentNO;
            oldModel.channel = entity.channel;
            oldModel.methodNO = entity.methodNO;
            oldModel.methodName = entity.methodName;
            oldModel.testTypeNO = entity.testTypeNO;
            oldModel.testFlowNO = entity.testFlowNO;
            oldModel.resultTypeNO = entity.resultTypeNO;
            oldModel.defaultValue = entity.defaultValue;
            oldModel.precision = entity.precision;
            oldModel.calculationFormula = entity.calculationFormula;
            oldModel.resultTable = entity.resultTable;
            oldModel.resultImg = entity.resultImg;
            oldModel.calculationState = entity.calculationState;
            oldModel.delegeteState = entity.delegeteState;
            oldModel.delegeteCompanyNO = entity.delegeteCompanyNO;
            oldModel.delegeteTime = entity.delegeteTime;
            oldModel.visibleState = entity.visibleState;
            oldModel.resultNullState = entity.resultNullState;
            oldModel.sort = entity.sort;
            oldModel.ihcState = entity.ihcState;
            oldModel.state = entity.state;
            oldModel.dstate = entity.dstate;
            oldModel.qcState = entity.qcState;
            oldModel.remark = entity.remark;


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
        public  async Task<WebApiCallBack> UpdateAsync(List<comm_item_test> entity)
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

            var bl = await DbClient.Deleteable<comm_item_test>(id).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Deleteable<comm_item_test>().In(ids).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Updateable<comm_item_test>().SetColumns(p => p.dstate == true).Where(p => p.id == Convert.ToInt32(id)).ExecuteCommandHasChangeAsync();
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
        public async Task<List<comm_item_test>> GetCaChe()
        {
            var cache = ManualDataCache.Instance.Get<List<comm_item_test>>(GlobalConstVars.comm_item_test);
            if (cache != null)
            {
                return cache;
            }
            return await UpdateCaChe();
        }

        /// <summary>
        ///     更新cache
        /// </summary>
        public async Task<List<comm_item_test>> UpdateCaChe()
        {
            var list = await DbClient.Queryable<comm_item_test>().With(SqlWith.NoLock).ToListAsync();
            ManualDataCache.Instance.Set(GlobalConstVars.comm_item_test, list);
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
        public  async Task<IPageList<comm_item_test>> QueryPageAsync(Expression<Func<comm_item_test, bool>> predicate,
            Expression<Func<comm_item_test, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<comm_item_test> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<comm_item_test>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new comm_item_test
                {
                    id = p.id,
                    no = p.no,
                    groupNO = p.groupNO,
                    names = p.names,
                    disNames = p.disNames,
                    simpleNames = p.simpleNames,
                    namesEN = p.namesEN,
                    shortNames = p.shortNames,
                    customCode = p.customCode,
                    cunstomNames = p.cunstomNames,
                    sampleTypeNO = p.sampleTypeNO,
                    unit = p.unit,
                    instrumentNO = p.instrumentNO,
                    channel = p.channel,
                    methodNO = p.methodNO,
                    methodName = p.methodName,
                    testTypeNO = p.testTypeNO,
                    testFlowNO = p.testFlowNO,
                    resultTypeNO = p.resultTypeNO,
                    defaultValue = p.defaultValue,
                    precision = p.precision,
                    calculationFormula = p.calculationFormula,
                    resultTable = p.resultTable,
                    resultImg = p.resultImg,
                    calculationState = p.calculationState,
                    delegeteState = p.delegeteState,
                    delegeteCompanyNO = p.delegeteCompanyNO,
                    delegeteTime = p.delegeteTime,
                    visibleState = p.visibleState,
                    resultNullState = p.resultNullState,
                    sort = p.sort,
                    ihcState = p.ihcState,
                    state = p.state,
                    dstate = p.dstate,
                    qcState = p.qcState,
                    remark = p.remark,


                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<comm_item_test>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new comm_item_test
                {
                    id = p.id,
                    no = p.no,
                    groupNO = p.groupNO,
                    names = p.names,
                    disNames = p.disNames,
                    simpleNames = p.simpleNames,
                    namesEN = p.namesEN,
                    shortNames = p.shortNames,
                    customCode = p.customCode,
                    cunstomNames = p.cunstomNames,
                    sampleTypeNO = p.sampleTypeNO,
                    unit = p.unit,
                    instrumentNO = p.instrumentNO,
                    channel = p.channel,
                    methodNO = p.methodNO,
                    methodName = p.methodName,
                    testTypeNO = p.testTypeNO,
                    testFlowNO = p.testFlowNO,
                    resultTypeNO = p.resultTypeNO,
                    defaultValue = p.defaultValue,
                    precision = p.precision,
                    calculationFormula = p.calculationFormula,
                    resultTable = p.resultTable,
                    resultImg = p.resultImg,
                    calculationState = p.calculationState,
                    delegeteState = p.delegeteState,
                    delegeteCompanyNO = p.delegeteCompanyNO,
                    delegeteTime = p.delegeteTime,
                    visibleState = p.visibleState,
                    resultNullState = p.resultNullState,
                    sort = p.sort,
                    ihcState = p.ihcState,
                    state = p.state,
                    dstate = p.dstate,
                    qcState = p.qcState,
                    remark = p.remark,


                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<comm_item_test>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
