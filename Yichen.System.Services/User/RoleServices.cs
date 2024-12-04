using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Nito.AsyncEx;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Yichen.Comm.IRepository.UnitOfWork;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Comm.Services;
using Yichen.Net.Auth.HttpContextUser;
using Yichen.Net.Auth.Policys;
using Yichen.Net.Caching.Manual;
using Yichen.Net.Configuration;
using Yichen.Net.Utility.Helper;
using Yichen.System.IRepository;
using Yichen.System.IServices.User;
using Yichen.System.Model;
using Yichen.System.Model.Entities;
using Yichen.Net.Utility;
using Yichen.System.IServices;

namespace Yichen.System.Services
{
    public partial class RoleServices : BaseServices<sys_role>, IRoleServices
    {
        private readonly AsyncLock _mutex = new AsyncLock();
        private readonly IHttpContextUser _httpContextUser;
        private readonly PermissionRequirement _permissionRequirement;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserLogServices _UserLogServices;
        private readonly IUserRepository _UserRepository;


        public RoleServices(IUnitOfWork unitOfWork
            , IHttpContextUser httpContextUser
            , PermissionRequirement permissionRequirement
            , IHttpContextAccessor httpContextAccessor
            , IUserLogServices UserLogServices
            , IUserRepository UserRepository
            )
        {
            _httpContextUser = httpContextUser;
            _permissionRequirement = permissionRequirement;
            _httpContextAccessor = httpContextAccessor;
            _UserLogServices = UserLogServices;
            _UserRepository = UserRepository;

        }

        public Task<WebApiCallBack> ChangePassword(int userId, string newPwd, string password = "")
        {
            throw new NotImplementedException();
        }

        public Task<WebApiCallBack> GetUserInfo()
        {
            throw new NotImplementedException();
        }

        public WebApiCallBack GetVierificationCode()
        {
            throw new NotImplementedException();
        }

        public Task<WebApiCallBack> Login(LoginInfo loginInfo, bool verificationCode = true)
        {
            throw new NotImplementedException();
        }
    }
}
