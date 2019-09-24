using System;

namespace GuoGuoCommunity.Domain.Abstractions.Models
{
    /*
     * 构建实体类时，记录最后操作记录信息 
     * 继承该基类，减少代码量
     */
    /// <summary>
    /// 最后操作基类字段
    /// </summary>
    public interface ILastOperation
    {
        string LastOperationUserId { get; set; }

        DateTimeOffset? LastOperationTime { get; set; }
    }
}
