using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllForPageGoodsTypeOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GetAllForPageGoodsTypeOutputModel> List { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class GetAllForPageGoodsTypeOutputModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Sort { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public int PageIndex { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public int PageSize { get; set; }
    }
}