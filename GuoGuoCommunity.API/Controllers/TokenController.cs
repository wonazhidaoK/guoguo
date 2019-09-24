using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Obsolete]
    [RoutePrefix("api/token")]
    public class TokenController : ApiController
    {
        private readonly ITokenRepository _tokenRepository;

        /// <summary>
        /// 
        /// </summary>
        public TokenController(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        //记录 Refresh Token，需记录在资料库
        private static readonly Dictionary<string, User> refreshTokens = new Dictionary<string, User>();

        /// <summary>
        /// 登陆
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("signIn")]
        public Token SignIn()
        {
            //模拟从资料库取得资料
            //if (!(model.UserId == "abc" && model.Password == "123"))
            //{
            //    throw new Exception("登入失败，账号或密码错误");
            //}
            var user = new User
            {
                Password = "123456",
                Name = "admin"

            };
            //产生 Token
            var token = _tokenRepository.Create(user);
            //需存入数据库
            refreshTokens.Add(token.Refresh_token, user);
            return token;
        }

        /// <summary>
        /// 换取新
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("refresh")]
        public Token Refresh([FromBody]string refreshToken)
        {
            //检查 Refresh Token 是否正确
            if (!refreshTokens.ContainsKey(refreshToken))
            {
                throw new Exception("查無此 Refresh Token");
            }
            //需查询资料库
            var user = refreshTokens[refreshToken];
            //产生一组新的 Token 和 Refresh Token
            var token = _tokenRepository.Create(user);
            //删除旧的
            refreshTokens.Remove(refreshToken);
            //存入新的
            refreshTokens.Add(token.Refresh_token, user);
            return token;
        }

        /// <summary>
        /// 测试是否通过验证
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("isAuthenticated")]
        public bool IsAuthenticated()
        {
            var token = HttpContext.Current.Request.Headers["Authorization"];
            var user = _tokenRepository.GetUser(token);
            if (user == null)
            {
                return false;
            }
            return true;
        }
    }
}
