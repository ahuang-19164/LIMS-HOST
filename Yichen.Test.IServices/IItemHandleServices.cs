using Yichen.Comm.IRepository;
using Yichen.Comm.IServices;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Test.Model.table;

namespace Yichen.Test.IServices
{

    /// <summary>
    /// 危急值插入信息接口
    /// </summary>
    public interface IItemHandleServices : IBaseServices<test_sampleInfo>
    {
        /// <summary>
        /// 插入危急值记录
        /// </summary>
        /// <param name="perid"></param>
        /// <param name="testid"></param>
        /// <param name="barcode"></param>
        /// <param name="itemCode"></param>
        /// <param name="itemName"></param>
        /// <param name="result"></param>
        /// <param name="Reference"></param>
        /// <param name="CrisisReference"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        Task<bool> ItemCrisisHandle(int perid, int testid, string barcode, string itemCode, string itemName, string result, string Reference, string CrisisReference, string UserName);
        /// <summary>
        /// 更新检验中参考值信息
        /// </summary>
        /// <param name="perid"></param>
        /// <param name="testid"></param>
        /// <param name="barcode"></param>
        /// <param name="itemCode"></param>
        /// <param name="itemName"></param>
        /// <param name="result"></param>
        /// <param name="Reference"></param>
        /// <param name="CrisisReference"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        Task<WebApiCallBack> ItemReferenceHandle(int testid, string barcode, bool defaultValue = true);
    }
}
