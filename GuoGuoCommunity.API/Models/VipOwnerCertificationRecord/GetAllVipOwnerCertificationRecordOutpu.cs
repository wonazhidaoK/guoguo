﻿using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllVipOwnerCertificationRecordOutpu
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GetVipOwnerCertificationRecordOutpu> List { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }
    }
}