/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/



using Yichen.Comm.IRepository;
using Yichen.Manage.Model;
using Yichen.Test.Model;

namespace Yichen.Manage.IRepository
{
    /// <summary>
    ///     用户日志 工厂接口
    /// </summary>
    public interface ICRMHandleRepository : IBaseRepository<object>
    {
        /// <summary>
        /// 客服信息处理
        /// </summary>
        /// <param name="testInfo"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        Task<CRMStateModel> Clienthandle(TesthandleModel testInfo, string UserName);
        /// <summary>
        /// 危急值信息处理
        /// </summary>
        /// <param name="testInfo"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        Task<CRMStateModel> Crisishandle(TesthandleModel testInfo, string UserName);
        /// <summary>
        /// 委托信息处理
        /// </summary>
        /// <param name="testInfo"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        Task<CRMStateModel> EntrustHandle(TesthandleModel testInfo, string UserName);
        /// <summary>
        /// 样本延迟操作
        /// </summary>
        /// <param name="testInfo"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        Task<CRMStateModel> DelayHandle(TesthandleModel testInfo, string UserName);
        /// <summary>
        /// 标本重采操作
        /// </summary>
        /// <param name="testInfo"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        Task<CRMStateModel> AginHandle(TesthandleModel testInfo, string UserName);
        /// <summary>
        /// 标本退单操作
        /// </summary>
        /// <param name="testInfo"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        Task<CRMStateModel> backHandle(TesthandleModel testInfo, string UserName);

    }
}