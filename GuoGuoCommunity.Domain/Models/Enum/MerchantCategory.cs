using System.Collections.Generic;

namespace GuoGuoCommunity.Domain.Models.Enum
{
    /// <summary>
    /// 商超类型
    /// </summary>
    public class MerchantCategory
    {
        static MerchantCategory()
        {
            Supermarket = new MerchantCategory { Name = "超市便利", Value = "Supermarket" };
            Fresh = new MerchantCategory { Name = "生鲜果蔬", Value = "Fresh" };
            Cake = new MerchantCategory { Name = "烘培蛋糕", Value = "Cake" };
            Plant = new MerchantCategory { Name = "鲜花绿植", Value = "Plant" };
            Medicine = new MerchantCategory { Name = "医药健康", Value = "Medicine" };
            HomeFashion = new MerchantCategory { Name = "家居时尚", Value = "HomeFashion" };
        }

        public string Name { get; set; }

        public string Value { get; set; }

        /// <summary>
        /// 超市便利
        /// </summary>
        public static MerchantCategory Supermarket { get; set; }

        /// <summary>
        /// 生鲜果蔬
        /// </summary>
        public static MerchantCategory Fresh { get; set; }

        /// <summary>
        /// 烘培蛋糕
        /// </summary>
        public static MerchantCategory Cake { get; set; }

        /// <summary>
        /// 鲜花绿植
        /// </summary>
        public static MerchantCategory Plant { get; set; }

        /// <summary>
        /// 医药健康
        /// </summary>
        public static MerchantCategory Medicine { get; set; }

        /// <summary>
        /// 家居时尚
        /// </summary>
        public static MerchantCategory HomeFashion { get; set; }

        public static IEnumerable<MerchantCategory> GetAll() => new List<MerchantCategory>() { Supermarket, Fresh, Cake, Plant, Medicine, HomeFashion };
    }
}
