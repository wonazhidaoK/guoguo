using System;

namespace GuoGuoCommunity.Domain.Abstractions.Models
{
    /// <summary>
    /// 投诉类型
    /// </summary>
    public interface IComplaintType
    {
        Guid ComplaintTypeId { get; set; }
    }
}
