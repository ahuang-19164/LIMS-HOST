/***********************************************************************
 *            Project: Yichen
 *        ProjectName: 屹辰智禾管理系统                                
 *                Web: https://www.zui51.com                 
 *             Author: 屹辰                                       
 *              Email: 499715561@qq.com                              
 *         CreateTime: 2023-11-16 11:56:59
 *        Description: 暂无
 ***********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
using NPOI.SS.Formula.Functions;

namespace Yichen.Stores.Repository
{
    /// <summary>
    ///  接口实现
    /// </summary>
    public class StoresJobRepository : BaseRepository<sw_record>, IStoresJobRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public StoresJobRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 刷新存储标本记录
        /// </summary>
        /// <returns></returns>
        public  async Task<bool> refreshRecord()
        {
            var recordlist=await DbClient.Queryable<sw_record>().Where(p=>p.state==0).ToListAsync();
            if(recordlist!=null)
            {
                foreach(var record in recordlist)
                {
                    if (record.outTime < DateTime.Now)
                    {
                        record.recordTypeNO = 3;
                        record.state = 1;
                        var s=await DbClient.Updateable<sw_record>(record).ExecuteCommandAsync();
                    }
                }
            }
            return true;
        }


        /// <summary>
        /// 刷新标本架状态
        /// </summary>
        /// <returns></returns>
        public  async Task<bool> refreshShelf()
        {
            var shelflist = await DbClient.Queryable<sw_shelf>().Where(p => p.shelfTypeNO != 4).ToListAsync();
            if(shelflist.Count>0)
            {
                foreach(var shelf in shelflist)
                {
                    var recordlist = await DbClient.Queryable<sw_record>().Where(p => p.shelfNO == shelf.no).Select(p=>new sw_record { recordTypeNO=p.recordTypeNO}).ToListAsync();
                    var overtype = recordlist.Where(p =>p.recordTypeNO==2).Count();
                    if (overtype!=0)
                    {
                        shelf.sampleCount = recordlist.Count;
                        if (overtype < recordlist.Count)
                        {
                            shelf.shelfTypeNO = 2;
                            var s = await DbClient.Updateable<sw_shelf>(shelf).ExecuteCommandAsync();
                        }
                        else
                        {
                            shelf.shelfTypeNO = 3;
                            var s = await DbClient.Updateable<sw_shelf>(shelf).ExecuteCommandAsync();
                        }

                    }
                }
                
            }
            return true;
        }



        #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> InsertAsync(sw_stores entity)
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
        public  async Task<WebApiCallBack> UpdateAsync(sw_stores entity)
        {
            var jm = new WebApiCallBack();

            var oldModel = await DbClient.Queryable<sw_stores>().In(entity.id).SingleAsync();
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
            oldModel.address = entity.address;
            oldModel.saveDay = entity.saveDay;
            oldModel.shoresRow = entity.shoresRow;
            oldModel.shoresCell = entity.shoresCell;
            oldModel.sampleType = entity.sampleType;
            oldModel.remark = entity.remark;
            oldModel.sore = entity.sore;
            oldModel.creater = entity.creater;
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
        public  async Task<WebApiCallBack> UpdateAsync(List<sw_stores> entity)
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

            var bl = await DbClient.Deleteable<sw_stores>(id).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Deleteable<sw_stores>().In(ids).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Updateable<sw_stores>().SetColumns(p => p.dstate == true).Where(p => p.id == Convert.ToInt32(id)).ExecuteCommandHasChangeAsync();
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
        public async Task<List<sw_stores>> GetCaChe()
        {
            var cache = ManualDataCache.Instance.Get<List<sw_stores>>(GlobalConstVars.Cachsw_stores);
            if (cache != null)
            {
                return cache;
            }
            return await UpdateCaChe();
        }

        /// <summary>
        ///     更新cache
        /// </summary>
        public async Task<List<sw_stores>> UpdateCaChe()
        {
            var list = await DbClient.Queryable<sw_stores>().With(SqlWith.NoLock).ToListAsync();
            ManualDataCache.Instance.Set(GlobalConstVars.Cachsw_stores, list);
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
        public  async Task<IPageList<sw_stores>> QueryPageAsync(Expression<Func<sw_stores, bool>> predicate,
            Expression<Func<sw_stores, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<sw_stores> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<sw_stores>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new sw_stores
                {
                      id = p.id,
                no = p.no,
                names = p.names,
                shortNames = p.shortNames,
                customCode = p.customCode,
                address = p.address,
                saveDay = p.saveDay,
                shoresRow = p.shoresRow,
                shoresCell = p.shoresCell,
                sampleType = p.sampleType,
                remark = p.remark,
                sore = p.sore,
                creater = p.creater,
                createTime = p.createTime,
                state = p.state,
                dstate = p.dstate,
                
                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<sw_stores>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new sw_stores
                {
                      id = p.id,
                no = p.no,
                names = p.names,
                shortNames = p.shortNames,
                customCode = p.customCode,
                address = p.address,
                saveDay = p.saveDay,
                shoresRow = p.shoresRow,
                shoresCell = p.shoresCell,
                sampleType = p.sampleType,
                remark = p.remark,
                sore = p.sore,
                creater = p.creater,
                createTime = p.createTime,
                state = p.state,
                dstate = p.dstate,
                
                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<sw_stores>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
