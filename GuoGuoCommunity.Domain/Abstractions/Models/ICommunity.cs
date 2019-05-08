using System;

namespace GuoGuoCommunity.Domain.Abstractions.Models
{
    /// <summary>
    /// 社区
    /// </summary>
    public interface ICommunity
    {
        Guid CommunityId { get; set; }
    }
}
