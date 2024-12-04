using Yichen.Comm.IRepository;
using Yichen.Comm.IServices;
using Yichen.Comm.Model;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Finance.Model;

namespace Yichen.Finance.IServices
{
    public interface IFinanceHandleServices : IBaseServices<object>
    {
        /// <summary>
        /// 创建账单流水号
        /// </summary>
        /// <returns></returns>
        new Task<WebApiCallBack> CheckGetSerial();

        /// <summary>
        /// 查询收费信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        new Task<WebApiCallBack> GetFinanceInfo(commInfoModel<SelectFinanceModel> infos);
        /// <summary>
        /// 获取审核账单信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        new Task<WebApiCallBack> GetCheckInfo(commInfoModel<SelectFinanceCheckModel> infos);
        /// <summary>
        /// 获取修改收费信息
        /// </summary>
        /// <returns></returns>
        new Task<WebApiCallBack> FinancePrice(commInfoModel<priceChangeModel> info);
        /// <summary>
        /// 账单审核
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        new Task<WebApiCallBack> BillCheck(commInfoModel<FinanceCheckInfoModel> info);
        /// <summary>
        /// 账单反审核
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        new Task<WebApiCallBack> BillReCheck(commInfoModel<FinanceCheckInfoModel> info);
        /// <summary>
        /// 回款处理
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        new Task<WebApiCallBack> FundHandle(commInfoModel<FundInfoModel> info);
    }
}
