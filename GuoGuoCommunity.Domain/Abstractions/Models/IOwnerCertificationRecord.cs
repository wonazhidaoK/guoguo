using System;

namespace GuoGuoCommunity.Domain.Abstractions.Models
{
    /// <summary>
    /// 业主申请记录
    /// </summary>
    public interface IOwnerCertificationRecord
    {
        Guid OwnerCertificationRecordId { get; set; }
    }
}
