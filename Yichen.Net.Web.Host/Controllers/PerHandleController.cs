using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Yichen.Comm.Model;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Per.IServices;
using Yichen.Per.Model;

namespace Yichen.Net.Web.Host.Controllers
{

    /// <summary>
    /// 前处理操作方法
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PerHandleController : ControllerBase
    {
        private readonly IEntryHandleServices _entryHandleServices;
        /// <summary>
        /// 构造函数
        /// </summary>
        public PerHandleController(
            IEntryHandleServices entryHandleServices
            )
        {
            _entryHandleServices = entryHandleServices;
        }


        #region 信息录入


        /// <summary>
        /// 查询条码是否存在
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("PerBarcodeExist")][Authorize]
        public async Task<WebApiCallBack> PerBarcodeExist(commInfoModel<string> barcode)
        {
            return await _entryHandleServices.PerBarcodeExist(barcode);
        }



        /// <summary>
        /// 获取录入信息
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("GetEntryInfo")][Authorize]
        public async Task<WebApiCallBack> GetEntryInfo(EntrySelectModel info)
        {
            return await _entryHandleServices.GetEntryInfo(info);
        }

        /// <summary>
        /// 常规样本信息录入
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("EntryInfo")][Authorize]
        public async Task<WebApiCallBack> EntryInfo(EntryInfoModel info)
        {
            return await _entryHandleServices.EntryInfoNew(info);
        }
        /// <summary>
        /// 疾控样本信息录入
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("EntryInfoJK")][Authorize]
        public async Task<WebApiCallBack> EntryInfoJK(JKEntryModel info)
        {
            return await _entryHandleServices.EntryInfoJK(info);
        }

        /// <summary>
        /// 删除样本信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("EntryDelete")][Authorize]
        public async Task<WebApiCallBack> EntryDelete(DeleteInfoModel infos)
        {
            return await _entryHandleServices.EntryDelete(infos);
        }






        ///// <summary>
        ///// 疾控样本信息录入
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost, Route("EntryInfoJK")][Authorize]
        //public async Task<WebApiCallBack> EntryInfoJK(JKEntryModel info)
        //{
        //    return await _entryHandleServices.EntryInfoJK(info);
        //}

        #endregion

    }
}
