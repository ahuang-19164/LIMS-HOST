
using Yichen.Comm.IServices;
using Yichen.Comm.Model;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Files.Model;

namespace Yichen.Files.IServices
{
    public interface IFileHandleServices : IBaseServices<object>
    {


        #region 文件保存

        /// <summary>
        /// 通用文件保存
        /// </summary>
        /// <param name="filestring">文件字符串</param>
        /// <param name="filename">文件名称</param>
        /// <param name="filetype">文件类型</param>
        /// <returns></returns>
        Task<bool> fileSave(string filename, string filestring);
        /// <summary>
        /// 通用文件保存
        /// </summary>
        /// <param name="filestring">文件字符串</param>
        /// <param name="filename">文件名称</param>
        /// <param name="filetype">文件类型</param>
        /// <returns></returns>
        Task<bool> fileSave(string filename, string filestring, string filetype);



        #endregion

        #region 系统文件
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> SysUpFile(commInfoModel<FileModel> infos);

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> SysDownFile(commInfoModel<FileModel> infos);

        /// <summary>
        /// 上传flow文件
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> FlowUpFile(commInfoModel<FileModel> infos);
        /// <summary>
        /// 下载flow文件
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> FlowDownFile(commInfoModel<FileModel> infos);
        /// <summary>
        /// 上传检验结果文件
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> TestUpFile(commInfoModel<FileModel> infos);
        /// <summary>
        /// 下载检验结果文件
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> TestDownFile(commInfoModel<FileModel> infos);




        #endregion







        #region 系统更新


        /// <summary>
        /// 更新客户端
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<FileStream> UpgradeClient(DownFileModel infos);
        /// <summary>
        /// 更新报告客户端
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<FileStream> UpgradeReportClient(DownFileModel infos);
        /// <summary>
        /// 获取更新记录信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> UpgradeUpInfos(ClientInfoModel infos);


        #endregion
    }
}
