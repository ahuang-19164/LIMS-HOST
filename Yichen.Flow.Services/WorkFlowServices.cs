using Yichen.Comm.IRepository.UnitOfWork;

using Yichen.Comm.Services;
using Yichen.System.Model;
using Yichen.Flow.IServices;

namespace Yichen.Flow.Services
{

    public class WorkFlowServices : BaseServices<comm_samplerecord>, IWorkFlowServices
    {
        public readonly IUnitOfWork _UnitOfWork;
        public WorkFlowServices(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }
        /// <summary>
        /// 获取流程节点信息
        /// </summary>
        /// <param name="FlowNO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<int> GetFlowInfo(string FlowNO)
        {
            throw new NotImplementedException();
        }

    }
}
