using Yichen.Comm.IServices;
using Yichen.Comm.Model.ViewModels.UI;

namespace Yichen.Jop.IServices
{
    /// <summary>
    /// 报告分发服务
    /// </summary>
    public interface IReportDispatchServices : IBaseServices<object>
    {
        /// <summary>
        /// 分发定时任务
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> ReportDispatchJOP();
    }
}
