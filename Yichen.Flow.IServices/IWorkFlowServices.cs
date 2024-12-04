using Yichen.Comm.IRepository;
using Yichen.Comm.IServices;
using Yichen.System.Model;

namespace Yichen.Flow.IServices
{
    public interface IWorkFlowServices : IBaseServices<comm_samplerecord>
    {

        /// <summary>
        /// 获取流程节点信息
        /// </summary>
        /// <param name="FlowNO"></param>
        /// <returns></returns>
        Task<int> GetFlowInfo(string FlowNO);
    }
}
