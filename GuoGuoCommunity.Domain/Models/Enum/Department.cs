using System.Collections.Generic;

namespace GuoGuoCommunity.Domain.Models.Enum
{
    public class Department
    {
        static Department()
        {
            YeZhu = new Department { Name = "业主", Value = "YeZhu" };
            YeZhuWeiYuanHui = new Department { Name = "业主委员会", Value = "YeZhuWeiYuanHui" };
            WuYe = new Department { Name = "物业", Value = "WuYe" };
            JieDaoBan = new Department { Name = "街道办", Value = "JieDaoBan" };
        }
        
        public string Name { get; set; }
        public string Value { get; set; }
        public static Department YeZhu { get; set; }

        public static Department YeZhuWeiYuanHui { get; set; }

        public static Department WuYe { get; set; }

        public static Department JieDaoBan { get; set; }

        public IEnumerable<Department> GetAll() => new List<Department>() { YeZhu, YeZhuWeiYuanHui, WuYe, JieDaoBan };
        
    }
}
