using System;

namespace GuoGuoCommunity.Domain.Abstractions.Models
{
    /*
     * 构建实体类时，记录删除记录信息 
     * 继承该基类，减少代码量
     */
    /// <summary>
    /// 删除操作基类字段
    /// </summary>
    public interface IDeleted
    {
        bool IsDeleted { get; set; }

        DateTimeOffset? DeletedTime { get; set; }
    }
}
