using Yichen.Comm.IRepository;
using Yichen.Comm.IServices;

namespace Yichen.QC.IServices
{
    /// <summary>
    /// 质控判断
    /// </summary>
    public interface IQCResultJudge : IBaseServices<object>
    {
        /// <summary>
        /// 新增指控记录
        /// </summary>
        /// <param name="planid">计划id</param>
        /// <param name="planGradeid">质控品编号</param>
        /// <param name="itemNO">项目编号</param>
        /// <param name="sort">排序</param>
        /// <returns></returns>
        Task<bool> NewRestultJudge(string planid, string planGradeid, string itemNO, int sort);
    }
}
