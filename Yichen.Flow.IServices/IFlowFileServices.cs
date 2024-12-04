using Yichen.Comm.IRepository;
using Yichen.Comm.IServices;

namespace Yichen.Flow.IServices
{
    public interface IFlowFileServices : IBaseServices<object>
    {
        /// <summary>
        /// 系统信息修改
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<string> SysUpdate(string infos);
        /// <summary>
        /// 系统信息查询
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<string> SysSelect(string infos);
        /// <summary>
        /// 系统信息插入
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<string> SysInsert(string infos);
        /// <summary>
        /// 系统信息删除
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<string> SysDelete(string infos);
        /// <summary>
        /// 系统信息隐藏
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<string> SysHide(string infos);
        /// <summary>
        /// 系统信息整表保存
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<string> SysSaveDT(string infos);
    }
}
