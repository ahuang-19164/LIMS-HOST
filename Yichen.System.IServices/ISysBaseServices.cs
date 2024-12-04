using Yichen.Comm.IRepository;
using Yichen.Comm.IServices;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Net.Data;

namespace Yichen.System.IServices
{
    public interface ISysBaseServices : IBaseServices<object>
    {
        /// <summary>
        /// 系统信息修改
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> BaseUpdate(uInfo infos);
        /// <summary>
        /// 系统信息查询
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> BaseSelect(sInfo infos);
        /// <summary>
        /// 系统信息插入
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> BaseInsert(iInfo infos);
        /// <summary>
        /// 系统信息删除
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> BaseDelete(dInfo infos);
        /// <summary>
        /// 系统信息隐藏
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> BaseHide(hideInfo infos);
        /// <summary>
        /// 系统信息整表保存
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> BaseSaveDT(SaveTableInfo infos);
    }
}
