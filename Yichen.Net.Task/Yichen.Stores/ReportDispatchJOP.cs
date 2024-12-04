using System.Threading.Tasks;
using Yichen.Jop.IServices;

namespace Yichen.Net.Tasks
{
    /// <summary>
    ///分发报告生成任务到Redis中
    /// </summary>
    public class ReportDispatchJOP
    {
        private readonly IReportDispatchServices _reportDispatchServices;

        public ReportDispatchJOP(IReportDispatchServices reportDispatchServices)
        {
            _reportDispatchServices = reportDispatchServices;
        }

        public async Task Execute()
        {
            await _reportDispatchServices.ReportDispatchJOP();
        }
    }
}
