using System.Collections.Generic;

namespace GuoGuoCommunity.Domain.Models.Enum
{
    public class SalesType
    {
        static SalesType()
        {
            Shelf = new SalesType { Name = "上架", Value = "Shelf" };
            Obtained = new SalesType { Name = "下架", Value = "Obtained" };
        }
        public string Name { get; set; }

        public string Value { get; set; }

        /// <summary>
        /// 上架
        /// </summary>
        public static SalesType Shelf { get; set; }

        /// <summary>
        /// 下架
        /// </summary>
        public static SalesType Obtained { get; set; }

        public static IEnumerable<SalesType> GetAll() => new List<SalesType>() { Shelf, Obtained };
    }
}
