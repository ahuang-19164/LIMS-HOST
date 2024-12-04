using Yichen.Comm.IRepository;
using Yichen.Comm.IServices;
using Yichen.Comm.Model;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Test.Model;
using Yichen.Test.Model.Result;
using Yichen.Test.Model.table;

namespace Yichen.Test.IServices
{
    public interface ITestHandleServices : IBaseServices<test_sampleInfo>
    {


        //Task<WebApiCallBack> AddTestInfo(test_sampleInfo infos);


        /// <summary>
        /// 获取检验中的样本信息
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> GetTestInfo(GetTestInfoModel infos);
        /// <summary>
        /// 获取指定样本的项目信息
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> GetItemInfo(GetItemInfoModel infos);
        /// <summary>
        /// 获取微生物结果
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> GetTestMicrobeInfo(commInfoModel<GetMicrobeItemModel> infos);
        /// <summary>
        /// 刷新样本项目的参考值信息
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> GetReferenceRefresh(commInfoModel<TestWorkModel> infos);
        /// <summary>
        /// 获取结果图片上传信息
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> GetTestImg(commInfoModel<TestDownModel> infos);
    }
}
