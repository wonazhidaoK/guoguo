using System;

namespace GuoGuoCommunity.Domain.Abstractions.Models
{
    public interface ILastOperation
    {
        string LastOperationUserId { get; set; }

        DateTimeOffset? LastOperationTime { get; set; }
    }
}
