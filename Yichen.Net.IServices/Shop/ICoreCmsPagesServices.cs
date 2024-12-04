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
using Yichen.Net.Model.Entities;
using Yichen.Net.Model.FromBody;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Comm.IServices;

namespace Yichen.Net.IServices
{
    /// <summary>
    ///     单页 服务工厂接口
    /// </summary>
    public interface ICoreCmsPagesServices : IBaseServices<CoreCmsPages>
    {

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> InsertAsync(CoreCmsPages entity);

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> UpdateAsync(CoreCmsPages entity);

        /// <summary>
        /// 重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> DeleteByIdAsync(int id);


        /// <summary>
        ///     更新设计
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> UpdateDesignAsync(FmPagesUpdate entity);

        /// <summary>
        ///     获取首页数据
        /// </summary>
        /// <param name="code">查询编码</param>
        /// <returns></returns>
        Task<WebApiCallBack> GetPageConfig(string code);


        /// <summary>
        /// 复制生成一个新的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> CopyByIdAsync(int id);

    }
}