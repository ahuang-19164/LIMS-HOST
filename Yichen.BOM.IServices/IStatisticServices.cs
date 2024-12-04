using Yichen.BOM.Model;
using Yichen.Comm.IRepository;
using Yichen.Comm.IServices;
using Yichen.Comm.Model.ViewModels.UI;

namespace Yichen.BOM.IServices
{
    /// <summary>
    /// 综合查询
    /// </summary>
    public interface IStatisticServices : IBaseServices<object>
    {
        #region 系统文件
        /// <summary>
        /// 综合查询方法
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Task<WebApiCallBack> StatisticHandle(StatisticModel infos);

        #endregion
    }
}
