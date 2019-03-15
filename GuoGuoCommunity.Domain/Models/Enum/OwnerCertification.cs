using System.Collections.Generic;

namespace GuoGuoCommunity.Domain.Models.Enum
{
    /// <summary>
    /// 业主认证状态
    /// </summary>
    public class OwnerCertification
    {
        static OwnerCertification()
        {
            Executing = new OwnerCertification { Name = "认证中", Value = "Executing" };
            Success = new OwnerCertification { Name = "认证成功", Value = "Success" };
            Failure = new OwnerCertification { Name = "认证失败", Value = "Failure" };
        }

        public string Name { get; set; }

        public string Value { get; set; }

        /// <summary>
        /// 认证中
        /// </summary>
        public static OwnerCertification Executing { get; set; }

        /// <summary>
        /// 认证成功
        /// </summary>
        public static OwnerCertification Success { get; set; }

        /// <summary>
        /// 认证失败
        /// </summary>
        public static OwnerCertification Failure { get; set; }


        public static IEnumerable<OwnerCertification> GetAll() => new List<OwnerCertification>() { Executing, Success, Failure };

    }
}
