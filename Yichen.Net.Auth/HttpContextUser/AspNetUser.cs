/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Yichen.Net.Utility.Extensions;

namespace Yichen.Net.Auth.HttpContextUser
{
    public class AspNetUser : IHttpContextUser
    {
        private readonly IHttpContextAccessor _accessor;

        public AspNetUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        //public string Name => _accessor.HttpContext.User.Identity.Name;

        //public string Name => GetClaimValueByType("userName").FirstOrDefault().ObjectToString();
        //public string UserNo=> GetClaimValueByType("userNo").FirstOrDefault().ObjectToString();
        //public int ID => GetClaimValueByType("jti").FirstOrDefault().ObjectToInt();
        //public int Role => GetClaimValueByType("Role").FirstOrDefault().ObjectToInt();
        //public int WebRole => GetClaimValueByType("WebRole").FirstOrDefault().ObjectToInt();

        public string Name => GetClaimValueByType("userName").ObjectToString();
        public string UserNo => GetClaimValueByType("userNo").ObjectToString();
        public int ID => GetClaimValueByType("jti").ObjectToInt();
        public int Role => GetClaimValueByType("Role").ObjectToInt();
        public int WebRole => GetClaimValueByType("WebRole").ObjectToInt();

        /// <summary>
        /// 是否通过身份验证
        /// </summary>
        /// <returns></returns>
        public bool IsAuthenticated()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }
        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>
        public string? GetToken()

        {
            return _accessor.HttpContext.Request.Headers["Authorization"].ObjectToString().Replace("Bearer ", "");
        }

        public List<string> GetUserInfoFromTokens(string ClaimType)
        {

            var jwtHandler = new JwtSecurityTokenHandler();
            if (!string.IsNullOrEmpty(GetToken()))
            {
                JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(GetToken());

                return (from item in jwtToken.Claims
                        where item.Type == ClaimType
                        select item.Value).ToList();
            }
            else
            {
                return new List<string>() { };
            }
        }
        public string GetUserInfoFromToken(string ClaimType)
        {

            var jwtHandler = new JwtSecurityTokenHandler();
            if (!string.IsNullOrEmpty(GetToken()))
            {
                JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(GetToken());

                return (from item in jwtToken.Claims
                        where item.Type == ClaimType
                        select item.Value).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 获取token Claim 信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return _accessor.HttpContext.User.Claims;
        }

        public string GetClaimValueByType(string ClaimType)
        {

            return (from item in GetClaimsIdentity()
                    where item.Type == ClaimType
                    select item.Value).FirstOrDefault();

        }

        public List<string> GetClaimValueByTypes(string ClaimType)
        {

            return (from item in GetClaimsIdentity()
                    where item.Type == ClaimType
                    select item.Value).ToList();

        }
    }
}
