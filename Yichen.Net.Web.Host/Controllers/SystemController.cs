/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nito.AsyncEx;
using System;
using System.Threading.Tasks;
using Yichen.Comm.Model;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Flow.Model;
using Yichen.Net.Auth.HttpContextUser;
using Yichen.Net.Configuration;
using Yichen.Net.Model.FromBody;
using Yichen.System.IServices;
using Yichen.System.Model;


namespace Yichen.Net.Web.Host.Controllers
{
    /// <summary>
    /// 用户操作事件
    /// </summary>
    //[Route("api/[controller]/[action]/[action]")]
    [Route("api/[controller]")]
    [ApiController]
    public partial class SystemController : ControllerBase
    {

        private readonly IHttpContextUser _httpContextUser;
        private readonly IClientGroupServices _clientGroupServices;
        private readonly IClientInfoServices _clientInfoServices;
        private readonly IGroupTestServices _groupTestServices;
        private readonly IItemApplyServices _itemApplyServices;
        private readonly IItemFlowServices _itemFlowServices;
        private readonly IItemGroupServices _itemGroupServices;
        private readonly IItemReferenceServices _itemReferenceServices;
        private readonly IItemTestServices _itemTestServices;


        private readonly AsyncLock _mutex = new AsyncLock();

        /// <summary>
        /// 构造函数 
        /// </summary>
        public SystemController(
             IHttpContextUser httpContextUser
            , IClientGroupServices clientGroupServices
            , IClientInfoServices clientInfoServices
            , IGroupTestServices groupTestServices
            , IItemApplyServices itemApplyServices
            , IItemFlowServices itemFlowServices
            , IItemGroupServices itemGroupServices
            , IItemReferenceServices itemReferenceServices
            , IItemTestServices itemTestServices
            )
        {
            _httpContextUser = httpContextUser;
            _clientGroupServices = clientGroupServices;
            _clientInfoServices = clientInfoServices;
            _groupTestServices = groupTestServices;
            _itemApplyServices = itemApplyServices;
            _itemFlowServices = itemFlowServices;
            _itemGroupServices = itemGroupServices;
            _itemReferenceServices = itemReferenceServices;
            _itemTestServices = itemTestServices;
        }


        #region 基础信息维护=========================================================




        /// <summary>
        /// 客户专业组信息维护
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[HttpPost][Authorize]
        [HttpPost, Route("ClientGroupHandle")][Authorize]
        public async Task<WebApiCallBack> ClientGroupHandle(BaseModel<comm_client_group> baseModel)
        {

            var userinfo = _httpContextUser.Name;
            if (baseModel.operateType == 0)
                return await _clientGroupServices.SelectAsync();
            if (baseModel.operateType == 1)
            {
                baseModel.Value.creater = userinfo;
                baseModel.Value.createTime = DateTime.Now;
                return await _clientGroupServices.InsertAsync(baseModel.Value);
            }
            if (baseModel.operateType == 2)
                return await _clientGroupServices.UpdateAsync(baseModel.Value);
            if (baseModel.operateType == 3)
                return await _clientGroupServices.HideByIdAsync(baseModel.Value);
            if (baseModel.operateType == 4)
                return await _clientGroupServices.DeleteByIdAsync(baseModel.Value.id);
            return new WebApiCallBack() { code = 1, status = false, msg = SysConstVars.Error };
        }


        /// <summary>
        /// 客户信息维护
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[HttpPost][Authorize]
        [HttpPost, Route("ClientInfoHandle")]
        [Authorize]
        public async Task<WebApiCallBack> ClientInfoHandle(BaseModel<comm_client_info> baseModel)
        {
            if (baseModel.operateType == 0)
                return await _clientInfoServices.SelectAsync();
            if (baseModel.operateType == 1)
                return await _clientInfoServices.InsertAsync(baseModel.Value);
            if (baseModel.operateType == 2)
                return await _clientInfoServices.UpdateAsync(baseModel.Value);
            if (baseModel.operateType == 3)
                return await _clientInfoServices.HideByIdAsync(baseModel.Value);
            if (baseModel.operateType == 4)
                return await _clientInfoServices.DeleteByIdAsync(baseModel.Value.id);
            return new WebApiCallBack() { code = 1, status = false, msg = SysConstVars.Error };
        }


        /// <summary>
        /// 客户专业组信息维护
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[HttpPost][Authorize]
        [HttpPost, Route("GroupTesthandle")]
        [Authorize]
        public async Task<WebApiCallBack> GroupTestHandle(BaseModel<comm_group_test> baseModel)
        {
            if (baseModel.operateType == 0)
                return await _groupTestServices.SelectAsync();
            if (baseModel.operateType == 1)
                return await _groupTestServices.InsertAsync(baseModel.Value);
            if (baseModel.operateType == 2)
                return await _groupTestServices.UpdateAsync(baseModel.Value);
            if (baseModel.operateType == 3)
                return await _groupTestServices.HideByIdAsync(baseModel.Value);
            if (baseModel.operateType == 4)
                return await _groupTestServices.DeleteByIdAsync(baseModel.Value.id);
            return new WebApiCallBack() { code = 1, status = false, msg = SysConstVars.Error };
        }





