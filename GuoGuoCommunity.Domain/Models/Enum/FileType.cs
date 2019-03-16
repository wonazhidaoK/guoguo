using System.Collections.Generic;

namespace GuoGuoCommunity.Domain.Models.Enum
{
    /// <summary>
    /// 文件类型
    /// </summary>
    public class FileType
    {
        static FileType()
        {
            YeZhu = new Department { Name = "业主", Value = "Owner" };
            YeZhuWeiYuanHui = new Department { Name = "业主委员会", Value = "VipOwner" };
            WuYe = new Department { Name = "物业", Value = "Property" };
            JieDaoBan = new Department { Name = "街道办", Value = "StreetOffice" };
        }
        
        public string Name { get; set; }

        public string Value { get; set; }

        public static Department YeZhu { get; set; }

        public static Department YeZhuWeiYuanHui { get; set; }

        public static Department WuYe { get; set; }

        public static Department JieDaoBan { get; set; }

        public static IEnumerable<Department> GetAll() => new List<Department>() { YeZhu, YeZhuWeiYuanHui, WuYe, JieDaoBan };
        
    }
}
