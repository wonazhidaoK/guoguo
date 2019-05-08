using System;

namespace GuoGuoCommunity.Domain.Abstractions.Models
{
    public interface IOwner
    {
        Guid? OwnerId { get; set; }
    }
}
