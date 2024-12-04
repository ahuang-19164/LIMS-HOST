

using Microsoft.AspNetCore.Http;
using Nito.AsyncEx;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Repository;
using Yichen.Comm.Services;
using Yichen.Net.Auth.HttpContextUser;
using Yichen.Net.Auth.Policys;

namespace Yichen.Flow.IServices
{
    public partial class FlowFileServices : BaseServices<object>, IFlowFileServices
    {
        private readonly AsyncLock _mutex = new AsyncLock();
        private readonly IHttpContextUser _httpContextUser;
        private readonly PermissionRequirement _permissionRequirement;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IUserLogServices _UserLogServices;

        public FlowFileServices(IUnitOfWork unitOfWork
            , IHttpContextUser httpContextUser
            , PermissionRequirement permissionRequirement
            , IHttpContextAccessor httpContextAccessor
            //,IUserLogServices UserLogServices
            )
        {
            _httpContextUser = httpContextUser;
            _permissionRequirement = permissionRequirement;
            _httpContextAccessor = httpContextAccessor;
            //_UserLogServices=UserLogServices;
        }

        public Task<string> SysDelete(string infos)
        {
            throw new NotImplementedException();
        }

        public Task<string> SysHide(string infos)
        {
            throw new NotImplementedException();
        }

        public Task<string> SysInsert(string infos)
        {
            throw new NotImplementedException();
        }

        public Task<string> SysSaveDT(string infos)
        {
            throw new NotImplementedException();
        }

        public Task<string> SysSelect(string infos)
        {
            throw new NotImplementedException();
        }

        public Task<string> SysUpdate(string infos)
        {
            throw new NotImplementedException();
        }
    }
}
