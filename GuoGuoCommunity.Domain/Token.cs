using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain
{
    public class Token
    {
        //Token
        public string access_token { get; set; }
        //Refresh Token
        public string refresh_token { get; set; }
        //幾秒過期
        public int expires_in { get; set; }
    }
}
