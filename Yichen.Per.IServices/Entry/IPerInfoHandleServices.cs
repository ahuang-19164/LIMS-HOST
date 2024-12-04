using Yichen.Comm.IRepository;
using Yichen.Comm.IServices;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Per.Model;
using Yichen.Per.Model.table;

namespace Yichen.Per.IServices
{
    public interface IPerInfoHandleServices : IBaseServices<per_sampleInfo>
    {

        /// <summary>
        /// 编辑样本信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> EditInfo(EntryInfoModel infos);

        /// <summary>
        /// 获取审核信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> GetCheckInfo(CheckSelectModel infos);


        /// <summary>
        /// 审核信息（新）
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> CheckInfos(CheckInfoModel infos);
        ///// <summary>
        ///// 新冠审核信息
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        //Task<WebApiCallBack> CheckInfoXG(CheckInfoModel infos);

        /// <summary>
        /// 反审核信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> CheckRe(CheckInfoModel infos);
        /// <summary>
        /// 补打条码
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> CheckBc(CheckInfoModel infos);


        /// <summary>
        /// 信息接收
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> GetReceiveInfo(ReceiveSelectModel infos);


        /// <summary>
        /// 信息接收
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> ReceiveInfo(ReceiveInfoModel infos);
        /// <summary>
        /// 信息反接收
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> ReceiveRe(ReceiveInfoModel infos);



        /// <summary>
        /// 获取分拣信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> GetSortInfo(SortSelectModel infos);

        /// <summary>
        /// 分拣处理方法
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> SortInfo(SortInfoModel infos);
    }
}
