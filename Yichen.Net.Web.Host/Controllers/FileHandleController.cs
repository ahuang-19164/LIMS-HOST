using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nito.AsyncEx;
using System.IO;
using System.Threading.Tasks;
using Yichen.Comm.Model;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Files.IServices;
using Yichen.Files.Model;
using Yichen.System.IServices;


namespace Yichen.Net.Web.Host.Controllers
{
    //[Route("api/[controller]/[action]")]
    [Route("api/[controller]")]
    [ApiController]
    public class FileHandleController : ControllerBase
    {

        private readonly AsyncLock _mutex = new AsyncLock();
        private readonly IFileHandleServices _fileHandleServices;
        private readonly ISysCommServices _sysCommServices;
        /// <summary>
        /// 构造函数
        /// </summary>
        public FileHandleController(
            ISysCommServices sysCommServices
            , IFileHandleServices fileHandleServices
            )
        {
            _sysCommServices = sysCommServices;
            _fileHandleServices = fileHandleServices;
        }



        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("SysDownFile")][Authorize]
        public async Task<WebApiCallBack> SysDownFile(commInfoModel<FileModel> infos)
        {
            return await _fileHandleServices.SysDownFile(infos);
        }
        /// <summary>
        /// 系统信息查询
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("SysUpFile")][Authorize]
        public async Task<WebApiCallBack> SysUpFile(commInfoModel<FileModel> infos)
        {
            return await _fileHandleServices.SysUpFile(infos);
        }

        /// <summary>
        /// 上传flow文件
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("FlowUpFile")][Authorize]
        public async Task<WebApiCallBack> FlowUpFile(commInfoModel<FileModel> infos)
        {
            return await _fileHandleServices.FlowUpFile(infos);
        }
        /// <summary>
        /// 下载flow文件
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("FlowDownFile")][Authorize]
        public async Task<WebApiCallBack> FlowDownFile(commInfoModel<FileModel> infos)
        {
            return await _fileHandleServices.FlowDownFile(infos);
        }

        /// <summary>
        /// 上传检验结果文件
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("TestUpFile")][Authorize]
        public async Task<WebApiCallBack> TestUpFile(commInfoModel<FileModel> infos)
        {
            return await _fileHandleServices.TestUpFile(infos);
        }
        /// <summary>
        /// 下载检验结果文件
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("TestDownFile")][Authorize]
        public async Task<WebApiCallBack> TestDownFile(commInfoModel<FileModel> infos)
        {
            return await _fileHandleServices.TestDownFile(infos);
        }

        /// <summary>
        /// 更新客户端
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("UpgradeClient")][Authorize]
        public async Task<FileStream> UpgradeClient(DownFileModel infos)
        {
            return await _fileHandleServices.UpgradeClient(infos);
        }
        /// <summary>
        /// 更新报告客户端
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("UpgradeReportClient")][Authorize]
        public async Task<FileStream> UpgradeReportClient(DownFileModel infos)
        {
            return await _fileHandleServices.UpgradeReportClient(infos);
        }
        /// <summary>
        /// 获取更新记录信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("UpgradeUpInfos")][Authorize]
        public async Task<WebApiCallBack> UpgradeUpInfos(ClientInfoModel infos)
        {
            return await _fileHandleServices.UpgradeUpInfos(infos);
        }








    }
}
