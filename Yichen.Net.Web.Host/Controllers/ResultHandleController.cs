using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nito.AsyncEx;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Files.IServices;
using Yichen.System.IServices;
using Yichen.System.IServices.User;
using Yichen.Test.IServices;
using Yichen.Test.Model.Result;

namespace Yichen.Net.Web.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultHandleController : ControllerBase
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
        public ResultHandleController(
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
        #region 常规，微生物，基因检测结果保存
        /// <summary>
        /// 常规项目结果保存
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("SaveTest")][Authorize]
        public async Task<WebApiCallBack> SaveTest(CommResultModel<TestInfoModel> info)
        {
            return await _itemSaveOldServices.SaveTest(info);
        }

        /// <summary>
        /// 微生物结果保存 
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("SaveMicrobe")][Authorize]
        public async Task<WebApiCallBack> SaveMicrobe(CommResultModel<MicrobeInfoModel> info)
        {
            return await _itemSaveOldServices.SaveMicrobe(info);
        }
        /// <summary>
        /// 新冠结果保存
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("SaveGeneXG")][Authorize]
        public async Task<WebApiCallBack> SaveGeneXG(CommResultModel<GeneInfoModel> info)
        {
            return await _itemSaveOldServices.SaveGeneXG(info);
        }
        /// <summary>
        /// 公共样本项目结果保存
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("SaveGene")][Authorize]
        public async Task<WebApiCallBack> SaveGene(CommResultModel<GeneInfoModel> info)
        {
            return await _itemSaveOldServices.SaveGene(info);
        }
        #endregion

        #region 病理结果保存
        /// <summary>
        /// 蜡块处理方法
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("BlockHandle")][Authorize]
        public async Task<WebApiCallBack> BlockHandle(CommResultModel<List<BlockInfoModel>> info)
        {
            return await _itemBLSaveServices.BlockHandle(info);
        }
        /// <summary>
        /// 会诊结果保存
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("SaveConsultation")][Authorize]
        public async Task<WebApiCallBack> SaveConsultation(CommResultModel<PathnologyInfoModel> info)
        {
            WebApiCallBack jm = new WebApiCallBack();
            jm.data = await _itemBLSaveServices.SaveConsultation(info);
            return jm;
        }
        /// <summary>
        /// 病理结果保存
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("SavePathology")][Authorize]
        public async Task<WebApiCallBack> SavePathology(CommResultModel<PathnologyInfoModel> info)
        {
            //return await _itemBLSaveServices.SavePathology(info.TsqlInfo);
            WebApiCallBack jm = new WebApiCallBack();
            jm.data = await _itemBLSaveServices.SavePathology(info);
            return jm;
        }
        /// <summary>
        /// 筛查结果保存
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("SaveScreen")][Authorize]
        public async Task<WebApiCallBack> SaveScreen(CommResultModel<ScreenInfoModel> info)
        {
            //return await _itemBLSaveServices.SaveScreen(info.TsqlInfo);
            WebApiCallBack jm = new WebApiCallBack();
            jm.data = await _itemBLSaveServices.SaveScreen(info);
            return jm;
        }
        /// <summary>
        /// TCT结果保存
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("SaveTCT")][Authorize]
        public async Task<WebApiCallBack> SaveTCT(CommResultModel<TCTInfoModel> info)
        {
            //return await _itemBLSaveServices.SaveTCT(info.TsqlInfo);
            WebApiCallBack jm = new WebApiCallBack();
            jm.data = await _itemBLSaveServices.SaveTCT(info);
            return jm;
        }
        /// <summary>
        /// TCT筛查
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost, Route("SaveTCTScreen")][Authorize]
        public async Task<WebApiCallBack> SaveTCTScreen(CommResultModel<TCTInfoModel> info)
        {
            //return await _itemBLSaveServices.SaveTCTScreen(info.TsqlInfo);
            WebApiCallBack jm = new WebApiCallBack();
            jm.data = await _itemBLSaveServices.SaveTCTScreen(info);
            return jm;
        }
        #endregion
    }
}
