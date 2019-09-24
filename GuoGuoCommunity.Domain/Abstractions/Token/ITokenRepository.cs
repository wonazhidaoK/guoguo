using GuoGuoCommunity.Domain.Models;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface ITokenRepository
    {
        /// <summary>
        /// 创建Token
        /// </summary>
        /// <param name="user">需要加密的信息</param>
        /// <returns></returns>
        Token Create(User user);

        /// <summary>
        /// 根据Token 获取用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        User GetUser(string token);
    }
}
