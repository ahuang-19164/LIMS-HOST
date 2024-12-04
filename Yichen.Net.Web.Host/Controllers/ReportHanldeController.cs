using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nito.AsyncEx;
using System.Threading.Tasks;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Report.IServices;
using Yichen.Report.Model;
using Yichen.System.IServices;
using Yichen.System.IServices.User;

namespace Yichen.Net.Web.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportHanldeController : ControllerBase
    {
        private readonly AsyncLock _mutex = new AsyncLock();
        private readonly IUserServices _userServices;
        private readonly IReportHandleServices _reportHandleServices;





        /// <summary>
        /// 构造函数
        /// </summary>
        public ReportHanldeController(
            IUserServices userServices
            , IReportHandleServices reportHandleServices)
        {
            _userServices = userServices;
            _reportHandleServices = reportHandleServices;
        }


        /// <summary>
        /// 上传报告接口
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("ReportUpload")][Authorize]
        public async Task<WebApiCallBack> ReportUpload(UpLoadReportModel info)
        {
            return await _reportHandleServices.ReportUpload(info);
        }

        /// <summary>
        /// 下载报告接口
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("ReportDown")][Authorize]
        public async Task<WebApiCallBack> ReportDown(GetReportModel info)
        {
            return await _reportHandleServices.ReportDown(info);
        }

    }
}
