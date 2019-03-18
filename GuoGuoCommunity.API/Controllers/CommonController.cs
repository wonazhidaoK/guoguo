using GuoGuoCommunity.Domain.Models.Enum;
using System;
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

        /// <summary>
        /// 获取服务器Id
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        [HttpGet]
        [Route("common/getIp")]
        public ApiResult<string> GetIp()=> new ApiResult<string>(APIResultCode.Success, IpUtility.GetLocalIP());

        /// <summary>
        /// 获取文件类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("fileType/getAll")]
        public ApiResult<List<FileType>> GetFileTypeAll() => new ApiResult<List<FileType>>(APIResultCode.Success, FileType.GetAll().ToList());
    }
}
