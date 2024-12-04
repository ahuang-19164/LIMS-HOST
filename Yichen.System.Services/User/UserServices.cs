using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Nito.AsyncEx;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
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
using Yichen.System.IServices;
using Yichen.System.Model;
using Yichen.System.Model.Entities;
using Yichen.Net.Utility;
using Yichen.Net.Auth.OverWrite;
using Yichen.Net.Utility.Extensions;

namespace Yichen.System.Services.User
{
    public partial class UserServices : BaseServices<sys_user>, IUserServices
    {
        private readonly AsyncLock _mutex = new AsyncLock();
        private readonly IHttpContextUser _httpContextUser;
        private readonly PermissionRequirement _permissionRequirement;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserLogServices _UserLogServices;
        private readonly IUserRepository _UserRepository;
        private readonly IRoleRepository _RoleRepository;
        private readonly IRoleMenuRepository _RoleMenuRepository;


        public UserServices(IUnitOfWork unitOfWork
            , IHttpContextUser httpContextUser
            , PermissionRequirement permissionRequirement
            , IHttpContextAccessor httpContextAccessor
            , IUserLogServices UserLogServices
            , IUserRepository UserRepository
            , IRoleRepository RoleRepository
            , IRoleMenuRepository RoleMenuRepository
            )
        {
            _httpContextUser = httpContextUser;
            _permissionRequirement = permissionRequirement;
            _httpContextAccessor = httpContextAccessor;
            _UserLogServices = UserLogServices;
            _UserRepository = UserRepository;
            _RoleRepository = RoleRepository;
            _RoleMenuRepository = RoleMenuRepository;

        }





        /// <summary>
        /// WebApi登陆
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <param name="verificationCode"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> Login(LoginInfo loginInfo, bool verificationCode = true)
        {
            var jm = new WebApiCallBack();


            if (string.IsNullOrEmpty(loginInfo.UserNo) || string.IsNullOrEmpty(loginInfo.Password))
            {
                jm.code = 1;
                jm.status = false;
                jm.msg = "用户名或密码不能为空";
                return jm;
            }

            //model.password = CommonHelper.Md5For32(model.password);

            var userInfo = await _UserRepository.UserLogin(loginInfo.UserNo, loginInfo.Password);
            if (userInfo != null)
            {
                if (!userInfo.state)
                {
                    jm.msg = "您的账户已经被冻结,请联系管理员解锁";
                    jm.code = 1;
                    jm.status = false;
                    return jm;
                }
                var userRoles = await _RoleRepository.GetUserRole(userInfo.roleNO);

                string[] userMenu = userRoles.powerList.Split(',');
                //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
                var claims = new List<Claim> {
                        new Claim("userNo",loginInfo.UserNo),
                        new Claim("userName",userInfo.names),
                        new Claim("Role", userInfo.roleNO.ToString()),
                        new Claim("WebRole", userInfo.webRoleNO.ToString()),
                        new Claim(ClaimTypes.GivenName, userInfo.no.ToString()),
                        //new Claim(ClaimTypes.Name, userInfo.userNO.ToString()),
                        //new Claim(ClaimTypes.NameIdentifier, userInfo.names.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, userInfo.id.ToString()),
                        new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_permissionRequirement.Expiration.TotalSeconds).ToString()) };
                claims.AddRange(userMenu.Select(s => new Claim(ClaimTypes.Role, s)));

                // ids4和jwt切换
                // jwt
                if (!Permissions.IsUseIds4)
                {
                    var data = await _RoleMenuRepository.GetRoleMenu(userMenu);
                    var list = (from item in data
                                orderby item.id
                                select new PermissionItem
                                {
                                    No=item.no,
                                    Authority = item.tagValue,
                                    //Url = item.menu?.component,
                                    //RouteUrl = item.menu?.path,
                                    //Authority = item.menu?.authority,
                                    //Role = item.role?.roleCode,
                                }).ToList();

                    _permissionRequirement.Permissions = list;
                }

                //用户标识
                var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
                identity.AddClaims(claims);

                var Authtoken = JwtToken.BuildJwtToken(claims.ToArray(), _permissionRequirement);

                jm.code = 0;
                jm.msg = "认证成功";
                jm.data = new
                {
                    Authtoken,
                    userInfo
                };

                //插入登录日志
                var log = new sys_user_log();
                log.names = loginInfo.UserNo;
                log.ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress != null ?
                    _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString() : "127.0.0.1";

