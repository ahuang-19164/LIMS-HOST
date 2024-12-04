using Yichen.Comm.IServices;
using Yichen.Comm.Model;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Per.Model;

namespace Yichen.Per.IServices
{
    public interface IEntryHandleServices : IBaseServices<object>
    {

        /// <summary>
        /// 查询条码号是否存在
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> PerBarcodeExist(commInfoModel<string> barcode);

        /// <summary>
        /// 获取录入信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> GetEntryInfo(EntrySelectModel infos);



        /// <summary>
        /// 常规录入信息方法
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> EntryInfo(EntryInfoModel infos);

        /// <summary>
        /// 常规录入信息方法
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> EntryInfoNew(EntryInfoModel infos);

        /// <summary>
        /// 疾控录入信息方法
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> EntryInfoJK(JKEntryModel infos);
        /// <summary>
        /// 双录信息方法
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> EntryDouble(EntryInfoModel infos);
        ///// <summary>
        ///// 疾控录入信息方法
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        //Task<WebApiCallBack> EntryJK(JKEntryModel infos);
        ///// <summary>
        ///// 疾控详细信息录入方法
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        //Task<WebApiCallBack> EntryInfoJK(JKEntryModel infos);


        /// <summary>
        /// 常规录入信息方法
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> EntryDelete(DeleteInfoModel infos);

        ///// <summary>
        ///// 微信录入方法
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        //Task<WebApiCallBack> EntryWx(WxEntryModel infos);


        ///// <summary>
        ///// 网页信息录入方法
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        //Task<WebApiCallBack> EntryWeb(EntryInfoModel infos);











    }
}
