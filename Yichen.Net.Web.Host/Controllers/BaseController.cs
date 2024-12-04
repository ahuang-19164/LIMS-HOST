using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nito.AsyncEx;
using System;
using System.Threading.Tasks;
using Yichen.Comm.IServices;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Net.Auth.HttpContextUser;
using Yichen.Net.Data;
using Yichen.System.IServices;

namespace Yichen.Net.Web.Host.Controllers
{
    /// <summary>
    /// 基础访问接口
    /// </summary>
    //[Route("api/[controller]/[action]")]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly AsyncLock _mutex = new AsyncLock();
        private readonly IHttpContextAccessor _userServices;
        private readonly IHttpContextUser _httpContextUser;
        private readonly ISysCommServices _sysCommServices;
        private readonly IToolsServices _toolsServices;
        //private readonly IItemBLSaveServices _itemBLSaveServices;




        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseController(
            ISysCommServices sysCommServices
            , IHttpContextUser httpContextUser
            //, IItemBLSaveServices itemBLSaveServices
            , IToolsServices toolsServices
            )
        {
            _sysCommServices = sysCommServices;
            _httpContextUser = httpContextUser;
            _toolsServices = toolsServices;
        }

        #region 系统基本增删改查方法

        /// <summary>
        /// 系统信息修改
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("SysUpdate")][Authorize]
        public async Task<WebApiCallBack> SysUpdate(uInfo infos)
        {
            string s = JsonConvert.SerializeObject(infos).ToLower();
            bool b = await _toolsServices.IllegalSqlContainsAny(s);
            if (b)
                return new WebApiCallBack() { code = 1, status = false, msg = "非法访问" };
            return await _sysCommServices.SysUpdate(infos);
        }
        /// <summary>
        /// 系统信息查询
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("SysSelect")][Authorize]
        public async Task<WebApiCallBack> SysSelect(sInfo infos)
        {

            string s = JsonConvert.SerializeObject(infos).ToLower();
            bool b = await _toolsServices.IllegalSqlContainsAny(s);
            if (b)
                return new WebApiCallBack() { code = 1, status = false, msg = "非法访问" };
            return await _sysCommServices.SysSelect(infos);
        }
        /// <summary>
        /// 系统信息插入
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("SysInsert")][Authorize]
        public async Task<WebApiCallBack> SysInsert(iInfo infos)
        {
            string s = JsonConvert.SerializeObject(infos).ToLower();
            bool b = await _toolsServices.IllegalSqlContainsAny(s);
            if (b)
                return new WebApiCallBack() { code = 1, status = false, msg = "非法访问" };
            return await _sysCommServices.SysInsert(infos);
        }
        /// <summary>
        /// 系统信息删除
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("SysDelete")][Authorize]
        public async Task<WebApiCallBack> SysDelete(dInfo infos)
        {
            string s = JsonConvert.SerializeObject(infos).ToLower();
            bool b = await _toolsServices.IllegalSqlContainsAny(s);
            if (b)
                return new WebApiCallBack() { code = 1, status = false, msg = "非法访问" };
            return await _sysCommServices.SysDelete(infos);
        }
        /// <summary>
        /// 系统信息隐藏
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("SysHide")][Authorize]
        public async Task<WebApiCallBack> SysHide(hideInfo infos)
        {
            string s = JsonConvert.SerializeObject(infos).ToLower();
            bool b = await _toolsServices.IllegalSqlContainsAny(s);
            if (b)
                return new WebApiCallBack() { code = 1, status = false, msg = "非法访问" };
            return await _sysCommServices.SysHide(infos);
        }
        /// <summary>
        /// 系统信息整表保存
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("SysSaveDT")][Authorize]
        public async Task<WebApiCallBack> SysSaveDT(SaveTableInfo infos)
        {
            string s = JsonConvert.SerializeObject(infos).ToLower();
            bool b = await _toolsServices.IllegalSqlContainsAny(s);
            if (b)
                return new WebApiCallBack() { code = 1, status = false, msg = "非法访问" };
            return await _sysCommServices.SysSaveDT(infos);
        }


        #endregion



        #region 新系统基本增删改查方法



        /// <summary>
        /// 系统信息修改
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("baseUpdate")]
        [Authorize]
        public async Task<WebApiCallBack> BaseUpdate(uInfo infos)
        {
            string s = JsonConvert.SerializeObject(infos).ToLower();
            bool b = await _toolsServices.IllegalSqlContainsAny(s);
            if (b)
                return new WebApiCallBack() { code = 1, status = false, msg = "非法访问" };
            return await _sysCommServices.SysUpdate(infos);
        }
        /// <summary>
        /// 系统信息查询
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("baseSelect")]
        [Authorize]
        public async Task<WebApiCallBack> BaseSelect(sInfo infos)
        {
            string s = JsonConvert.SerializeObject(infos).ToLower();
            bool b = await _toolsServices.IllegalSqlContainsAny(s);
            if (b)
                return new WebApiCallBack() { code = 1, status = false, msg = "非法访问" };
            return await _sysCommServices.SysSelect(infos);
        }
        /// <summary>
        /// 系统信息插入
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("baseInsert")]
        [Authorize]
        public async Task<WebApiCallBack> BaseInsert(iInfo infos)
        {
            string s = JsonConvert.SerializeObject(infos).ToLower();
            bool b = await _toolsServices.IllegalSqlContainsAny(s);
            if (b)
                return new WebApiCallBack() { code = 1, status = false, msg = "非法访问" };
            return await _sysCommServices.SysInsert(infos);
        }
        /// <summary>
        /// 系统信息删除
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("baseDelete")]
        [Authorize]
        public async Task<WebApiCallBack> BaseDelete(dInfo infos)
        {
            string s = JsonConvert.SerializeObject(infos).ToLower();
            bool b = await _toolsServices.IllegalSqlContainsAny(s);
            if (b)
                return new WebApiCallBack() { code = 1, status = false, msg = "非法访问" };
            return await _sysCommServices.SysDelete(infos);
        }
        /// <summary>
        /// 系统信息隐藏
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("baseHide")]
        [Authorize]
        public async Task<WebApiCallBack> BaseHide(hideInfo infos)
        {
            string s = JsonConvert.SerializeObject(infos).ToLower();
            bool b = await _toolsServices.IllegalSqlContainsAny(s);
            if (b)
                return new WebApiCallBack() { code = 1, status = false, msg = "非法访问" };
            return await _sysCommServices.SysHide(infos);
        }
        /// <summary>
        /// 系统信息整表保存
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [HttpPost, Route("baseSaveDT")]
        [Authorize]
        public async Task<WebApiCallBack> BaseSaveDT(SaveTableInfo infos)
        {
            string s = JsonConvert.SerializeObject(infos).ToLower();
            bool b = await _toolsServices.IllegalSqlContainsAny(s);
            if (b)
                return new WebApiCallBack() { code = 1, status = false, msg = "非法访问" };
            return await _sysCommServices.SysSaveDT(infos);
        }


        #endregion


    }
}
