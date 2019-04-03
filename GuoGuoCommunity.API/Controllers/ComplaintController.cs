using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models.Enum;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 投诉管理
    /// </summary>
    public class ComplaintController : ApiController
    {
        private readonly IComplaintRepository _complaintRepository;
        private readonly IComplaintAnnexRepository _complaintAnnexRepository;
        private readonly IComplaintFollowUpRepository _complaintFollowUpRepository;
        private readonly IComplaintStatusChangeRecordingRepository _complaintStatusChangeRecordingRepository;
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="complaintRepository"></param>
        /// <param name="complaintAnnexRepository"></param>
        /// <param name="complaintFollowUpRepository"></param>
        /// <param name="complaintStatusChangeRecordingRepository"></param>
        /// <param name="tokenManager"></param>
        public ComplaintController(IComplaintRepository complaintRepository,
            IComplaintAnnexRepository complaintAnnexRepository,
            IComplaintFollowUpRepository complaintFollowUpRepository,
            IComplaintStatusChangeRecordingRepository complaintStatusChangeRecordingRepository,
            TokenManager tokenManager)
        {
            _complaintRepository = complaintRepository;
            _complaintAnnexRepository = complaintAnnexRepository;
            _complaintFollowUpRepository = complaintFollowUpRepository;
            _complaintStatusChangeRecordingRepository = complaintStatusChangeRecordingRepository;
            _tokenManager = tokenManager;
        }

        /* 新增投诉需要提供接口
         * 1.提供用户所有小区集合
         * 2.提供用户小区下业户集合
         * 3.提供业主用上级投诉部门集合
         * 4.提供业主委员会用上级投诉部门集合
         */

        /* 1.业主新增投诉
         * 2.业主关闭投诉接口
         */
        #region 业主端


        /// <summary>
        /// 添加投诉信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("complaint/add")]
        public async Task<ApiResult<AddComplaintOutput>> Add([FromBody]AddComplaintInput input, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<AddComplaintOutput>(APIResultCode.Unknown, new AddComplaintOutput { }, APIResultMessage.TokenNull);
                }
                if (string.IsNullOrWhiteSpace(input.ComplaintTypeId))
                {
                    throw new NotImplementedException("投诉类型Id信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.DepartmentValue))
                {
                    throw new NotImplementedException("部门值信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.OwnerCertificationId))
                {
                    throw new NotImplementedException("业主认证Id信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.Description))
                {
                    throw new NotImplementedException("投诉描述信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.AnnexId))
                {
                    throw new NotImplementedException("投诉附件信息为空！");
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddComplaintOutput>(APIResultCode.Unknown, new AddComplaintOutput { }, APIResultMessage.TokenError);
                }

                var department = Department.GetAllForOwner().Where(x => x.Value == input.DepartmentValue).FirstOrDefault();
                if (department == null)
                {
                    throw new NotImplementedException("业主投诉部门信息不准确！");
                }

                var entity = await _complaintRepository.AddAsync(new ComplaintDto
                {
                    Description = input.Description,
                    OwnerCertificationId = input.OwnerCertificationId,
                    DepartmentValue = department.Value,
                    DepartmentName = department.Name,
                    ComplaintTypeId = input.ComplaintTypeId,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString(),
                    OperationDepartmentName = Department.YeZhu.Name,
                    OperationDepartmentValue = Department.YeZhu.Name
                }, cancelToken);

                if (!string.IsNullOrWhiteSpace(input.AnnexId))
                {
                    await _complaintAnnexRepository.AddAsync(new ComplaintAnnexDto
                    {
                        AnnexContent = input.AnnexId,
                        ComplaintId = entity.Id.ToString(),
                        OperationTime = DateTimeOffset.Now,
                        OperationUserId = user.Id.ToString()
                    }, cancelToken);
                }
                var complaintFollowUpEntity = await _complaintFollowUpRepository.AddAsync(new ComplaintFollowUpDto
                {
                    ComplaintId = entity.Id.ToString(),
                    OperationDepartmentName = Department.YeZhu.Name,
                    OperationDepartmentValue = Department.YeZhu.Name,
                    Description = "已通过小程序发起了投诉，请尽快处理",
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString(),
                    OwnerCertificationId = input.OwnerCertificationId
                }, cancelToken);

                await _complaintStatusChangeRecordingRepository.AddAsync(new ComplaintStatusChangeRecordingDto
                {
                    ComplaintFollowUpId = complaintFollowUpEntity.Id.ToString(),
                    ComplaintId = entity.Id.ToString(),
                    NewStatus = ComplaintStatus.NotAccepted.Value,
                    OperationUserId = user.Id.ToString(),
                    OperationTime = DateTimeOffset.Now,
                }, cancelToken);
                return new ApiResult<AddComplaintOutput>(APIResultCode.Success, new AddComplaintOutput { Id = entity.Id.ToString() });
            }
            catch (Exception e)
            {
                return new ApiResult<AddComplaintOutput>(APIResultCode.Success_NoB, new AddComplaintOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 关闭投诉
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("complaint/closed")]
        public async Task<ApiResult> Closed([FromBody]ClosedComplaintFollowUpInput input, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
                }
                if (string.IsNullOrWhiteSpace(input.ComplaintId))
                {
                    throw new NotImplementedException("投诉Id信息为空！");
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
                }
                var complaintEntity = await _complaintRepository.GetAsync(input.ComplaintId, cancelToken);

                await _complaintRepository.ClosedAsync(new ComplaintDto
                {
                    Id = input.ComplaintId,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString(),
                    OwnerCertificationId = input.OwnerCertificationId
                }, cancelToken);
                var complaintFollowUpEntity = await _complaintFollowUpRepository.AddAsync(new ComplaintFollowUpDto
                {
                    ComplaintId = input.ComplaintId,
                    OperationDepartmentName = Department.YeZhu.Name,
                    OperationDepartmentValue = Department.YeZhu.Name,
                    Description = "业主通过小程序关闭投诉！",
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString(),
                    OwnerCertificationId = input.OwnerCertificationId
                }, cancelToken);

                await _complaintStatusChangeRecordingRepository.AddAsync(new ComplaintStatusChangeRecordingDto
                {
                    ComplaintFollowUpId = complaintFollowUpEntity.Id.ToString(),
                    ComplaintId = input.ComplaintId,
                    OldStatus = complaintEntity.StatusValue,
                    NewStatus = ComplaintStatus.Completed.Value,
                    OperationUserId = user.Id.ToString(),
                    OperationTime = DateTimeOffset.Now,
                }, cancelToken);

                return new ApiResult(APIResultCode.Success);
            }
            catch (Exception e)
            {
                return new ApiResult(APIResultCode.Success_NoB, e.Message);
            }
        }

        /// <summary>
        /// 查询投诉列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("complaint/getAll")]
        public async Task<ApiResult<GetAllComplaintOutput>> GetAll([FromUri]GetAllComplaintInput input, CancellationToken cancelToken)
        {
            try
            {
                if (input.PageIndex < 1)
                {
                    input.PageIndex = 1;
                }
                if (input.PageSize < 1)
                {
                    input.PageSize = 10;
                }
                int startRow = (input.PageIndex - 1) * input.PageSize;
                var data = await _complaintRepository.GetAllAsync(new ComplaintDto
                {
                    OwnerCertificationId = input.OwnerCertificationId
                }, cancelToken);

                return new ApiResult<GetAllComplaintOutput>(APIResultCode.Success, new GetAllComplaintOutput
                {
                    List = data.Select(x => new GetComplaintOutput
                    {
                        Id = x.Id.ToString(),
                        CreateTime = x.CreateOperationTime.Value,
                        Description = x.Description,
                        StatusName = x.StatusName,
                        StatusValue = x.StatusValue,
                        Url = _complaintAnnexRepository.GetUrl(x.Id.ToString())
                    }).Skip(startRow).Take(input.PageSize).ToList(),
                    TotalCount = data.Count()
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetAllComplaintOutput>(APIResultCode.Success_NoB, new GetAllComplaintOutput { }, e.Message);
            }
        }

        #endregion

        #region 业委会端

        /*
         * 1.业委会添加投诉
         * 2.业务会添加投诉关闭投诉
         * 3.浏览业主投诉
         */

        /// <summary>
        /// 业委会查看投诉
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("complaint/viewForVipOwner")]
        public async Task<ApiResult> ViewForVipOwner([FromBody]ClosedComplaintFollowUpInput input, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
                }
                if (string.IsNullOrWhiteSpace(input.ComplaintId))
                {
                    throw new NotImplementedException("投诉Id信息为空！");
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
                }
                var complaintEntity = await _complaintRepository.GetAsync(input.ComplaintId, cancelToken);

                await _complaintRepository.ViewForVipOwnerAsync(new ComplaintDto
                {
                    Id = input.ComplaintId,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);
                var complaintFollowUpEntity = await _complaintFollowUpRepository.AddAsync(new ComplaintFollowUpDto
                {
                    ComplaintId = input.ComplaintId,
                    OperationDepartmentName = Department.YeZhuWeiYuanHui.Name,
                    OperationDepartmentValue = Department.YeZhuWeiYuanHui.Name,
                    Description = "业主委员会已查看正在处理！",
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString(),
                    OwnerCertificationId = input.OwnerCertificationId
                }, cancelToken);

                await _complaintStatusChangeRecordingRepository.AddAsync(new ComplaintStatusChangeRecordingDto
                {
                    ComplaintFollowUpId = complaintFollowUpEntity.Id.ToString(),
                    ComplaintId = input.ComplaintId,
                    OldStatus = complaintEntity.StatusValue,
                    NewStatus = ComplaintStatus.Processing.Value,
                    OperationUserId = user.Id.ToString(),
                    OperationTime = DateTimeOffset.Now,
                }, cancelToken);

                return new ApiResult(APIResultCode.Success);
            }
            catch (Exception e)
            {
                return new ApiResult(APIResultCode.Success_NoB, e.Message);
            }
        }

        /// <summary>
        /// 业委会查询投诉列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("complaint/getAllForVipOwner")]
        public async Task<ApiResult<GetAllComplaintOutput>> GetAllForVipOwner([FromUri]GetAllComplaintInput input, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<GetAllComplaintOutput>(APIResultCode.Unknown, new GetAllComplaintOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<GetAllComplaintOutput>(APIResultCode.Unknown, new GetAllComplaintOutput { }, APIResultMessage.TokenError);
                }

                if (input.PageIndex < 1)
                {
                    input.PageIndex = 1;
                }
                if (input.PageSize < 1)
                {
                    input.PageSize = 10;
                }
                int startRow = (input.PageIndex - 1) * input.PageSize;
                var data = await _complaintRepository.GetAllForVipOwnerAsync(new ComplaintDto
                {
                    SmallDistrictId = user.SmallDistrictId
                }, cancelToken);

                return new ApiResult<GetAllComplaintOutput>(APIResultCode.Success, new GetAllComplaintOutput
                {
                    List = data.Select(x => new GetComplaintOutput
                    {
                        Id = x.Id.ToString(),
                        CreateTime = x.CreateOperationTime.Value,
                        Description = x.Description,
                        StatusName = x.StatusName,
                        StatusValue = x.StatusValue,
                        Url = _complaintAnnexRepository.GetUrl(x.Id.ToString())
                    }).Skip(startRow).Take(input.PageSize).ToList(),
                    TotalCount = data.Count()
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetAllComplaintOutput>(APIResultCode.Success_NoB, new GetAllComplaintOutput { }, e.Message);
            }
        }

        #endregion
    }
}
