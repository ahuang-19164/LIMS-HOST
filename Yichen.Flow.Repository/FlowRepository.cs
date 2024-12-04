using Nito.AsyncEx;
using System.Data;
using Yichen.Comm.IRepository.UnitOfWork;

using Yichen.Comm.Repository;
using Yichen.Flow.IRepository;
using Yichen.Flow.Model;
using Yichen.System.Model;

namespace Yichen.Flow.Repository
{
    public class FlowRepository : BaseRepository<comm_item_flow>, IFlowRepository
    {
        private readonly AsyncLock _mutex = new AsyncLock();
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IHttpContextUser _httpContextUser;
        //private readonly PermissionRequirement _permissionRequirement;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        public FlowRepository(IUnitOfWork unitOfWork
       //, IHttpContextUser httpContextUser
       //, PermissionRequirement permissionRequirement
       //, IHttpContextAccessor httpContextAccessor
       //,IUserLogServices UserLogServices
       ) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            //_httpContextUser = httpContextUser;
            //_permissionRequirement = permissionRequirement;
            //_httpContextAccessor = httpContextAccessor;
            //_UserLogServices=UserLogServices;
        }


        public async Task<DataTable> GetFlowInfoDT()
        {
            var infos = await DbClient.Queryable<comm_item_flow>().ToDataTableAsync();
            return infos;
        }




        public async Task<List<comm_item_flow>> GetFlowInfoList()
        {
            List<comm_item_flow> infos = null;
            await Task.Run(() =>
            {
                infos = DbClient.Queryable<comm_item_flow>().ToList();

            });
            return infos;
        }

        public async Task<comm_item_flow> GetFlowInfo(int flowNO)
        {
            var infos = await DbClient.Queryable<comm_item_flow>().FirstAsync(p => p.no == flowNO);
            //string nextNO = infos.nextFlow!=null?infos.nextFlow.ToString():"";
            return infos;
        }


    }
}
