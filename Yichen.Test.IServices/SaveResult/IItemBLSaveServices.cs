using Yichen.Comm.IServices;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Test.Model.Result;
using Yichen.Test.Model.table;


namespace Yichen.Test.IServices
{
    /// <summary>
    /// 病理结果保存
    /// </summary>
    public interface IItemBLSaveServices : IBaseServices<test_sampleInfo>
    {

        /// <summary>
        /// 蜡块处理方法
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> BlockHandle(CommResultModel<List<BlockInfoModel>> info);
        /// <summary>
        /// 会诊结果保存
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> SaveConsultation(CommResultModel<PathnologyInfoModel> info);
        /// <summary>
        /// 病理结果保存
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> SavePathology(CommResultModel<PathnologyInfoModel> info);
        /// <summary>
        /// 瓜片结果保存
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> SaveScreen(CommResultModel<ScreenInfoModel> info);
        /// <summary>
        /// TCT结果保存
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> SaveTCT(CommResultModel<TCTInfoModel> info);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<WebApiCallBack> SaveTCTScreen(CommResultModel<TCTInfoModel> info);

    }
}
