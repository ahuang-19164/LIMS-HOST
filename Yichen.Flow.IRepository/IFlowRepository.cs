using System.Data;
using Yichen.Comm.IRepository;
using Yichen.Flow.Model;
using Yichen.System.Model;

namespace Yichen.Flow.IRepository
{
    public interface IFlowRepository : IBaseRepository<comm_item_flow>
    {

        /// <summary>
        /// 获取流程信息
        /// </summary>
        /// <param name="flowNO"></param>
        /// <returns></returns>
        Task<comm_item_flow> GetFlowInfo(int flowNO);

        /// <summary>
        /// 获取流程信息
        /// </summary>
        /// <returns></returns>
        Task<DataTable> GetFlowInfoDT();
    }
}
