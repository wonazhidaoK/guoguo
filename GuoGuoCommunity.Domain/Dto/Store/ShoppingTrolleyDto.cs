using System;

namespace GuoGuoCommunity.Domain.Dto.Store
{
    public class ShoppingTrolleyDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 店铺关联外键
        /// </summary>
        public string ShopId { get; set; }
        /// <summary>
        /// 店铺商品ID（外键）
        /// </summary>
        public string ShopCommodityId { get; set; }
        /// <summary>
        /// 用户认证ID（外键）
        /// </summary>
        public string OwnerCertificationRecordId { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        public int CommodityCount { get; set; }

        /// <summary>
        /// 操作人Id
        /// </summary>
        public string OperationUserId { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTimeOffset? OperationTime { get; set; }
    }
}
