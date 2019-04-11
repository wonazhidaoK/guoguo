using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models.Enum;
using System;
using System.Collections.Generic;
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
        private readonly IOwnerCertificationRecordRepository _ownerCertificationRecordRepository;
        private readonly IUserRepository _userRepository;
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="complaintRepository"></param>
        /// <param name="complaintFollowUpRepository"></param>
        /// <param name="complaintAnnexRepository"></param>
        /// <param name="complaintStatusChangeRecordingRepository"></param>
        /// <param name="ownerCertificationRecordRepository"></param>
        /// <param name="userRepository"></param>
        public ComplaintFollowUpController(
            IComplaintRepository complaintRepository,
            IComplaintFollowUpRepository complaintFollowUpRepository,
            IComplaintAnnexRepository complaintAnnexRepository,
            IComplaintStatusChangeRecordingRepository complaintStatusChangeRecordingRepository,
            IOwnerCertificationRecordRepository ownerCertificationRecordRepository,
            IUserRepository userRepository)
        {
            _complaintRepository = complaintRepository;
            _complaintAnnexRepository = complaintAnnexRepository;
            _complaintStatusChangeRecordingRepository = complaintStatusChangeRecordingRepository;
            _complaintFollowUpRepository = complaintFollowUpRepository;
            _ownerCertificationRecordRepository = ownerCertificationRecordRepository;
            _userRepository = userRepository;
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
                //查询跟进记录达到两条不允许在进行跟进
                var complaintFollowUpList = await _complaintFollowUpRepository.GetListAsync(new ComplaintFollowUpDto
                {
                    ComplaintId = input.ComplaintId,
                    OwnerCertificationId = input.OwnerCertificationId
                }, cancelToken);

                if (complaintFollowUpList.Count > 1)
                {
                    throw new NotImplementedException("投诉跟进达到上限！");
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

        /// <summary>
        /// 获取投诉跟进详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("complaintFollowUp/get")]
        public async Task<ApiResult<List<GetComplaintFollowUpOutput>>> Get([FromUri]string id, CancellationToken cancelToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new NotImplementedException("投诉ID为空！");
                }
                var data = await _complaintFollowUpRepository.GetListForComplaintIdAsync(id, cancelToken);
                List<GetComplaintFollowUpOutput> list = new List<GetComplaintFollowUpOutput>();
                foreach (var item in data)
                {
                    string OperationName = "";
                    if (item.OperationDepartmentValue == Department.YeZhu.Value)
                    {
                        OperationName = (await _ownerCertificationRecordRepository.GetAsync(item.OwnerCertificationId, cancelToken))?.OwnerName;
                    }
                    else
                    {
                        OperationName = (await _userRepository.GetForIdAsync(item.CreateOperationUserId, cancelToken)).Name;
                    }
                    var entity = new GetComplaintFollowUpOutput
                    {
                        Aappeal = item.Aappeal,
                        Description = item.Description,
                        OperationDepartmentName = item.OperationDepartmentName,
                        OperationName = OperationName,
                        Url = _complaintAnnexRepository.GetUrlForFollowUpId(item.Id.ToString())
                    };
                    list.Add(entity);
                }
                return new ApiResult<List<GetComplaintFollowUpOutput>>(APIResultCode.Success, list);
            }
            catch (Exception e)
            {
                return new ApiResult<List<GetComplaintFollowUpOutput>>(APIResultCode.Success_NoB, new List<GetComplaintFollowUpOutput> { }, e.Message);
            }
        }

        #endregion

        #region 业委会端

        /*
         * 
         * 2.处理业主投诉
         * 
         * 1.跟进业委会发起的投诉
         * 4.申诉
         */

        /// <summary>
        /// 处理业主投诉投诉信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("complaintFollowUp/addForVipOwner")]
        public async Task<ApiResult<AddComplaintFollowUpOutput>> AddForVipOwner([FromBody]AddComplaintFollowUpInput input, CancellationToken cancelToken)
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
                    OperationDepartmentName = Department.YeZhuWeiYuanHui.Name,
                    OperationDepartmentValue = Department.YeZhuWeiYuanHui.Value,
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
                var complaintEntity = await _complaintRepository.GetAsync(input.ComplaintId, cancelToken);

                await _complaintStatusChangeRecordingRepository.AddAsync(new ComplaintStatusChangeRecordingDto
                {
                    ComplaintFollowUpId = entity.Id.ToString(),
                    ComplaintId = input.ComplaintId,
                    OldStatus = complaintEntity.StatusValue,
                    NewStatus = ComplaintStatus.Finished.Value,
                    OperationUserId = user.Id.ToString(),
                    OperationTime = DateTimeOffset.Now,
                }, cancelToken);

                await _complaintRepository.UpdateForFinishedAsync(new ComplaintDto
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

        #region 物业端

        /*
         * 处理投诉
         */

        /// <summary>
        /// 处理业主投诉信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("complaintFollowUp/addForProperty")]
        public async Task<ApiResult<AddComplaintFollowUpOutput>> AddForProperty([FromBody]AddComplaintFollowUpForPropertyInput input, CancellationToken cancelToken)
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
                    ComplaintId = input.ComplaintId,
                    OperationDepartmentName = Department.WuYe.Name,
                    OperationDepartmentValue = Department.WuYe.Value,
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
                var complaintEntity = await _complaintRepository.GetAsync(input.ComplaintId, cancelToken);

                await _complaintStatusChangeRecordingRepository.AddAsync(new ComplaintStatusChangeRecordingDto
                {
                    ComplaintFollowUpId = entity.Id.ToString(),
                    ComplaintId = input.ComplaintId,
                    OldStatus = complaintEntity.StatusValue,
                    NewStatus = ComplaintStatus.Finished.Value,
                    OperationUserId = user.Id.ToString(),
                    OperationTime = DateTimeOffset.Now,
                }, cancelToken);

                await _complaintRepository.UpdateForFinishedAsync(new ComplaintDto
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

        #region 街道办端

        /*
         * 处理投诉
         */
        
        /// <summary>
        /// 处理业主投诉信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("complaintFollowUp/addForStreetOffice")]
        public async Task<ApiResult<AddComplaintFollowUpOutput>> AddForStreetOffice([FromBody]AddComplaintFollowUpForPropertyInput input, CancellationToken cancelToken)
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
                    ComplaintId = input.ComplaintId,
                    OperationDepartmentName = Department.JieDaoBan.Name,
                    OperationDepartmentValue = Department.JieDaoBan.Value,
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
                var complaintEntity = await _complaintRepository.GetAsync(input.ComplaintId, cancelToken);

                await _complaintStatusChangeRecordingRepository.AddAsync(new ComplaintStatusChangeRecordingDto
                {
                    ComplaintFollowUpId = entity.Id.ToString(),
                    ComplaintId = input.ComplaintId,
                    OldStatus = complaintEntity.StatusValue,
                    NewStatus = ComplaintStatus.Finished.Value,
                    OperationUserId = user.Id.ToString(),
                    OperationTime = DateTimeOffset.Now,
                }, cancelToken);

                await _complaintRepository.UpdateForFinishedAsync(new ComplaintDto
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
    }
}
