using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models.Enum;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 添加投票跟进记录
    /// </summary>
    public class ComplaintFollowUpController : ApiController
    {
        private readonly IComplaintRepository _complaintRepository;
        private readonly IComplaintFollowUpRepository _complaintFollowUpRepository;
        private readonly IComplaintAnnexRepository _complaintAnnexRepository;
        private readonly IComplaintStatusChangeRecordingRepository _complaintStatusChangeRecordingRepository;
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="complaintRepository"></param>
        /// <param name="complaintFollowUpRepository"></param>
        /// <param name="complaintAnnexRepository"></param>
        /// <param name="complaintStatusChangeRecordingRepository"></param>
        public ComplaintFollowUpController(
            IComplaintRepository complaintRepository,
            IComplaintFollowUpRepository complaintFollowUpRepository,
            IComplaintAnnexRepository complaintAnnexRepository,
            IComplaintStatusChangeRecordingRepository complaintStatusChangeRecordingRepository)
        {
            _complaintRepository = complaintRepository;
            _complaintAnnexRepository = complaintAnnexRepository;
            _complaintStatusChangeRecordingRepository = complaintStatusChangeRecordingRepository;
            _complaintFollowUpRepository = complaintFollowUpRepository;
            _tokenManager = new TokenManager();
        }

        /*
         * 1.业主跟进记录
         * 2.业委会跟进记录
         * 3.物业跟进记录
         * 4.街道办跟进记录
         */

        #region 业主端

        /*
         * 1.跟进投诉
         * 2.申诉投诉
         * 3.关闭投诉
         */

        /// <summary>
        /// 添加投诉跟进信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("complaintFollowUp/add")]
        public async Task<ApiResult<AddComplaintFollowUpOutput>> Add([FromBody]AddComplaintFollowUpInput input, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<AddComplaintFollowUpOutput>(APIResultCode.Unknown, new AddComplaintFollowUpOutput { }, APIResultMessage.TokenNull);
                }
                if (string.IsNullOrWhiteSpace(input.ComplaintId))
                {
                    throw new NotImplementedException("投诉Id信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.Description))
                {
                    throw new NotImplementedException("描述信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.OwnerCertificationId))
                {
                    throw new NotImplementedException("业主认证Id信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.AnnexId))
                {
                    throw new NotImplementedException("投诉附件信息为空！");
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddComplaintFollowUpOutput>(APIResultCode.Unknown, new AddComplaintFollowUpOutput { }, APIResultMessage.TokenError);
                }

                var entity = await _complaintFollowUpRepository.AddAsync(new ComplaintFollowUpDto
                {
                    Description = input.Description,
                    OwnerCertificationId = input.OwnerCertificationId,
                    ComplaintId = input.ComplaintId,
                    OperationDepartmentName = Department.YeZhu.Name,
                    OperationDepartmentValue = Department.YeZhu.Value,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);

                if (!string.IsNullOrWhiteSpace(input.AnnexId))
                {
                    await _complaintAnnexRepository.AddForFollowUpIdAsync(new ComplaintAnnexDto
                    {
                        AnnexContent = input.AnnexId,
                        ComplaintId = entity.Id.ToString(),
                        OperationTime = DateTimeOffset.Now,
                        OperationUserId = user.Id.ToString(),
                        ComplaintFollowUpId = entity.Id.ToString()
                    }, cancelToken);
                }

                return new ApiResult<AddComplaintFollowUpOutput>(APIResultCode.Success, new AddComplaintFollowUpOutput { Id = entity.Id.ToString() });
            }
            catch (Exception e)
            {
                return new ApiResult<AddComplaintFollowUpOutput>(APIResultCode.Success_NoB, new AddComplaintFollowUpOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 申诉投诉
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("complaintFollowUp/appeal")]
        public async Task<ApiResult<AddComplaintFollowUpOutput>> Appeal([FromBody]AppealComplaintFollowUpInput input, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<AddComplaintFollowUpOutput>(APIResultCode.Unknown, new AddComplaintFollowUpOutput { }, APIResultMessage.TokenNull);
                }
                if (string.IsNullOrWhiteSpace(input.ComplaintId))
                {
                    throw new NotImplementedException("投诉Id信息为空！");
                }

                if (string.IsNullOrWhiteSpace(input.OwnerCertificationId))
                {
                    throw new NotImplementedException("业主认证Id信息为空！");
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddComplaintFollowUpOutput>(APIResultCode.Unknown, new AddComplaintFollowUpOutput { }, APIResultMessage.TokenError);
                }

                var entity = await _complaintFollowUpRepository.AddAsync(new ComplaintFollowUpDto
                {
                    Description = "由于问题一直没有得到解决，我已向街道办投诉！",
                    OwnerCertificationId = input.OwnerCertificationId,
                    ComplaintId = input.ComplaintId,
                    OperationDepartmentName = Department.YeZhu.Name,
                    OperationDepartmentValue = Department.YeZhu.Value,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);
                var complaintEntity = await _complaintRepository.GetAsync(input.ComplaintId, cancelToken);

                await _complaintStatusChangeRecordingRepository.AddAsync(new ComplaintStatusChangeRecordingDto
                {
                    ComplaintFollowUpId = entity.Id.ToString(),
                    ComplaintId = input.ComplaintId,
                    OldStatus = complaintEntity.StatusValue,
                    NewStatus = ComplaintStatus.StreetOfficeNotAccepted.Value,
                    OperationUserId = user.Id.ToString(),
                    OperationTime = DateTimeOffset.Now,
                }, cancelToken);

                await _complaintRepository.UpdateForAppealAsync(new ComplaintDto
                {
                    Id = input.ComplaintId,
                    OperationUserId = user.Id.ToString(),
                    OperationTime = DateTimeOffset.Now,
                });
                return new ApiResult<AddComplaintFollowUpOutput>(APIResultCode.Success, new AddComplaintFollowUpOutput { Id = entity.Id.ToString() });
            }
            catch (Exception e)
            {
                return new ApiResult<AddComplaintFollowUpOutput>(APIResultCode.Success_NoB, new AddComplaintFollowUpOutput { }, e.Message);
            }
        }

        #endregion

        #region 业委会端

        /*
         * 1.跟进业委会发起的投诉
         * 2.处理业主投诉
         * 3.浏览业主投诉
         * 4.申诉
         */

        #endregion
    }
}
