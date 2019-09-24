namespace GuoGuoCommunity.Domain.Abstractions.Models
{
    /// <summary>
    /// 包含IDeleted,ICreateOperation,ILastOperation 的实体基类
    /// </summary>
    interface IEntitity : IDeleted, ICreateOperation, ILastOperation
    {
        //Guid Id { get; set; }
    }
}
