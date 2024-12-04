using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using Yichen.Net.Loging;

namespace Yichen.Net.Web.Host
{
    /// <summary>
    /// 启动类
    /// </summary>
    public class Program
    {
        /// <summary>
        ///     启动配置
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            try
            {
                //确保NLog.config中连接字符串与appsettings.json中同步
                NLogUtil.EnsureNlogConfig("NLog.config");
                //其他项目启动时需要做的事情
                NLogUtil.WriteAll(NLog.LogLevel.Trace, LogType.Web, "接口启动", "接口启动成功");

                host.Run();
                //Console.WriteLine("服务正在加载缓存。。");
                //StartupHelper.loadbaseinfo();
                //Console.WriteLine("缓存加载完成");
                //Console.WriteLine("HLIMS服务启动成功！");
            }
            catch (Exception ex)
            {
                //使用nlog写到本地日志文件（万一数据库没创建/连接成功）
                NLogUtil.WriteFileLog(NLog.LogLevel.Error, LogType.ApiRequest, "接口启动", "初始化数据异常", ex);
                throw;
            }
        }

        /// <summary>
        ///     创建启动支撑
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory()) //<--NOTE THIS
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders(); //移除已经注册的其他日志处理程序
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace); //设置最小的日志级别
                })
                .UseNLog() //NLog: Setup NLog for Dependency injection
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureKestrel(serverOptions =>
                        {
                            serverOptions.AllowSynchronousIO = true; //启用同步 IO
                            serverOptions.Limits.MaxRequestBodySize = 10485760; //服务器选项限制最大请求体大小
                        })
                        .UseKestrel().UseUrls("http://*:9600;http://*:9610;http://*:5000")
                        .UseStartup<Startup>();
                });
        }
    }
}