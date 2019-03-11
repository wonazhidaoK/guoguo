using GuoGuoCommunity.Domain;
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
    [RoutePrefix("api/token")]
    public class TokenController : ApiController
    {
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        public TokenController()
        {
            _tokenManager = new TokenManager();
        }

        //紀錄 Refresh Token，需紀錄在資料庫
        private static Dictionary<string, User> refreshTokens =
            new Dictionary<string, User>();

        //登入
        [HttpPost]
        [Route("signIn")]
        public Token SignIn( )
        {
            //模擬從資料庫取得資料
            //if (!(model.UserId == "abc" && model.Password == "123"))
            //{
            //    throw new Exception("登入失敗，帳號或密碼錯誤");
            //}
            var user = new User
            {
                 Password="123456",
                  Name="admin"
                  
            };
            //產生 Token
            var token = _tokenManager.Create(user);
            //需存入資料庫
            refreshTokens.Add(token.refresh_token, user);
            return token;
        }

        //換取新 Token
        [HttpPost]
        [Route("refresh")]
        public Token Refresh([FromBody]string refreshToken)
        {
            //檢查 Refresh Token 是否正確
            if (!refreshTokens.ContainsKey(refreshToken))
            {
                throw new Exception("查無此 Refresh Token");
            }
            //需查詢資料庫
            var user = refreshTokens[refreshToken];
            //產生一組新的 Token 和 Refresh Token
            var token = _tokenManager.Create(user);
            //刪除舊的
            refreshTokens.Remove(refreshToken);
            //存入新的
            refreshTokens.Add(token.refresh_token, user);
            return token;
        }

        /// <summary>
        /// 測試是否通過驗證
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("isAuthenticated")]
        public bool IsAuthenticated()
        {
            var token = HttpContext.Current.Request.Headers["Authorization"];
            var user = _tokenManager.GetUser(token);
            if (user == null)
            {
                return false;
            }
            return true;
        }
    }
}
