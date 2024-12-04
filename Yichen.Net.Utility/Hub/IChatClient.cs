/***********************************************************************
 *            Project: Yichen.Net                                     *
 *                Web: https://Yichen.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *         CreateTime: 2020-09-06 23:39:07
 *        Description: 暂无
 ***********************************************************************/


using System.Threading.Tasks;

namespace Yichen.Net.Utility.Hub
{
    public interface IChatClient
    {
        /// <summary>
        /// SignalR接收信息
        /// </summary>
        /// <param name="message">信息内容</param>
        /// <returns></returns>
        Task ReceiveMessage(object message);

        /// <summary>
        /// SignalR接收信息
        /// </summary>
        /// <param name="user">指定接收客户端</param>
        /// <param name="message">信息内容</param>
        /// <returns></returns>
        Task ReceiveMessage(string user, string message);

        Task ReceiveUpdate(object message);
    }
}