        /// <summary>
        /// 项目组套信息维护
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[HttpPost][Authorize]
        [HttpPost, Route("ItemApplyHandle")]
        [Authorize]
        public async Task<WebApiCallBack> ItemApplyHandle(BaseModel<comm_item_apply> baseModel)
        {
            if (baseModel.operateType == 0)
                return await _itemApplyServices.SelectAsync();
            if (baseModel.operateType == 1)
                return await _itemApplyServices.InsertAsync(baseModel.Value);
            if (baseModel.operateType == 2)
                return await _itemApplyServices.UpdateAsync(baseModel.Value);
            if (baseModel.operateType == 3)
                return await _itemApplyServices.HideByIdAsync(baseModel.Value);
            if (baseModel.operateType == 4)
                return await _itemApplyServices.DeleteByIdAsync(baseModel.Value.id);
            return new WebApiCallBack() { code = 1, status = false, msg = SysConstVars.Error };
        }


        /// <summary>
        /// 项目组合流程信息维护
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[HttpPost][Authorize]
        [HttpPost, Route("ItemFlowHandle")]
        [Authorize]
        public async Task<WebApiCallBack> ItemFlowHandle(BaseModel<comm_item_flow> baseModel)
        {
            if (baseModel.operateType == 0)
                return await _itemFlowServices.SelectAsync();
            if (baseModel.operateType == 1)
                return await _itemFlowServices.InsertAsync(baseModel.Value);
            if (baseModel.operateType == 2)
                return await _itemFlowServices.UpdateAsync(baseModel.Value);
            if (baseModel.operateType == 3)
                return await _itemFlowServices.HideByIdAsync(baseModel.Value);
            if (baseModel.operateType == 4)
                return await _itemFlowServices.DeleteByIdAsync(baseModel.Value.id);
            return new WebApiCallBack() { code = 1, status = false, msg = SysConstVars.Error };
        }


        /// <summary>
        /// 项目组合信息维护
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[HttpPost][Authorize]
        [HttpPost, Route("ItemGroupHandle")]
        [Authorize]
        public async Task<WebApiCallBack> ItemGroupHandle(BaseModel<comm_item_group> baseModel)
        {
            if (baseModel.operateType == 0)
                return await _itemGroupServices.SelectAsync();
            if (baseModel.operateType == 1)
                return await _itemGroupServices.InsertAsync(baseModel.Value);
            if (baseModel.operateType == 2)
                return await _itemGroupServices.UpdateAsync(baseModel.Value);
            if (baseModel.operateType == 3)
                return await _itemGroupServices.HideByIdAsync(baseModel.Value);
            if (baseModel.operateType == 4)
                return await _itemGroupServices.DeleteByIdAsync(baseModel.Value.id);
            return new WebApiCallBack() { code = 1, status = false, msg = SysConstVars.Error };
        }

        /// <summary>
        /// 项目组合信息维护
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[HttpPost][Authorize]
        [HttpPost, Route("ItemTestHandle")]
        [Authorize]
        public async Task<WebApiCallBack> ItemTestHandle(BaseModel<comm_item_test> baseModel)
        {
            if (baseModel.operateType == 0)
                return await _itemTestServices.SelectAsync();
            if (baseModel.operateType == 1)
                return await _itemTestServices.InsertAsync(baseModel.Value);
            if (baseModel.operateType == 2)
                return await _itemTestServices.UpdateAsync(baseModel.Value);
            if (baseModel.operateType == 3)
                return await _itemTestServices.HideByIdAsync(baseModel.Value);
            if (baseModel.operateType == 4)
                return await _itemTestServices.DeleteByIdAsync(baseModel.Value.id);
            return new WebApiCallBack() { code = 1, status = false, msg = SysConstVars.Error };
        }


        /// <summary>
        /// 项目子项目信息维护
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[HttpPost][Authorize]
        [HttpPost, Route("ItemReferenceHandle")]
        [Authorize]
        public async Task<WebApiCallBack> ItemReferenceHandle(BaseModel<comm_item_reference> baseModel)
        {
            if (baseModel.operateType == 0)
                return await _itemReferenceServices.SelectAsync();
            if (baseModel.operateType == 1)
                return await _itemReferenceServices.InsertAsync(baseModel.Value);
            if (baseModel.operateType == 2)
                return await _itemReferenceServices.UpdateAsync(baseModel.Value);
            if (baseModel.operateType == 3)
                return await _itemReferenceServices.HideByIdAsync(baseModel.Value);
            if (baseModel.operateType == 4)
                return await _itemReferenceServices.DeleteByIdAsync(baseModel.Value.id);
            return new WebApiCallBack() { code = 1, status = false, msg = SysConstVars.Error };
        }



        #endregion



    }
}