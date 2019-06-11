using System;

namespace GuoGuoCommunity.Domain.Abstractions.Models
{
    interface IEntitity:IDeleted,ICreateOperation,ILastOperation
    {
        //Guid Id { get; set; }
    }
}