                //log.os = RuntimeInformation.OSDescription; //登录系统类型

                //登录浏览器
                //if (_httpContextAccessor.HttpContext != null)
                //    log.browser = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.UserAgent];
                log.parameters = "登录成功";
                log.createTime = DateTime.Now;
                await _UserLogServices.InsertAsync(log);
                return jm;
            }
            else
            {
                //插入登录日志
                var log = new sys_user_log();
                log.names = loginInfo.UserNo;
                log.ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress != null ?
                    _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString() : "127.0.0.1";

                //log.os = RuntimeInformation.OSDescription;//登录系统类型


                //登录浏览器
                //if (_httpContextAccessor.HttpContext != null)
                //    log.browser = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.UserAgent];
                log.parameters = "登录失败";
                log.createTime = DateTime.Now;
                await _UserLogServices.InsertAsync(log);
                jm.code = 1;
                jm.status = false;
                jm.msg = "账户密码错误";
                return jm;
            }


        }


        #region 修改用户密码，如果用户之前没有密码，那么就不校验原密码
        /// <summary>
        ///   修改用户密码，如果用户之前没有密码，那么就不校验原密码
        /// </summary>
        public async Task<WebApiCallBack> ChangePassword(int userId, string newPwd, string password = "")
        {
            var jm = new WebApiCallBack();

            var user =_UserRepository.QueryByClause(p => p.no == userId);

            string aaa = _httpContextUser.GetToken();

            //Console.WriteLine(aaa);

            List<string> vv = _httpContextUser.GetClaimValueByTypes("GroupSid");
            //foreach (string ssa in vv)
            //{
            //    Console.WriteLine(ssa);
            //}


            if (user == null)
            {
                jm.msg = GlobalErrorCodeVars.Code10000;
                return jm;
            }

            //if (!string.IsNullOrEmpty(user.pwd))
            ////if (user.pwd==null)
            //{
            //  if (string.IsNullOrEmpty(password))
            //  {
            //      jm.msg = "请输入原密码!";
            //      return jm;
            //  }
            //  if (user.pwd != CommonHelper.EnPassword(password, user.createDate))
            //  {
            //      jm.msg = "原密码不正确!";
            //      return jm;
            //  }
            //}



            //if (string.IsNullOrEmpty(password))
            //{
            //    jm.msg = "请输入原密码!";
            //    return jm;
            //}
            //if (user.pwd != CommonHelper.EnPassword(password, user.createDate))
            //{
            //    jm.msg = "原密码不正确!";
            //    return jm;
            //}

            if (string.IsNullOrEmpty(newPwd) || newPwd.Length < 6 || newPwd.Length > 16)
            {
                jm.msg = GlobalErrorCodeVars.Code11009;
                return jm;
            }

            var md5Pwd = CommonHelper.EnPassword(newPwd, user.createDate);

            //if (!string.IsNullOrEmpty(user.pwd) && user.pwd == md5Pwd)
            //{
            //    jm.msg = "原密码和新密码一样!";
            //    return jm;
            //}
            //await DbClient.Updateable<CoreCmsAgentGrade>().SetColumns(it => it.isDefault == false).Where(p => p.isDefault == true && p.id != entity.id).ExecuteCommandAsync();
            var bl = await _UserRepository.UpdateAsync(new sys_user() { pwd = md5Pwd, lastModifyPwdDate = DateTime.Now, id = userId });
            jm.status = true;
            jm.msg = bl.code==0 ? "密码修改成功!" : "密码修改失败!";
            return jm;
        }
        #endregion




        #region 生成验证码方法
        /// <summary>
        /// 2020.06.15增加登陆验证码
        /// </summary>
        /// <returns></returns>
        public WebApiCallBack GetVierificationCode()
        {
            var jm = new WebApiCallBack();

            try
            {
                string user = _httpContextUser.Name;
                //录入登录日志
                //var log = new sys_userlog();
                //log.userId = 0;
                //log.names = "ceshi";
                //log.state = (int)GlobalEnumVars.UserLogTypes.登录;
                //log.ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress != null ? _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString() : "127.0.0.1";
                //log.createTime = DateTime.Now;
                //log.parameters = GlobalEnumVars.UserLogTypes.登录.ToString();
                //_UserLogServices.InsertAsync(log);





                string code = VierificationCode.RandomText();
                var data = new
                {
                    user = user,
                    img = VierificationCode.CreateBase64Imgage(code),
                    uuid = Guid.NewGuid()
                };

                ManualDataCache.Instance.Set(data.uuid.ToString(), code, 5);
                //HttpContext.GetService<IMemoryCache>().Set(data.uuid.ToString(), code, new TimeSpan(0, 5, 0));
                jm.msg = "验证码获取成功";
                jm.data = data;
            }
            catch (Exception ex)
            {
                jm.msg = ex.Message + ex.StackTrace;

            }
            return jm;
        }

        #endregion


        #region 获取用户信息

        /// <summary>
        /// 个人中心获取当前用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetUserInfo()
        {
            var jm = new WebApiCallBack();

            int id = _httpContextUser.ID;

            Console.WriteLine("访问用户id:"+ id);


            string usernnn = _httpContextUser.GetClaimValueByType("userNO")[0].ToString();

            Console.WriteLine("userNO:" + usernnn);

            string name = _httpContextUser.Name;
            var user =_UserRepository.QueryByClause(p => p.id == id);
            jm.msg = "访问成功";
            jm.data = user;
            return jm;
        }

        #endregion



        /// <summary>
        /// 请求刷新Token（以旧换新）
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> RefreshToken()
        {

            string token = _httpContextUser.GetToken();
            Console.WriteLine($"refresh token:{token} ");
            var jm = new WebApiCallBack();
            if (string.IsNullOrEmpty(token))
            {
                jm.code = 0;
                jm.status = false;
                jm.msg = "token无效，请重新登录！";
                return jm;
            }
            LoginInfo loginInfo = new LoginInfo();
            var tokenModel = JwtHelper.SerializeJwt(token);
            var log = new sys_user_log();

            if (tokenModel != null && tokenModel.Uid > 0)
            {

                var userInfo = await _UserRepository.QueryByIdAsync(tokenModel.Uid);
                if (userInfo != null)
                {
                    var userRoles = await _RoleRepository.GetUserRole(userInfo.roleNO);
                    string[] userMenu = userRoles.powerList.Split(',');
                    //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
                    var claims = new List<Claim> {
                        //new Claim("name", tokenModel.names.ToString()),
                        //new Claim("userNo", tokenModel.userNo.ToString()),
                        //new Claim("Role", tokenModel.Role.ToString()),
                        //new Claim("WebRole", tokenModel.WebRole.ToString()),
                        new Claim("userNo",tokenModel.userNo),
                        new Claim("userName",userInfo.names),
                        new Claim("Role", userInfo.roleNO.ToString()),
                        new Claim("WebRole", userInfo.webRoleNO.ToString()),
                        new Claim(ClaimTypes.Name, userInfo.userNO),
                        new Claim(JwtRegisteredClaimNames.Jti, tokenModel.Uid.ObjectToString()),
                        new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_permissionRequirement.Expiration.TotalSeconds).ToString()) };
                    claims.AddRange(userMenu.Select(s => new Claim(ClaimTypes.Role, s)));

                    //用户标识
                    var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
                    identity.AddClaims(claims);

                    var refreshToken = JwtToken.BuildJwtToken(claims.ToArray(), _permissionRequirement);
                    jm.code = 0;
                    jm.msg = "认证成功";
                    jm.data = new
                    {
                        refreshToken,
                        userInfo
                    };

                    //插入登录日志

                    log.names = loginInfo.UserNo;
                    log.ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress != null ?
                        _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString() : "127.0.0.1";

                    //log.os = RuntimeInformation.OSDescription; //登录系统类型

                    //登录浏览器
                    //if (_httpContextAccessor.HttpContext != null)
                    //    log.browser = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.UserAgent];
                    log.parameters = "登录成功";
                    log.createTime = DateTime.Now;
                    await _UserLogServices.InsertAsync(log);
                    return jm;
                }
            }

            log.names = loginInfo.UserNo;
            log.ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress != null ?
                _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString() : "127.0.0.1";

            //log.os = RuntimeInformation.OSDescription;//登录系统类型


            //登录浏览器
            //if (_httpContextAccessor.HttpContext != null)
            //    log.browser = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.UserAgent];
            log.parameters = "登录失败";
            log.createTime = DateTime.Now;
            await _UserLogServices.InsertAsync(log);
            jm.code = 1;
            jm.status = false;
            jm.msg = "账户密码错误";
            return jm;
        }
    }
}
