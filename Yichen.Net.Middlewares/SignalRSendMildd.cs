/***********************************************************************
 *            Project: Yichen.Net                                     *
 *                Web: https://Yichen.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *         CreateTime: 2020-09-06 23:37:04
 *        Description: 暂无
 ***********************************************************************/


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Yichen.Net.Utility.Hub;

namespace Yichen.Net.Middlewares
{
    /// <summary>
    /// 中间件
    /// SignalR发送数据
    /// </summary>
    public class SignalRSendMildd
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly RequestDelegate _next;
        private readonly IHubContext<ChatHub> _hubContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="hubContext"></param>
        public SignalRSendMildd(RequestDelegate next, IHubContext<ChatHub> hubContext)
        {
            _next = next;
            _hubContext = hubContext;
        }



        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);
        }

    }
}
