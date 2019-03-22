using GuoGuoCommunity.Domain.Models;

namespace GuoGuoCommunity.Domain
{
    public class Payload
    {
        //使用者信息
        public User Info { get; set; }
        //过期时间
        public int Exp { get; set; }
    }
}
