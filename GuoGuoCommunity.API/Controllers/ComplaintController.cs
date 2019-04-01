using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 投诉管理
    /// </summary>
    public class ComplaintController : ApiController
    {
        private readonly IComplaintRepository _complaintRepository;
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="complaintRepository"></param>
        /// <param name="tokenManager"></param>
        public ComplaintController(IComplaintRepository complaintRepository, TokenManager tokenManager)
        {
            _complaintRepository = complaintRepository;
            _tokenManager = tokenManager;
        }

        /* 新增投诉需要提供接口
         * 1.提供用户所有小区集合
         * 2.提供用户小区下业户集合
         * 3.提供业主用上级投诉部门集合
         * 4.提供业主委员会用上级投诉部门集合
         */

        /* 1.业主新增投诉
         * 2.业主跟进投诉接口
         * 3.业主向上级街道办申诉接口
         * 4.业主关闭投诉接口
         * 
         */
    }
}
