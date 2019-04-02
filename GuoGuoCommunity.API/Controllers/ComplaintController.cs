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
         * 2.业主跟进投诉接口
         * 3.业主向上级街道办申诉接口
         * 4.业主关闭投诉接口
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


        #endregion

        #region 业委会端


        /// <summary>
        /// 业委会添加投诉信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("complaint/addForVipOwner")]
        public async Task<ApiResult<AddComplaintOutput>> AddForVipOwner([FromBody]AddComplaintInput input, CancellationToken cancelToken)
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

                var department = Department.GetAllForVipOwner().Where(x => x.Value == input.DepartmentValue).FirstOrDefault();
                if (department == null)
                {
                    throw new NotImplementedException("业委会投诉部门信息不准确！");
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
                    OperationDepartmentName = Department.YeZhuWeiYuanHui.Name,
                    OperationDepartmentValue = Department.YeZhuWeiYuanHui.Name
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
                    OperationDepartmentName = Department.YeZhuWeiYuanHui.Name,
                    OperationDepartmentValue = Department.YeZhuWeiYuanHui.Name,
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


        #endregion
    }
}
