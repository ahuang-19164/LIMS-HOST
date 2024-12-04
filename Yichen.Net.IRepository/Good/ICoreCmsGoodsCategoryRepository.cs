/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;
using Yichen.Comm.IRepository;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Net.Model.Entities;
namespace Yichen.Net.IRepository
{
    /// <summary>
    ///     商品分类 工厂接口
    /// </summary>
    public interface ICoreCmsGoodsCategoryRepository : IBaseRepository<CoreCmsGoodsCategory>
    {
        #region 获取缓存的所有数据==========================================================

        /// <summary>
        ///     获取缓存的所有数据
        /// </summary>
        /// <returns></returns>
        Task<List<CoreCmsGoodsCategory>> GetCaChe();

        #endregion

        #region 重写增删改查操作===========================================================

        /// <summary>
        ///     事务重写异步插入方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> InsertAsync(CoreCmsGoodsCategory entity);


        /// <summary>
        ///     重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> UpdateAsync(CoreCmsGoodsCategory entity);


        /// <summary>
        ///     重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> UpdateAsync(List<CoreCmsGoodsCategory> entity);


        /// <summary>
        ///     重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> DeleteByIdAsync(object id);


        /// <summary>
        ///     重写删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> DeleteByIdsAsync(int[] ids);

        #endregion
    }
}