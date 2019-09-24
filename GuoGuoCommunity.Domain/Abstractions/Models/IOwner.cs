using System;

namespace GuoGuoCommunity.Domain.Abstractions.Models
{
    /// <summary>
    /// 业主
    /// </summary>
    public interface IOwner
    {
        Guid? OwnerId { get; set; }
    }
}
