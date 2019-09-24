using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuoGuoCommunity.API.Models.Shop
{
    public class UpdateShopSignInput
    {
        public string Id { get; set; }

        /// <summary>
        /// 商家开启的活动标记 0不开启活动，1开启满减活动，1,2开启满减和某其他活动
        /// </summary>
        public string ActivitySign { get; set; }
    }
}