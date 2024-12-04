using Yichen.Comm.IServices;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.System.Model;

namespace Yichen.System.IServices
{
    public interface IUserServices : IBaseServices<sys_user>
    {
        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <param name="verificationCode"></param>
        /// <returns></returns>
        Task<WebApiCallBack> Login(LoginInfo loginInfo, bool verificationCode = true);
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        WebApiCallBack GetVierificationCode();

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="oldPwd">原密码</param>
        /// <param name="newPwd">新密码</param>
        /// <returns></returns>
        Task<WebApiCallBack> ChangePassword(int userId, string newPwd, string password = "");
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> GetUserInfo();
        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>

        Task<WebApiCallBack> RefreshToken();


        //Task<WebApiCallBack> GetCurrentUserInfo();
    }
}
