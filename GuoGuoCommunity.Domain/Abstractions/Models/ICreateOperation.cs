using System;

namespace GuoGuoCommunity.Domain.Abstractions.Models
{
    /*
     * 构建实体类时，记录创建记录信息 
     * 继承该基类，减少代码量
     */
    /// <summary>
    /// 创建操作基类字段
    /// </summary>
    public interface ICreateOperation
    {
        string CreateOperationUserId { get; set; }

        DateTimeOffset? CreateOperationTime { get; set; }
    }
}
