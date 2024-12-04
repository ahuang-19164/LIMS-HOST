using InitQ.Abstractions;
using InitQ.Attributes;
using NLog;
using System.Threading.Tasks;
using Yichen.Net.Configuration;
using Yichen.Net.Loging;

namespace Yichen.Net.RedisMQ.Subscribe
{
    /// <summary>
    /// 登录信息订阅
    /// </summary>
    public class LogingSubscribe : IRedisSubscribe
    {

        [Subscribe(RedisMessageQueueKey.LogingQueue)]
        private async Task SubRedisOrder2(string msg)
        {
            NLogUtil.WriteDbLog(LogLevel.Debug, LogType.RedisMessageQueue, "消息队列", $"接口端订阅从队列{RedisMessageQueueKey.LogingQueue} 接受到 消息:{msg}");

            await Task.CompletedTask;
        }


        [Subscribe(RedisMessageQueueKey.SmsQueue)]
        private async Task SubSmsQueue1(string msg)
        {
            NLogUtil.WriteDbLog(LogLevel.Debug, LogType.RedisMessageQueue, "消息队列", $"接口端订阅从队列{RedisMessageQueueKey.SmsQueue} 接受到 消息:{msg}");

            await Task.CompletedTask;
        }

    }
}
