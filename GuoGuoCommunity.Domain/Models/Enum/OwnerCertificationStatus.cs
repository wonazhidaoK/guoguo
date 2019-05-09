using System.Collections.Generic;

namespace GuoGuoCommunity.Domain.Models.Enum
{
    /// <summary>
    /// 业主认证状态
    /// </summary>
    public class OwnerCertificationStatus
    {
        static OwnerCertificationStatus()
        {
            Executing = new OwnerCertificationStatus { Name = "认证中", Value = "Executing" };
            Success = new OwnerCertificationStatus { Name = "认证成功", Value = "Success" };
            Failure = new OwnerCertificationStatus { Name = "认证失败", Value = "Failure" };
        }

        public string Name { get; set; }

        public string Value { get; set; }

        /// <summary>
        /// 认证中
        /// </summary>
        public static OwnerCertificationStatus Executing { get; set; }

        /// <summary>
        /// 认证成功
        /// </summary>
        public static OwnerCertificationStatus Success { get; set; }

        /// <summary>
        /// 认证失败
        /// </summary>
        public static OwnerCertificationStatus Failure { get; set; }


        public static IEnumerable<OwnerCertificationStatus> GetAll() => new List<OwnerCertificationStatus>() { Executing, Success, Failure };

    }
}
