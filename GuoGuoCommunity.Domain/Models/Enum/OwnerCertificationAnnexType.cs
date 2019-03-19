using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Models.Enum
{
  public  class OwnerCertificationAnnexType
    {

        static OwnerCertificationAnnexType()
        {
            IDCardFront = new OwnerCertificationAnnexType { Name = "身份证正面", Value = "IDCardFront" };
            IDCardNegative = new OwnerCertificationAnnexType { Name = "身份证反面", Value = "IDCardNegative" };
            Deed = new OwnerCertificationAnnexType { Name = "房产证", Value = "Deed" };
        }

        public string Name { get; set; }

        public string Value { get; set; }

        /// <summary>
        /// 身份证正面
        /// </summary>
        public static OwnerCertificationAnnexType IDCardFront { get; set; }

        /// <summary>
        /// 身份证反面
        /// </summary>
        public static OwnerCertificationAnnexType IDCardNegative { get; set; }

        /// <summary>
        /// 房产证
        /// </summary>
        public static OwnerCertificationAnnexType Deed { get; set; }

        public static IEnumerable<OwnerCertificationAnnexType> GetAll() => new List<OwnerCertificationAnnexType>() { IDCardFront, IDCardNegative, Deed };
    }
}
