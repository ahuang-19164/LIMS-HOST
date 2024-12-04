using Yichen.Comm.IRepository;
using Yichen.Comm.IServices;
using Yichen.Comm.Model;

using Yichen.Comm.Model.ViewModels.UI;
using Yichen.System.Model;
using Yichen.Test.Model;

namespace Yichen.Manage.IServices
{
    public interface ICRMServices : IBaseServices<comm_samplerecord>
    {
        /// <summary>
        /// 客服信息处理
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<WebApiCallBack> CRMHandle(commInfoModel<TesthandleModel> info);

        /// <summary>
        /// 危急值信息处理
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<WebApiCallBack> CrisisHandle(commInfoModel<TesthandleModel> info);
    }
}
