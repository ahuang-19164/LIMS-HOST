
using Yichen.Comm.IServices;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Report.Model;

namespace Yichen.Report.IServices
{
    public interface IReportHandleServices : IBaseServices<object>
    {
        /// <summary>
        /// 上传报告接口
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> ReportUpload(UpLoadReportModel infos);


        /// <summary>
        /// 下载报告接口
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> ReportDown(GetReportModel infos);

    }
}
