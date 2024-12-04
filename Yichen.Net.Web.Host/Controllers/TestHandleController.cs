using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nito.AsyncEx;
using System.Threading.Tasks;
using Yichen.Comm.Model;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Files.IServices;
using Yichen.System.IServices;
using Yichen.System.IServices.User;
using Yichen.Test.IServices;
using Yichen.Test.Model;
using Yichen.Test.Model.Result;

namespace Yichen.Net.Web.Host.Controllers
{
    /// <summary>
    /// 检验中操作方法
    /// </summary>
    //[Route("api/[controller]/[action]")]
    [Route("api/[controller]")]
    [ApiController]
    public partial class TestHandleController : ControllerBase
    {
        private readonly AsyncLock _mutex = new AsyncLock();
        private readonly IUserServices _userServices;
        private readonly IItemSaveOldServices _itemSaveOldServices;
        private readonly IItemBLSaveServices _itemBLSaveServices;
        private readonly IFileHandleServices _FileHandleServices;
        private readonly ITestHandleServices _testHandleServices;




        /// <summary>
        /// 构造函数
        /// </summary>
        public TestHandleController(
            IUserServices userServices
            , ITestHandleServices testHandleServices
            , IItemSaveOldServices itemSaveOldServices
            , IItemBLSaveServices itemBLSaveServices
            , IFileHandleServices fileHandleServices

            )
        {
            _userServices = userServices;
            _itemSaveOldServices = itemSaveOldServices;
            _itemBLSaveServices = itemBLSaveServices;
            _FileHandleServices = fileHandleServices;
            _testHandleServices = testHandleServices;
        }






        /// <summary>
        /// 获取检验信息
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("GetTestInfo")][Authorize]
        public async Task<WebApiCallBack> GetTestInfo(GetTestInfoModel info)
        {
            return await _testHandleServices.GetTestInfo(info);
        }


        /// <summary>
        /// 获取样本项目信息
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("GetItemInfo")][Authorize]
        public async Task<WebApiCallBack> GetItemInfo(GetItemInfoModel info)
        {
            return await _testHandleServices.GetItemInfo(info);
        }

        /// <summary>
        /// 获取微生物结果
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost, Route("GetMicrobeInfo")][Authorize]
        public async Task<WebApiCallBack> GetMicrobeInfo(commInfoModel<GetMicrobeItemModel> info)
        {
            return await _testHandleServices.GetTestMicrobeInfo(info);
        }

        /// <summary>
        /// 获取结果图片上传信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost, Route("GetTestImg")][Authorize]
        public async Task<WebApiCallBack> GetTestImg(commInfoModel<TestDownModel> info)
        {
            return await _testHandleServices.GetTestImg(info);
        }

        /// <summary>
        /// 刷新样本项目的参考值信息
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("GetReferenceRefresh")][Authorize]
        public async Task<WebApiCallBack> GetReferenceRefresh(commInfoModel<TestWorkModel> info)
        {
            return await _testHandleServices.GetReferenceRefresh(info);
        }

























    }
}
