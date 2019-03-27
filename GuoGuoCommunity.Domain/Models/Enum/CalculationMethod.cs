using System.Collections.Generic;

namespace GuoGuoCommunity.Domain.Models.Enum
{
    /// <summary>
    /// 投票结果计算方式
    /// </summary>
    public class CalculationMethod
    {
        static CalculationMethod()
        {
            EndorsedNumber = new CalculationMethod { Name = "赞同人数", Value = "EndorsedNumber" };
            Opposition = new CalculationMethod { Name = "反对人数", Value = "Opposition" };
        }

        public string Name { get; set; }

        public string Value { get; set; }

        /// <summary>
        /// 反对人数不超过3/1
        /// </summary>
        public static CalculationMethod Opposition { get; set; }

        /// <summary>
        /// 赞同人数超过3/2
        /// </summary>
        public static CalculationMethod EndorsedNumber { get; set; }

        public static IEnumerable<CalculationMethod> GetAll() => new List<CalculationMethod>() { EndorsedNumber, Opposition };
    }
}
