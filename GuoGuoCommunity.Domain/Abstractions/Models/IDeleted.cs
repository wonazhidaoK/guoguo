using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions.Models
{
    public  interface IDeleted
    {
        bool IsDeleted { get; set; }

        DateTimeOffset? DeletedTime { get; set; }
    }
}
