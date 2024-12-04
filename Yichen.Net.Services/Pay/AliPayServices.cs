/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using Yichen.Net.IRepository;
using Yichen.Net.IServices;
using Yichen.Net.Model.Entities;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Comm.Services;

namespace Yichen.Net.Services
{
    /// <summary>
    ///     支付宝支付 接口实现
    /// </summary>
    public class AliPayServices : BaseServices<CoreCmsSetting>, IAliPayServices
    {
        public AliPayServices(IWeChatPayRepository dal)
        {
            BaseDal = dal;
        }

        /// <summary>
        ///     发起支付
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public WebApiCallBack PubPay(CoreCmsBillPayments entity)
        {
            var jm = new WebApiCallBack();
            return jm;
        }
    }
}