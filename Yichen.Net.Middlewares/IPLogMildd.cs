/***********************************************************************
 *            Project: Yichen.Net                                     *
 *                Web: https://Yichen.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *           Versions: 1.0                                             *
 *         CreateTime: 2020-02-03 23:33:49
 *          NameSpace: Yichen.Net.Framework.Middlewares
 *           FileName: IPLogMildd
 *   ClassDescription:
 ***********************************************************************/


using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Yichen.Comm.Model.ViewModels.LogInfo;
using Yichen.Net.Configuration;
using Yichen.Net.Loging;
using Yichen.Net.Utility.Extensions;
using Yichen.Net.Utility.Helper;

namespace Yichen.Net.Middlewares
{
    /// <summary>
    /// 中间件
    /// 记录IP请求数据
    /// </summary>
    public class IPLogMildd
    {
        /// <summary>
        ///
        /// </summary>
        private readonly RequestDelegate _next;
        /// <summary>
        ///
        /// </summary>
        /// <param name="next"></param>
        public IPLogMildd(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (AppSettingsConstVars.MiddlewareIpLogEnabled)
            {
                // 过滤，只有接口
                if (context.Request.Path.Value != null && (context.Request.Path.Value.Contains("api") || context.Request.Path.Value.Contains("Api")))
                {
                    context.Request.EnableBuffering();
                    try
                    {
                        // 存储请求数据
                        var dt = DateTime.Now;
                        var request = context.Request;
                        StreamReader stream = new StreamReader(request.Body);
                        string body = stream.ReadToEnd();
                        var requestInfo = JsonConvert.SerializeObject(new RequestInfo()
                        {
                            Ip = GetClientIp(context),
                            Url = request.Path.ObjectToString().TrimEnd('/').ToLower(),
                            Datetime = dt.ToString("yyyy-MM-dd HH:mm:ss"),
                            Date = dt.ToString("yyyy-MM-dd"),
                            Week = CommonHelper.GetWeek(),
                            Data = body,
                        });

                        if (!string.IsNullOrEmpty(requestInfo))
                        {
                            Parallel.For(0, 1, e =>
                            {
                                LogLockHelper.OutSql2Log("RequestIpInfoLog", "RequestIpInfoLog" + dt.ToString("yyyy-MM-dd-HH"), new string[] { requestInfo + "," }, false);
                            });
                            request.Body.Position = 0;
                        }
                        await _next(context);
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    await _next(context);
                }
            }
            else
            {
                await _next(context);
            }
        }

        public static string GetClientIp(HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].ObjectToString();
            if (string.IsNullOrEmpty(ip))
            {
                if (context.Connection.RemoteIpAddress != null)
                {
                    ip = context.Connection.RemoteIpAddress.MapToIPv4().ObjectToString();
                }
            }
            return ip;
        }

    }
}
