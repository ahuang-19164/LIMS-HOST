using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Yichen.Comm.Model;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Manage.IServices;
using Yichen.Manage.Model;
using Yichen.Test.Model;

namespace Yichen.Net.Web.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageHandleController : ControllerBase
    {
        //private readonly AsyncLock _mutex = new AsyncLock();
        //private readonly IUserServices _userServices;
        //private readonly IEntryHandleServices _entryHandleServices;
        private readonly ICRMServices _crmServices;
        private readonly ISampleHandleServices _sampleHandleServices;
        private readonly IManageInfoServices _manageInfoServices;




        /// <summary>
        /// 构造函数
        /// </summary>
        public ManageHandleController(
            ICRMServices crmServices
            , ISampleHandleServices sampleHandleServices
            , IManageInfoServices manageInfoServices
            )
        {
            _crmServices = crmServices;
            _sampleHandleServices = sampleHandleServices;
            _manageInfoServices = manageInfoServices;
        }


        /// <summary>
        /// 获取委托样本信息
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("GetEntrustInfo")][Authorize]
        public async Task<WebApiCallBack> GetEntrustInfo(ManageInfoModel info)
        {
            return await _manageInfoServices.GetEntrustInfo(info);
        }


        /// <summary>
        /// 获取IHC样本信息（免疫组化）
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("GetIHCInfo")][Authorize]
        public async Task<WebApiCallBack> GetIHCInfo(ManageInfoModel info)
        {
            return await _manageInfoServices.GetIHCInfo(info);
        }


        /// <summary>
        /// 客服信息处理
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("CRMHandle")][Authorize]
        public async Task<WebApiCallBack> CRMHandle(commInfoModel<TesthandleModel> info)
        {
            return await _crmServices.CRMHandle(info);
        }

        /// <summary>
        /// 危急值信息处理
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("CrisisHandle")][Authorize]
        public async Task<WebApiCallBack> CrisisHandle(commInfoModel<TesthandleModel> info)
        {
            return await _crmServices.CrisisHandle(info);
        }



        /// <summary>
        /// 样本信息特殊处理信息提交
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("SampleHandle")][Authorize]
        public async Task<WebApiCallBack> SampleHandle(commInfoModel<TesthandleModel> infos)
        {
            return await _sampleHandleServices.SampleHandle(infos);
        }



        ///// <summary>
        ///// 样本延迟
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        //[HttpPost, Route("InfoDelay")][Authorize]
        //public async Task<WebApiCallBack> InfoDelay(commInfoModel<TesthandleModel> infos)
        //{
        //    return await _sampleHandleServices.InfoDelay(infos);
        //}
        ///// <summary>
        ///// 样本重采
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        //[HttpPost, Route("InfoAgin")][Authorize]
        //public async Task<WebApiCallBack> InfoAgin(commInfoModel<TesthandleModel> infos)
        //{
        //    return await _sampleHandleServices.InfoAgin(infos);
        //}
        ///// <summary>
        ///// 样本退回
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        //[HttpPost, Route("Infoback")][Authorize]
        //public async Task<WebApiCallBack> Infoback(commInfoModel<TesthandleModel> infos)
        //{
        //    return await _sampleHandleServices.Infoback(infos);
        //}
        ///// <summary>
        ///// 退项申请
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        //[HttpPost, Route("GroupItemCancel")][Authorize]
        //public async Task<WebApiCallBack> GroupItemCancel(commInfoModel<TesthandleModel> infos)
        //{
        //    return await _sampleHandleServices.GroupItemCancel(infos);
        //}
        ///// <summary>
        ///// 增项申请
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        //[HttpPost, Route("GorupItemAdd")][Authorize]
        //public async Task<WebApiCallBack> GorupItemAdd(commInfoModel<TesthandleModel> infos)
        //{
        //    return await _sampleHandleServices.GorupItemAdd(infos);
        //}
        ///// <summary>
        ///// 更新样本申请信息
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        //[HttpPost, Route("ReSpecialRecord")][Authorize]
        //public async Task<WebApiCallBack> ReSpecialRecord(commInfoModel<TesthandleModel> infos)
        //{
        //    return await _sampleHandleServices.ReSpecialRecord(infos);
        //}
        ///// <summary>
        ///// 更新免疫组化提交记录
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        //[HttpPost, Route("ReIHCRecord")][Authorize]
        //public async Task<WebApiCallBack> ReIHCRecord(commInfoModel<TesthandleModel> infos)
        //{
        //    return await _sampleHandleServices.ReIHCRecord(infos);
        //}

    }
}
