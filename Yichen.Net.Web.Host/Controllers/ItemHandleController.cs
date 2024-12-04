using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nito.AsyncEx;
using System.Threading.Tasks;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Files.IServices;
using Yichen.System.IRepository;
using Yichen.System.IServices;
using Yichen.System.IServices.User;
using Yichen.Test.IRepository;
using Yichen.Test.IServices;
using Yichen.Test.Model.table;

namespace Yichen.Net.Web.Host.Controllers
{
    //[Route("api/[controller]/[action]")]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemHandleController : ControllerBase
    {
        private readonly AsyncLock _mutex = new AsyncLock();
        private readonly IUserServices _userServices;
        private readonly IItemSaveOldServices _itemSaveOldServices;
        private readonly IItemBLSaveServices _itemBLSaveServices;
        private readonly IFileHandleServices _FileHandleServices;
        private readonly IItemHandleServices _itemHandleServices;
        private readonly ITestHandleServices _testHandleServices;
        private readonly ITestItemInfoRepository _itemHandleRepository;




        /// <summary>
        /// 构造函数
        /// </summary>
        public ItemHandleController(
            IUserServices userServices
            , IItemSaveOldServices itemSaveOldServices
            , IItemBLSaveServices itemBLSaveServices
            , IFileHandleServices fileHandleServices
            , IItemHandleServices itemHandleServices
            , ITestHandleServices testHandleServices
            , ITestItemInfoRepository itemHandleRepository

            )
        {
            _userServices = userServices;
            _itemSaveOldServices = itemSaveOldServices;
            _itemBLSaveServices = itemBLSaveServices;
            _FileHandleServices = fileHandleServices;
            _itemHandleServices = itemHandleServices;
            _testHandleServices = testHandleServices;
            _itemHandleRepository = itemHandleRepository;
        }

        /// <summary>
        /// 插入检验信息
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("AddTestInfo")][Authorize]
        public async Task<WebApiCallBack> AddTestInfo(test_sampleInfo info)
        {
            WebApiCallBack jm = new WebApiCallBack();
            jm.data = await _itemHandleServices.InsertAsync(info);
            return jm;
        }

        ///// <summary>
        ///// 插入检验信息2
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet, Route("ItemReferenceHandle")][Authorize]
        //public async Task<WebApiCallBack> ItemReferenceHandle(int testid,bool state=true)
        //{
        //    WebApiCallBack jm = new WebApiCallBack();
        //    bool stateaa= await _itemHandleRepository.ItemReferenceHandle(testid, state);
        //    if (stateaa)
        //        jm.msg = "刷新成功";
        //    return jm;
        //}


    }
}
