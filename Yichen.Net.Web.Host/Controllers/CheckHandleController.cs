using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Per.IServices;
using Yichen.Per.Model;

namespace Yichen.Net.Web.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   public class CheckHandleController : ControllerBase
   {
        //private readonly AsyncLock _mutex = new AsyncLock();
        //private readonly IUserServices _userServices;
        //private readonly IItemSaveOldServices _itemSaveOldServices;
        //private readonly IItemBLSaveServices _itemBLSaveServices;
        //private readonly IFileHandleServices _FileHandleServices;
        //private readonly IEntryHandleServices _entryHandleServices;
        private readonly IPerInfoHandleServices _perInfoHandleServices;




        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckHandleController(
            //IUserServices userServices
            //, IItemSaveOldServices itemSaveOldServices
            //, IItemBLSaveServices itemBLSaveServices
            //, IFileHandleServices fileHandleServices
            //IEntryHandleServices entryHandleServices
            IPerInfoHandleServices perInfoHandleServices

            )
        {
            //_userServices = userServices;
            //_itemSaveOldServices = itemSaveOldServices;
            //_itemBLSaveServices = itemBLSaveServices;
            //_FileHandleServices = fileHandleServices;
            //_entryHandleServices = entryHandleServices;
            _perInfoHandleServices = perInfoHandleServices;
        }


        #region


        /// <summary>
        /// 录入信息审核
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("GetCheckInfo")][Authorize]
        public async Task<WebApiCallBack> GetCheckInfo(CheckSelectModel infos)
        {
            return await _perInfoHandleServices.GetCheckInfo(infos);
        }


        /// <summary>
        /// 录入信息审核
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("CheckInfo")][Authorize]
        public async Task<WebApiCallBack> CheckInfo(CheckInfoModel infos)
        {
            return await _perInfoHandleServices.CheckInfos(infos);
        }

        ///// <summary>
        ///// 录入信息审核
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        //[HttpPost, Route("CheckInfoXG")][Authorize]
        //public async Task<WebApiCallBack> CheckInfoXG(CheckInfoModel infos)
        //{
        //    return await _perInfoHandleServices.CheckInfoXG(infos);
        //}


        /// <summary>
        /// 反审核信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("CheckRe")][Authorize]
        public async Task<WebApiCallBack> CheckRe(CheckInfoModel infos)
        {
            return await _perInfoHandleServices.CheckRe(infos);
        }

        /// <summary>
        /// 补打条码
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("CheckBc")][Authorize]
        public async Task<WebApiCallBack> CheckBc(CheckInfoModel infos)
        {
            return await _perInfoHandleServices.CheckBc(infos);
        }

        #endregion


        #region 分拣处理


        /// <summary>
        /// 获取分拣信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("GetSortInfo")][Authorize]
        public async Task<WebApiCallBack> GetSortInfo(SortSelectModel infos)
        {
            return await _perInfoHandleServices.GetSortInfo(infos);
        }

        /// <summary>
        /// 分拣处理方法
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("SortInfo")][Authorize]
        public async Task<WebApiCallBack> SortInfo(SortInfoModel infos)
        {
            return await _perInfoHandleServices.SortInfo(infos);
        }



        #endregion


        #region  交接处理

        /// <summary>
        /// 获取接收信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("GetReceiveInfo")][Authorize]
        public async Task<WebApiCallBack> GetReceiveInfo(ReceiveSelectModel infos)
        {
            return await _perInfoHandleServices.GetReceiveInfo(infos);
        }


        /// <summary>
        /// 信息接收
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("ReceiveInfo")][Authorize]
        public async Task<WebApiCallBack> ReceiveInfo(ReceiveInfoModel infos)
        {
            return await _perInfoHandleServices.ReceiveInfo(infos);
        }
        /// <summary>
        /// 信息反接收
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("ReceiveRe")][Authorize]
        public async Task<WebApiCallBack> ReceiveRe(ReceiveInfoModel infos)
        {
            return await _perInfoHandleServices.ReceiveRe(infos);
        }

        #endregion





        /// <summary>
        /// 编辑样本信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("EditInfo")][Authorize]
        public async Task<WebApiCallBack> EditInfo(EntryInfoModel infos)
        {
            return await _perInfoHandleServices.EditInfo(infos);
        }






    }
}
