using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Dto
{
  public  class OrdeItemDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }

        public string OrderId { get; set; }

      
        public string ShopCommodityId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 商品折扣价
        /// </summary>
        public decimal? DiscountPrice { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int CommodityCount { get; set; }
    }
}
