using System;

namespace GuoGuoCommunity.Domain.Abstractions.Models
{
    public interface ICreateOperation
    {
        string CreateOperationUserId { get; set; }

        DateTimeOffset? CreateOperationTime { get; set; }
    }
}
