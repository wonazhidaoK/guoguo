using System.Collections.Generic;

namespace GuoGuoCommunity.Domain.Models.Enum
{
    /// <summary>
    /// 部门
    /// </summary>
    public class Department
    {
        static Department()
        {
            YeZhu = new Department { Name = "业主", Value = "Owner" };
            YeZhuWeiYuanHui = new Department { Name = "业主委员会", Value = "VipOwner" };
            WuYe = new Department { Name = "物业", Value = "Property" };
            JieDaoBan = new Department { Name = "街道办", Value = "StreetOffice" };
        }
        
        public string Name { get; set; }

        public string Value { get; set; }

        /// <summary>
        /// 业主
        /// </summary>
        public static Department YeZhu { get; set; }

        /// <summary>
        /// 业主委员会
        /// </summary>
        public static Department YeZhuWeiYuanHui { get; set; }

        /// <summary>
        /// 物业
        /// </summary>
        public static Department WuYe { get; set; }

        /// <summary>
        /// 街道办
        /// </summary>
        public static Department JieDaoBan { get; set; }

        public static IEnumerable<Department> GetAll() => new List<Department>() { YeZhu, YeZhuWeiYuanHui, WuYe, JieDaoBan };

        public static IEnumerable<Department> GetAllForVipOwner() => new List<Department>() { WuYe, JieDaoBan };

        public static IEnumerable<Department> GetAllForOwner() => new List<Department>() { YeZhuWeiYuanHui, WuYe };

        public static IEnumerable<Department> GetAllForComplaintType() => new List<Department>() { YeZhuWeiYuanHui, YeZhu };
    }
}
