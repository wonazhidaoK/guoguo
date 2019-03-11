using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain
{
    public class Payload
    {
        //使用者資訊
        public User info { get; set; }
        //過期時間
        public int exp { get; set; }
    }
}
