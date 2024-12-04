using InitQ;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Yichen.Net.Configuration;
using Yichen.Net.RedisMQ.Subscribe;

namespace Yichen.Net.Core.Config
{
    /// <summary>
    /// Redis 消息队列 启动服务
    /// </summary>
    public static class RedisMessageQueueSetup
    {
        public static void AddRedisMessageQueueSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddInitQ(m =>
            {
                //时间间隔
                m.SuspendTime = 1000;
                //redis服务器地址
                m.ConnectionString = AppSettingsConstVars.RedisConfigConnectionString;
                //对应的订阅者类，需要new一个实例对象，当然你也可以传参，比如日志对象
                m.ListSubscribe = new List<Type>() {
                    typeof(OrderAgentOrDistributionSubscribe),
                    typeof(OrderAutomaticDeliverySubscribe),
                    typeof(OrderFinishCommandSubscribe),
                    typeof(OrderPrintSubscribe),

                    typeof(LogingSubscribe),

                    typeof(UserSubscribe),
                    typeof(WeChatPayNoticeSubscribe),
                    typeof(SendWxTemplateMessageSubscribe),
                    typeof(AfterSalesReviewSubscribe),
                };
                //显示日志
                m.ShowLog = false;
            });
        }
    }
}
