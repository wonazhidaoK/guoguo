using System;

namespace GuoGuoCommunity.Domain.Abstractions.Models
{
    public  interface IDeleted
    {
        bool IsDeleted { get; set; }

        DateTimeOffset? DeletedTime { get; set; }
    }
}
