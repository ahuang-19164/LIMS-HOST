using Yichen.Comm.IRepository;
using Yichen.Comm.IServices;
using Yichen.Comm.Model;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Test.Model;

namespace Yichen.Manage.IServices
{
    public interface ISampleHandleServices : IBaseServices<object>
    {


        /// <summary>
        /// 样本信息特殊处理信息提交
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> SampleHandle(commInfoModel<TesthandleModel> infos);
        ///// <summary>
        ///// 样本延迟
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        //Task<WebApiCallBack> InfoDelay(commInfoModel<TesthandleModel> infos);
        ///// <summary>
        ///// 样本重采
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        //Task<WebApiCallBack> InfoAgin(commInfoModel<TesthandleModel> infos);
        ///// <summary>
        ///// 样本退回
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        //Task<WebApiCallBack> Infoback(commInfoModel<TesthandleModel> infos);
        ///// <summary>
        ///// 退项申请
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        //Task<WebApiCallBack> GroupItemCancel(commInfoModel<TesthandleModel> infos);
        ///// <summary>
        ///// 增项申请
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        //Task<WebApiCallBack> GorupItemAdd(commInfoModel<TesthandleModel> infos);
        ///// <summary>
        ///// 增加免疫组化申请
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        ////Task<WebApiCallBack> GorupIHCAdd(commInfoModel<TesthandleModel> infos);
        ///// <summary>
        ///// 更新样本申请信息
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        //Task<WebApiCallBack> ReSpecialRecord(commInfoModel<TesthandleModel> infos);
        ///// <summary>
        ///// ReIHCRecord
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        //Task<WebApiCallBack> ReIHCRecord(commInfoModel<TesthandleModel> infos);
    }
}
