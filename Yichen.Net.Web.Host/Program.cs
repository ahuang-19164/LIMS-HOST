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
    /// ������
    /// </summary>
    public class Program
    {
        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            try
            {
                //ȷ��NLog.config�������ַ�����appsettings.json��ͬ��
                NLogUtil.EnsureNlogConfig("NLog.config");
                //������Ŀ����ʱ��Ҫ��������
                NLogUtil.WriteAll(NLog.LogLevel.Trace, LogType.Web, "�ӿ�����", "�ӿ������ɹ�");

                host.Run();
                //Console.WriteLine("�������ڼ��ػ��档��");
                //StartupHelper.loadbaseinfo();
                //Console.WriteLine("����������");
                //Console.WriteLine("HLIMS���������ɹ���");
            }
            catch (Exception ex)
            {
                //ʹ��nlogд��������־�ļ�����һ���ݿ�û����/���ӳɹ���
                NLogUtil.WriteFileLog(NLog.LogLevel.Error, LogType.ApiRequest, "�ӿ�����", "��ʼ�������쳣", ex);
                throw;
            }
        }

        /// <summary>
        ///     ��������֧��
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory()) //<--NOTE THIS
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders(); //�Ƴ��Ѿ�ע���������־�������
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
                        .UseKestrel().UseUrls("http://*:9600;http://*:9610;http://*:5000")
                        .UseStartup<Startup>();
                });
        }
    }
}