/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.Yichen.Net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2023/11/4 16:00:37
 *        Description: 暂无
 ***********************************************************************/

using System.Linq.Expressions;
using Yichen.Comm.Model.ViewModels.Basics;
using Yichen.Comm.Model.ViewModels.UI;
using SqlSugar;
using Yichen.Comm.Services;
using Yichen.System.Model;
using Yichen.System.IServices;
using Yichen.System.IRepository;
using Yichen.Comm.IRepository.UnitOfWork;

namespace Yichen.System.Services
{
    /// <summary>
    /// 选择题表 接口实现
    /// </summary>
    public class ItemReferenceServices : BaseServices<comm_item_reference>, IItemReferenceServices
    {
        private readonly IItemReferenceRepository _dal;
        private readonly IUnitOfWork _unitOfWork;

        public ItemReferenceServices(IUnitOfWork unitOfWork, IItemReferenceRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }

        #region 实现重写增删改查操作==========================================================
        /// <summary>
        /// 重写异步查询方法（过滤dstate信息）
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> SelectAsync()
        {
            var jm = new WebApiCallBack();

            var list = await _dal.GetCaChe();
            ////var lists = list.Where(p => p.dstate == false);
            if (list.Count > 0)
            {
                jm.code = 0;
                jm.status = true;
                jm.data = list;
                jm.msg = "查询成功";
            }
            else
            {
                jm.code = 1;
                jm.status = false;
                jm.msg = "未查询到数据";
            }
            return jm;
        }
        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> InsertAsync(comm_item_reference entity)
        {
            return await _dal.InsertAsync(entity);
        }

        /// <summary>
        /// 重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> UpdateAsync(comm_item_reference entity)
        {
            return await _dal.UpdateAsync(entity);
        }

        /// <summary>
        /// 重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> UpdateAsync(List<comm_item_reference> entity)
        {
            return await _dal.UpdateAsync(entity);
        }

        /// <summary>
        /// 重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> DeleteByIdAsync(object id)
        {
            return await _dal.DeleteByIdAsync(id);
        }

        /// <summary>
        /// 重写删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public  async Task<WebApiCallBack> DeleteByIdsAsync(int[] ids)
        {
            return await _dal.DeleteByIdsAsync(ids);
        }

        /// <summary>
        /// 隐藏指定ID集合的数据
        /// </summary>
        /// <param name="ids"></pa
        public  async Task<WebApiCallBack> HideByIdAsync(object id)
        {
            return await _dal.HideByIdAsync(id);
        }

        #endregion

        #region 获取缓存的所有数据==========================================================

        /// <summary>
        /// 获取缓存的所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<comm_item_reference>> GetCaChe()
        {
            return await _dal.GetCaChe();
        }

        /// <summary>
        ///     更新cache
        /// </summary>
        public async Task<List<comm_item_reference>> UpdateCaChe()
        {
            return await _dal.UpdateCaChe();
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
        public  async Task<IPageList<comm_item_reference>> QueryPageAsync(Expression<Func<comm_item_reference, bool>> predicate,
            Expression<Func<comm_item_reference, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            return await _dal.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize, blUseNoLock);
        }


        #endregion


    }
}
