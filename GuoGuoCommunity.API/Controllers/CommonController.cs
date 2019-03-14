﻿using GuoGuoCommunity.Domain.Models.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 公共接口
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CommonController : ApiController
    {
        /// <summary>
        /// 获取部门集合
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("department/getAll")]
        public ApiResult<List<Department>> GetAll() => new ApiResult<List<Department>>(APIResultCode.Success, Department.GetAll().ToList());
    }
}
