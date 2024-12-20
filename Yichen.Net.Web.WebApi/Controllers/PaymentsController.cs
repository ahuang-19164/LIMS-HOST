﻿/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yichen.Net.Auth.HttpContextUser;
using Yichen.Net.Configuration;
using Yichen.Net.IServices;
using Yichen.Net.Model.FromBody;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Net.Model.DTO;
using Yichen.Net.Utility.Extensions;
using Yichen.Net.Utility.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Yichen.Net.Web.WebApi.Controllers
{
    /// <summary>
    /// 支付调用接口数据
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {

        private IHttpContextUser _user;
        private ICoreCmsBillPaymentsServices _billPaymentsServices;
        private ICoreCmsPaymentsServices _paymentsServices;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="user"></param>
        /// <param name="billPaymentsServices"></param>
        /// <param name="paymentsServices"></param>
        public PaymentsController(IHttpContextUser user
            , ICoreCmsBillPaymentsServices billPaymentsServices
            , ICoreCmsPaymentsServices paymentsServices
        )
        {
            _user = user;
            _billPaymentsServices = billPaymentsServices;
            _paymentsServices = paymentsServices;
        }

        //公共接口====================================================================================================

        #region 获取支付方式列表==================================================
        /// <summary>
        /// 获取支付方式列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> GetList()
        {
            var jm = new WebApiCallBack();

            var list = await _paymentsServices.QueryListByClauseAsync(p => p.isEnable == true, p => p.sort, OrderByType.Asc);
            jm.status = true;
            jm.data = list;
            return jm;

        }
        #endregion

        //验证接口====================================================================================================

        #region 支付确认页面取信息==================================================
        /// <summary>
        /// 支付确认页面取信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> CheckPay([FromBody] CheckPayPost entity)
        {
            var jm = new WebApiCallBack();

            if (string.IsNullOrEmpty(entity.ids))
            {
                jm.msg = GlobalErrorCodeVars.Code13100;
                return jm;
            }

            jm = await _billPaymentsServices.FormatPaymentRel(entity.ids, entity.paymentType, entity.@params);
            return jm;

        }
        #endregion


        #region 获取支付单详情==================================================
        /// <summary>
        /// 获取支付单详情
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GetInfo([FromBody] FMStringId entity)
        {
            var jm = new WebApiCallBack();
            if (string.IsNullOrEmpty(entity.id))
            {
                jm.msg = GlobalErrorCodeVars.Code13100;
                return jm;
            }
            var userId = entity.data.ObjectToInt(0);
            jm = await _billPaymentsServices.GetInfo(entity.id, userId);
            return jm;

        }
        #endregion

    }
}