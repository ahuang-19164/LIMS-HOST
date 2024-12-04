using Yichen.Comm.IRepository;
using Yichen.Comm.IServices;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Net.Data;

namespace Yichen.Stores.IServices
{
    public interface ISysCommServices : IBaseServices<object>
    {
        /// <summary>
        /// 系统信息修改
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> SysUpdate(uInfo infos);
        /// <summary>
        /// 系统信息查询
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> SysSelect(sInfo infos);
        /// <summary>
        /// 系统信息插入
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> SysInsert(iInfo infos);
        /// <summary>
        /// 系统信息删除
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> SysDelete(dInfo infos);
        /// <summary>
        /// 系统信息隐藏
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> SysHide(hideInfo infos);
        /// <summary>
        /// 系统信息整表保存
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> SysSaveDT(SaveTableInfo infos);
    }
}
