/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/7/28 20:42:38
 *        Description: 暂无
 ***********************************************************************/

using Yichen.Net.Model.Entities;
using Yichen.Comm.IServices;

namespace Yichen.Net.IServices
{
    /// <summary>
    ///     微信授权交互 服务工厂接口
    /// </summary>
    public interface IWeChatAccessTokenServices : IBaseServices<WeChatAccessToken>
    {
    }
}