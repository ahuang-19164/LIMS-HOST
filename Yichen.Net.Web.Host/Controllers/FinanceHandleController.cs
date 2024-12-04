using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nito.AsyncEx;
using System.Threading.Tasks;
using Yichen.Comm.Model;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Finance.IServices;
using Yichen.Finance.Model;

namespace Yichen.Net.Web.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanceHandleController : ControllerBase
    {
        private readonly AsyncLock _mutex = new AsyncLock();
        private readonly IFinanceHandleServices _financeHandleServices;
        public FinanceHandleController(
            IFinanceHandleServices financeHandleServices
            )
        {
            _financeHandleServices = financeHandleServices;
        }


        /// <summary>
        /// 获取财务流水号
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("GetCheckSerial")][Authorize]
        public async Task<WebApiCallBack> CheckGetSerial()
        {
            //WebApiCallBack jm = new WebApiCallBack();
            //jm.data = await _financeHandleServices.CheckGetSerial();
            //return jm;
            return await _financeHandleServices.CheckGetSerial();
        }
        /// <summary>
        /// 获取收费信息
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("GetFinanceInfo")][Authorize]
        public async Task<WebApiCallBack> GetFinanceInfo(commInfoModel<SelectFinanceModel> info)
        {
            //WebApiCallBack jm = new WebApiCallBack();
            //jm.data = await _financeHandleServices.GetFinanceInfo(info);
            //return jm;
            return await _financeHandleServices.GetFinanceInfo(info);
        }

        /// <summary>
        /// 获取审核账单信息
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("GetCheckInfo")][Authorize]
        public async Task<WebApiCallBack> GetCheckInfo(commInfoModel<SelectFinanceCheckModel> info)
        {
            //WebApiCallBack jm = new WebApiCallBack();
            //jm.data = await _financeHandleServices.GetCheckInfo(info);
            //return jm;
            return await _financeHandleServices.GetCheckInfo(info);
        }

        /// <summary>
        /// 获取修改收费信息
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("FinancePrice")][Authorize]
        public async Task<WebApiCallBack> FinancePrice(commInfoModel<priceChangeModel> info)
        {
            //WebApiCallBack jm = new WebApiCallBack();
            //jm.data = await _financeHandleServices.FinancePrice(info);
            //return jm;
            return await _financeHandleServices.FinancePrice(info);
        }

        /// <summary>
        /// 账单审核
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("BillCheck")][Authorize]
        public async Task<WebApiCallBack> BillCheck(commInfoModel<FinanceCheckInfoModel> info)
        {
            //WebApiCallBack jm = new WebApiCallBack();
            //jm.data = await _financeHandleServices.BillCheck(info);
            //return jm;
            return await _financeHandleServices.BillCheck(info);
        }
        /// <summary>
        /// 账单反审核
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("BillReCheck")][Authorize]
        public async Task<WebApiCallBack> BillReCheck(commInfoModel<FinanceCheckInfoModel> info)
        {
            //WebApiCallBack jm = new WebApiCallBack();
            //jm.data = await _financeHandleServices.BillReCheck(info);
            //return jm;
            return await _financeHandleServices.BillReCheck(info);
        }

        /// <summary>
        /// 回款信息处理
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("FundHandle")][Authorize]
        public async Task<WebApiCallBack> FundHandle(commInfoModel<FundInfoModel> info)
        {
            //WebApiCallBack jm = new WebApiCallBack();
            //jm.data = await _financeHandleServices.FundHandle(info);
            //return jm;
            return await _financeHandleServices.FundHandle(info);
        }





    }
}
