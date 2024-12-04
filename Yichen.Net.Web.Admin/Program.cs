using Autofac.Extensions.DependencyInjection;
using Yichen.Net.Loging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;

namespace Yichen.Net.Web.Admin
{
    /// <summary>
    /// ��ʼ��
    /// </summary>
    public class Program
    {
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            try
            {
                //ȷ��NLog.config�������ַ�����appsettings.json��ͬ��
                NLogUtil.EnsureNlogConfig("NLog.config");
                //throw new Exception("�����쳣");//for test
                //������Ŀ����ʱ��Ҫ��������
                NLogUtil.WriteAll(NLog.LogLevel.Trace, LogType.Web, "��վ����", "��վ�����ɹ�");

                host.Run();
            }
            catch (Exception ex)
            {
                //ʹ��nlogд��������־�ļ�����һ���ݿ�û����/���ӳɹ���
                NLogUtil.WriteFileLog(NLog.LogLevel.Error, LogType.Web, "��վ����", "��ʼ�������쳣", ex);
                throw;
            }
        }

        /// <summary>
        /// ��������֧��
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                //Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory()) //<--NOTE THIS
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders(); //�Ƴ��Ѿ�ע���������־��������
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace); //������С����־����
                })
                .UseNLog() //NLog: Setup NLog for Dependency injection
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureKestrel(serverOptions =>
                        {
                            serverOptions.AllowSynchronousIO = true; //����ͬ�� IO
                            serverOptions.Limits.MaxRequestBodySize = 10485760; //������ѡ����������������С
                        })
                        .UseKestrel().UseUrls("http://*:5000")
                        .UseStartup<Startup>();
                });
    }
}