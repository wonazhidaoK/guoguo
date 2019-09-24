using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 投诉管理
    /// </summary>
    public class ComplaintController : BaseController
    {
        private readonly IComplaintRepository _complaintRepository;
        private readonly IComplaintAnnexRepository _complaintAnnexRepository;
        private readonly IComplaintFollowUpRepository _complaintFollowUpRepository;
        private readonly IComplaintStatusChangeRecordingRepository _complaintStatusChangeRecordingRepository;
        private readonly IOwnerCertificationRecordRepository _ownerCertificationRecordRepository;
        private readonly ITokenRepository _tokenRepository;

        /// <summary>
        /// 
        /// </summary>
        public ComplaintController(IComplaintRepository complaintRepository,
            IComplaintAnnexRepository complaintAnnexRepository,
            IComplaintFollowUpRepository complaintFollowUpRepository,
            IComplaintStatusChangeRecordingRepository complaintStatusChangeRecordingRepository,
            IOwnerCertificationRecordRepository ownerCertificationRecordRepository,
            ITokenRepository tokenRepository)
        {
            _complaintRepository = complaintRepository;
            _complaintAnnexRepository = complaintAnnexRepository;
            _complaintFollowUpRepository = complaintFollowUpRepository;
            _complaintStatusChangeRecordingRepository = complaintStatusChangeRecordingRepository;
            _ownerCertificationRecordRepository = ownerCertificationRecordRepository;
            _tokenRepository = tokenRepository;
        }

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
            if (Authorization == null)
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

            var user = _tokenRepository.GetUser(Authorization);
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
                OperationDepartmentValue = Department.YeZhu.Value
            }, cancelToken);

            var complaintFollowUpEntity = await _complaintFollowUpRepository.AddAsync(new ComplaintFollowUpDto
            {
                ComplaintId = entity.Id.ToString(),
                OperationDepartmentName = Department.YeZhu.Name,
                OperationDepartmentValue = Department.YeZhu.Value,
                Description = "已通过小程序发起了投诉，请尽快处理",
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString(),
                OwnerCertificationId = input.OwnerCertificationId
            }, cancelToken);

            if (!string.IsNullOrWhiteSpace(input.AnnexId))
            {
                await _complaintAnnexRepository.AddAsync(new ComplaintAnnexDto
                {
                    AnnexContent = input.AnnexId,
                    ComplaintId = entity.Id.ToString(),
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString(),
                    ComplaintFollowUpId = complaintFollowUpEntity.Id.ToString()
                }, cancelToken);
            }

            await _complaintStatusChangeRecordingRepository.AddAsync(new ComplaintStatusChangeRecordingDto
            {
                ComplaintFollowUpId = complaintFollowUpEntity.Id.ToString(),
                ComplaintId = entity.Id.ToString(),
                NewStatus = ComplaintStatus.NotAccepted.Value,
                OperationUserId = user.Id.ToString(),
                OperationTime = DateTimeOffset.Now,
            }, cancelToken);
            return new ApiResult<AddComplaintOutput>(APIResultCode.Success, new AddComplaintOutput { Id = entity.Id.ToString(), CreateTime = entity.CreateOperationTime.Value, StatusValue = ComplaintStatus.NotAccepted.Value });
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
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.ComplaintId))
            {
                throw new NotImplementedException("投诉Id信息为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }
            var complaintEntity = await _complaintRepository.GetAsync(input.ComplaintId, cancelToken);
            if (complaintEntity.StatusValue == ComplaintStatus.Completed.Value)
            {
                return new ApiResult(APIResultCode.Success_NoB, "投诉状态已完成并关闭，不能继续进行操作");
            }
            await _complaintRepository.ClosedAsync(new ComplaintDto
            {
                Id = input.ComplaintId,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString(),
                OwnerCertificationId = complaintEntity.OwnerCertificationRecordId.ToString()
            }, cancelToken);

            var complaintFollowUpEntity = await _complaintFollowUpRepository.AddAsync(new ComplaintFollowUpDto
            {
                ComplaintId = input.ComplaintId,
                OperationDepartmentName = Department.YeZhu.Name,
                OperationDepartmentValue = Department.YeZhu.Value,
                Description = "业主通过小程序关闭投诉！",
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString(),
                OwnerCertificationId = complaintEntity.OwnerCertificationRecordId.ToString()
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
            if (Authorization == null)
            {
                return new ApiResult<GetAllComplaintOutput>(APIResultCode.Unknown, new GetAllComplaintOutput { }, APIResultMessage.TokenNull);
            }
            if (input.PageIndex < 1)
            {
                input.PageIndex = 1;
            }
            if (input.PageSize < 1)
            {
                input.PageSize = 10;
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllComplaintOutput>(APIResultCode.Unknown, new GetAllComplaintOutput { }, APIResultMessage.TokenError);
            }
            int startRow = (input.PageIndex - 1) * input.PageSize;
            var data = await _complaintRepository.GetAllAsync(new ComplaintDto
            {
                OwnerCertificationId = input.OwnerCertificationId
            }, cancelToken);

            var listCount = data.Count();
            var listData = data.OrderByDescending(a => a.CreateOperationTime).Skip(startRow).Take(input.PageSize);

            var complaintAnnexList = await _complaintAnnexRepository.GetByComplaintIdsAsync(listData.Select(x => x.Id.ToString()).ToList());
            List<GetComplaintOutput> list = new List<GetComplaintOutput>();
            foreach (var item in listData)
            {
                list.Add(new GetComplaintOutput
                {
                    Id = item.Id.ToString(),
                    CreateTime = item.CreateOperationTime.Value,
                    Description = item.Description,
                    StatusName = item.StatusName,
                    StatusValue = item.StatusValue,
                    Url = (complaintAnnexList?.Where(x => x.ComplaintFollowUp.ComplaintId == item.Id)).FirstOrDefault()?.AnnexContent,
                    OwnerCertificationId = item.OwnerCertificationRecordId.ToString()
                });
            }
            return new ApiResult<GetAllComplaintOutput>(APIResultCode.Success, new GetAllComplaintOutput
            {
                List = list,
                TotalCount = listCount,
                CompletedCount = data.Where(x => x.StatusValue == ComplaintStatus.Completed.Value).ToList().Count(),
                FinishedCount = data.Where(x => x.StatusValue == ComplaintStatus.Finished.Value).ToList().Count(),
                NotAcceptedCount = data.Where(x => x.StatusValue == ComplaintStatus.NotAccepted.Value).ToList().Count(),
                ProcessingCount = data.Where(x => x.StatusValue == ComplaintStatus.Processing.Value).ToList().Count()
            });
        }

        /// <summary>
        /// 业主删除投诉
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("complaint/delete")]
        public async Task<ApiResult> Delete([FromBody]ClosedComplaintFollowUpInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.ComplaintId))
            {
                throw new NotImplementedException("投诉Id信息为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }
            var complaintEntity = await _complaintRepository.GetAsync(input.ComplaintId, cancelToken);
            if (complaintEntity.StatusValue == ComplaintStatus.Completed.Value)
            {
                return new ApiResult(APIResultCode.Success_NoB, "投诉状态已完成并关闭，不能继续进行操作");
            }
            if (complaintEntity.StatusValue != ComplaintStatus.NotAccepted.Value)
            {
                return new ApiResult(APIResultCode.Success_NoB, "投诉已被授理,不能删除！可选择关闭投诉");
            }

            await _complaintRepository.DeleteAsync(new ComplaintDto
            {
                Id = input.ComplaintId,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            var complaintFollowUpEntity = await _complaintFollowUpRepository.AddAsync(new ComplaintFollowUpDto
            {
                ComplaintId = input.ComplaintId,
                OperationDepartmentName = Department.YeZhu.Name,
                OperationDepartmentValue = Department.YeZhu.Value,
                Description = "业主删除投诉！",
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString(),
                OwnerCertificationId = complaintEntity.OwnerCertificationRecordId.ToString()
            }, cancelToken);

            await _complaintStatusChangeRecordingRepository.AddAsync(new ComplaintStatusChangeRecordingDto
            {
                ComplaintFollowUpId = complaintFollowUpEntity.Id.ToString(),
                ComplaintId = input.ComplaintId,
                OldStatus = complaintEntity.StatusValue,
                OperationUserId = user.Id.ToString(),
                OperationTime = DateTimeOffset.Now,
            }, cancelToken);

            return new ApiResult(APIResultCode.Success);
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
            if (Authorization == null)
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
            var user = _tokenRepository.GetUser(Authorization);
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
                OperationDepartmentValue = Department.YeZhuWeiYuanHui.Value
            }, cancelToken);


            var complaintFollowUpEntity = await _complaintFollowUpRepository.AddAsync(new ComplaintFollowUpDto
            {
                ComplaintId = entity.Id.ToString(),
                OperationDepartmentName = Department.YeZhuWeiYuanHui.Name,
                OperationDepartmentValue = Department.YeZhuWeiYuanHui.Value,
                Description = "已通过小程序发起了投诉，请尽快处理",
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString(),
                OwnerCertificationId = input.OwnerCertificationId
            }, cancelToken);

            if (!string.IsNullOrWhiteSpace(input.AnnexId))
            {
                await _complaintAnnexRepository.AddAsync(new ComplaintAnnexDto
                {
                    AnnexContent = input.AnnexId,
                    ComplaintId = entity.Id.ToString(),
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString(),
                    ComplaintFollowUpId = complaintFollowUpEntity.Id.ToString()
                }, cancelToken);
            }
            await _complaintStatusChangeRecordingRepository.AddAsync(new ComplaintStatusChangeRecordingDto
            {
                ComplaintFollowUpId = complaintFollowUpEntity.Id.ToString(),
                ComplaintId = entity.Id.ToString(),
                NewStatus = ComplaintStatus.NotAccepted.Value,
                OperationUserId = user.Id.ToString(),
                OperationTime = DateTimeOffset.Now,
            }, cancelToken);

            return new ApiResult<AddComplaintOutput>(APIResultCode.Success, new AddComplaintOutput { Id = entity.Id.ToString(), CreateTime = entity.CreateOperationTime.Value, StatusValue = ComplaintStatus.NotAccepted.Value });
        }

        /// <summary>
        /// 业委会查看投诉
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("complaint/viewForVipOwner")]
        public async Task<ApiResult> ViewForVipOwner([FromBody]ViewComplaintForVipOwnerInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.ComplaintId))
            {
                throw new NotImplementedException("投诉Id信息为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }
            var complaintEntity = await _complaintRepository.GetAsync(input.ComplaintId, cancelToken);

            if (complaintEntity.StatusValue != ComplaintStatus.NotAccepted.Value)
            {
                return new ApiResult(APIResultCode.Success);
            }

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
                OperationDepartmentValue = Department.YeZhuWeiYuanHui.Value,
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

        /// <summary>
        /// 业委会查询投诉列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("complaint/getAllForVipOwner")]
        public async Task<ApiResult<GetAllComplaintForVipOwnerOutput>> GetAllForVipOwner([FromUri]GetAllComplaintInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<GetAllComplaintForVipOwnerOutput>(APIResultCode.Unknown, new GetAllComplaintForVipOwnerOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllComplaintForVipOwnerOutput>(APIResultCode.Unknown, new GetAllComplaintForVipOwnerOutput { }, APIResultMessage.TokenError);
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
                OwnerCertificationId = input.OwnerCertificationId
            }, cancelToken);

            var listCount = data.Count();
            var listData = data.OrderByDescending(a => a.CreateOperationTime).Skip(startRow).Take(input.PageSize);
            var complaintAnnexList = await _complaintAnnexRepository.GetByComplaintIdsAsync(listData.Select(x => x.Id.ToString()).ToList());
            List<GetComplaintOutput> list = new List<GetComplaintOutput>();
            foreach (var item in listData)
            {
                list.Add(new GetComplaintOutput
                {
                    Id = item.Id.ToString(),
                    CreateTime = item.CreateOperationTime.Value,
                    Description = item.Description,
                    StatusName = item.StatusName,
                    StatusValue = item.StatusValue,
                    Url = (complaintAnnexList?.Where(x => x.ComplaintFollowUp.ComplaintId == item.Id)).FirstOrDefault()?.AnnexContent
                });
            }
            return new ApiResult<GetAllComplaintForVipOwnerOutput>(APIResultCode.Success, new GetAllComplaintForVipOwnerOutput
            {
                List = list,
                TotalCount = listCount,
                CompletedCount = data.Count(x => x.StatusValue == ComplaintStatus.Completed.Value),
                FinishedCount = data.Count(x => x.StatusValue == ComplaintStatus.Finished.Value),
                NotAcceptedCount = data.Count(x => x.StatusValue == ComplaintStatus.NotAccepted.Value),
                ProcessingCount = data.Count(x => x.StatusValue == ComplaintStatus.Processing.Value)
            });
        }

        /// <summary>
        /// 业委会将投票置为无效
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("complaint/invalidVipOwner")]
        public async Task<ApiResult> InvalidVipOwner([FromBody]ViewComplaintForVipOwnerInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.ComplaintId))
            {
                throw new NotImplementedException("投诉Id信息为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }
            var complaintEntity = await _complaintRepository.GetAsync(input.ComplaintId, cancelToken);

            await _complaintRepository.InvalidAsync(new ComplaintDto
            {
                Id = input.ComplaintId,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            var complaintFollowUpEntity = await _complaintFollowUpRepository.AddAsync(new ComplaintFollowUpDto
            {
                ComplaintId = input.ComplaintId,
                OperationDepartmentName = Department.YeZhuWeiYuanHui.Name,
                OperationDepartmentValue = Department.YeZhuWeiYuanHui.Value,
                Description = "业主委成员将投诉置为无效，投诉关闭！",
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

        /// <summary>
        /// 业委会删除投诉
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("complaint/deleteForVipOwner")]
        public async Task<ApiResult> DeleteForVipOwner([FromBody]ClosedComplaintFollowUpInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.ComplaintId))
            {
                throw new NotImplementedException("投诉Id信息为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);

            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }

            var complaintEntity = await _complaintRepository.GetAsync(input.ComplaintId, cancelToken);

            if (complaintEntity.StatusValue == ComplaintStatus.Completed.Value)
            {
                return new ApiResult(APIResultCode.Success_NoB, "投诉状态已完成并关闭，不能继续进行操作");
            }
            if (complaintEntity.StatusValue != ComplaintStatus.NotAccepted.Value)
            {
                return new ApiResult(APIResultCode.Success_NoB, "投诉已被授理,不能删除！可选择关闭投诉");
            }

            await _complaintRepository.DeleteAsync(new ComplaintDto
            {
                Id = input.ComplaintId,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            var complaintFollowUpEntity = await _complaintFollowUpRepository.AddAsync(new ComplaintFollowUpDto
            {
                ComplaintId = input.ComplaintId,
                OperationDepartmentName = Department.YeZhuWeiYuanHui.Name,
                OperationDepartmentValue = Department.YeZhuWeiYuanHui.Value,
                Description = "业主委员会删除投诉！",
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString(),
                OwnerCertificationId = complaintEntity.OwnerCertificationRecordId.ToString()
            }, cancelToken);

            await _complaintStatusChangeRecordingRepository.AddAsync(new ComplaintStatusChangeRecordingDto
            {
                ComplaintFollowUpId = complaintFollowUpEntity.Id.ToString(),
                ComplaintId = input.ComplaintId,
                OldStatus = complaintEntity.StatusValue,
                OperationUserId = user.Id.ToString(),
                OperationTime = DateTimeOffset.Now,
            }, cancelToken);

            return new ApiResult(APIResultCode.Success);
        }

        /// <summary>
        /// 关闭投诉
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("complaint/closedForVipOwner")]
        public async Task<ApiResult> ClosedForVipOwner([FromBody]ClosedComplaintFollowUpInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.ComplaintId))
            {
                throw new NotImplementedException("投诉Id信息为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }
            var complaintEntity = await _complaintRepository.GetAsync(input.ComplaintId, cancelToken);
            if (complaintEntity.StatusValue == ComplaintStatus.Completed.Value)
            {
                return new ApiResult(APIResultCode.Success_NoB, "投诉状态已完成并关闭，不能继续进行操作");
            }
            await _complaintRepository.ClosedAsync(new ComplaintDto
            {
                Id = input.ComplaintId,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString(),
                OwnerCertificationId = complaintEntity.OwnerCertificationRecordId.ToString()
            }, cancelToken);

            var complaintFollowUpEntity = await _complaintFollowUpRepository.AddAsync(new ComplaintFollowUpDto
            {
                ComplaintId = input.ComplaintId,
                OperationDepartmentName = Department.YeZhuWeiYuanHui.Name,
                OperationDepartmentValue = Department.YeZhuWeiYuanHui.Value,
                Description = "业委会通过小程序关闭投诉！",
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString(),
                OwnerCertificationId = complaintEntity.OwnerCertificationRecordId.ToString()
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

        #endregion

        #region 街道办端

        /// <summary>
        /// 街道办查询投诉列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("complaint/getAllForStreetOffice")]
        public async Task<ApiResult<GetAllComplaintForStreetOfficeOutput>> GetAllForStreetOffice([FromUri]GetAllComplaintForStreetOfficeInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<GetAllComplaintForStreetOfficeOutput>(APIResultCode.Unknown, new GetAllComplaintForStreetOfficeOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllComplaintForStreetOfficeOutput>(APIResultCode.Unknown, new GetAllComplaintForStreetOfficeOutput { }, APIResultMessage.TokenError);
            }
            if (input.PageIndex < 1)
            {
                input.PageIndex = 1;
            }
            if (input.PageSize < 1)
            {
                input.PageSize = 10;
            }
            var startTime = DateTimeOffset.Parse("1997-01-01");

            var endTime = DateTimeOffset.Parse("2997-01-01");

            if (DateTimeOffset.TryParse(input.StartTime, out DateTimeOffset startTimeSet))
            {
                startTime = startTimeSet;
            }
            if (DateTimeOffset.TryParse(input.EndTime, out DateTimeOffset endTimeSet))
            {
                endTime = endTimeSet.AddDays(1).AddMinutes(-1);
            }

            int startRow = (input.PageIndex - 1) * input.PageSize;

            var data = await _complaintRepository.GetAllForStreetOfficeIncludeAsync(new ComplaintDto
            {
                StatusValue = input.StatusValue,
                StreetOfficeId = user.StreetOfficeId,
                EndTime = endTime,
                StartTime = startTime,
                Description = input.Description,
            }, cancelToken);
            if (!string.IsNullOrWhiteSpace(input?.DepartmentValue))
            {
                data = data.Where(x => x.OperationDepartmentValue == input.DepartmentValue).ToList();
            }
            if (!string.IsNullOrWhiteSpace(input?.ComplaintTypeId))
            {
                data = data.Where(x => x.ComplaintTypeId.ToString() == input.ComplaintTypeId).ToList();
            }
            var listCount = data.Count();
            var list = data.OrderByDescending(a => a.CreateOperationTime).Skip(startRow).Take(input.PageSize);

            List<GetComplaintOutput> listOutput = new List<GetComplaintOutput>();
            foreach (var item in list)
            {
                string OperationName = "";
                if (item.OperationDepartmentValue == Department.YeZhu.Value)
                {
                    OperationName = (await _ownerCertificationRecordRepository.GetIncludeAsync(item.OwnerCertificationRecordId.ToString(), cancelToken))?.Owner.Name;
                }
                else if (item.OperationDepartmentValue == Department.YeZhuWeiYuanHui.Value)
                {
                    OperationName = (await _ownerCertificationRecordRepository.GetIncludeAsync(item.OwnerCertificationRecordId.ToString(), cancelToken))?.Owner.Name;
                }
                var complaintFollowUp = (await _complaintFollowUpRepository.GetListForComplaintIdAsync(item.Id.ToString(), cancelToken)).Where(x => x.Aappeal != null).FirstOrDefault();
                var complaintFollowUpLists = (await _complaintFollowUpRepository.GetAllAsync(new ComplaintFollowUpDto { ComplaintId = item.Id.ToString() }, cancelToken)).Where(x => x.OperationDepartmentValue == Department.JieDaoBan.Value).ToList();
                var Aappeal = "";
                if (complaintFollowUpLists.Count > 1)
                {
                    Aappeal = "";
                }
                else
                {
                    Aappeal = complaintFollowUp?.Aappeal;
                }
                if (item.StatusValue == ComplaintStatus.Completed.Value)
                {
                    Aappeal = "";
                }
                listOutput.Add(new GetComplaintOutput
                {
                    Id = item.Id.ToString(),
                    CreateTime = item.CreateOperationTime.Value,
                    Description = item.Description,
                    StatusName = item.StatusName,
                    StatusValue = item.StatusValue,
                    Url = (await _complaintAnnexRepository.GetByComplaintIdIncludeAsync(item.Id.ToString()))?.AnnexContent,
                    OperationName = OperationName,
                    OperationDepartmentName = item.OperationDepartmentName,
                    ComplaintTypeName = item.ComplaintType.Name,//complaintFollowUp
                    Aappeal = Aappeal
                });
            }
            return new ApiResult<GetAllComplaintForStreetOfficeOutput>(APIResultCode.Success, new GetAllComplaintForStreetOfficeOutput
            {
                List = listOutput,
                TotalCount = listCount
            });
        }

        /// <summary>
        /// 街道办查看投诉
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("complaint/viewForStreetOffice")]
        public async Task<ApiResult> ViewForStreetOffice([FromBody]ViewForPropertyInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.ComplaintId))
            {
                throw new NotImplementedException("投诉Id信息为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }
            var complaintEntity = await _complaintRepository.GetAsync(input.ComplaintId, cancelToken);
            var complaintFollowUpList = (await _complaintFollowUpRepository.GetAllAsync(new ComplaintFollowUpDto { ComplaintId = input.ComplaintId }, cancelToken));
            if (complaintFollowUpList.Where(x => x.Description == "街道办已查看正在处理！").Any())
            {
                return new ApiResult(APIResultCode.Success);
            }

            //var complaintFollowUpLists = (await _complaintFollowUpRepository.GetAllAsync(new ComplaintFollowUpDto { ComplaintId = input.ComplaintId }, cancelToken)).Where(x => x.Aappeal == "true");//.Where(x => x.Description == "由于问题一直没有得到解决，我已向街道办投诉！").ToList();
            var complaintFollowUpEntity = await _complaintFollowUpRepository.AddAsync(new ComplaintFollowUpDto
            {
                ComplaintId = input.ComplaintId,
                OperationDepartmentName = Department.JieDaoBan.Name,
                OperationDepartmentValue = Department.JieDaoBan.Value,
                Description = "街道办已查看正在处理！",
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            if (!complaintFollowUpList.Where(x => x.Aappeal == "true").Any())
            {
                await _complaintRepository.ViewForStreetOfficeAsync(new ComplaintDto
                {
                    Id = input.ComplaintId,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);

                await _complaintStatusChangeRecordingRepository.AddAsync(new ComplaintStatusChangeRecordingDto
                {
                    ComplaintFollowUpId = complaintFollowUpEntity.Id.ToString(),
                    ComplaintId = input.ComplaintId,
                    OldStatus = complaintEntity.StatusValue,
                    NewStatus = ComplaintStatus.StreetOfficeProcessing.Value,
                    OperationUserId = user.Id.ToString(),
                    OperationTime = DateTimeOffset.Now,
                }, cancelToken);
            }
            return new ApiResult(APIResultCode.Success);

        }

        /// <summary>
        /// 街道办将投票置为无效
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("complaint/invalidStreetOffice")]
        public async Task<ApiResult> InvalidStreetOffice([FromBody]InvalidPropertyInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.ComplaintId))
            {
                throw new NotImplementedException("投诉Id信息为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }
            var complaintEntity = await _complaintRepository.GetAsync(input.ComplaintId, cancelToken);

            await _complaintRepository.InvalidAsync(new ComplaintDto
            {
                Id = input.ComplaintId,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            var complaintFollowUpEntity = await _complaintFollowUpRepository.AddAsync(new ComplaintFollowUpDto
            {
                ComplaintId = input.ComplaintId,
                OperationDepartmentName = Department.JieDaoBan.Name,
                OperationDepartmentValue = Department.JieDaoBan.Value,
                Description = "街道办管理员将投诉置为无效，投诉关闭！",
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
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

        #endregion

        #region 物业端

        /// <summary>
        /// 物业查询投诉列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("complaint/getAllForProperty")]
        public async Task<ApiResult<GetAllComplaintForPropertyOutput>> GetAllForProperty([FromUri]GetAllComplaintForPropertyInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<GetAllComplaintForPropertyOutput>(APIResultCode.Unknown, new GetAllComplaintForPropertyOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllComplaintForPropertyOutput>(APIResultCode.Unknown, new GetAllComplaintForPropertyOutput { }, APIResultMessage.TokenError);
            }
            var startTime = DateTimeOffset.Parse("1997-01-01");

            var endTime = DateTimeOffset.Parse("2997-01-01");

            if (DateTimeOffset.TryParse(input.StartTime, out DateTimeOffset startTimeSet))
            {
                startTime = startTimeSet;
            }
            if (DateTimeOffset.TryParse(input.EndTime, out DateTimeOffset endTimeSet))
            {
                endTime = endTimeSet.AddDays(1).AddMinutes(-1);
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
            var data = await _complaintRepository.GetAllForPropertyIncludeAsync(new ComplaintDto
            {
                StatusValue = input.StatusValue,
                SmallDistrictId = user.SmallDistrictId,
                EndTime = endTime,
                StartTime = startTime,
                Description = input.Description
            }, cancelToken);
            if (!string.IsNullOrWhiteSpace(input?.DepartmentValue))
            {
                data = data.Where(x => x.OperationDepartmentValue == input.DepartmentValue).ToList();
            }
            if (!string.IsNullOrWhiteSpace(input?.ComplaintTypeId))
            {
                data = data.Where(x => x.ComplaintTypeId.ToString() == input.ComplaintTypeId).ToList();
            }
            var listCount = data.Count();
            var list = data.OrderByDescending(a => a.CreateOperationTime).Skip(startRow).Take(input.PageSize);

            var complaintAnnexList = await _complaintAnnexRepository.GetByComplaintIdsAsync(list.Select(x => x.Id.ToString()).ToList());
            List<GetComplaintOutput> listOutput = new List<GetComplaintOutput>();
            foreach (var item in list)
            {
                string OperationName = "";
                if (item.OperationDepartmentValue == Department.YeZhu.Value)
                {
                    OperationName = (await _ownerCertificationRecordRepository.GetIncludeAsync(item.OwnerCertificationRecordId.ToString(), cancelToken))?.Owner.Name;
                }
                else if (item.OperationDepartmentValue == Department.YeZhuWeiYuanHui.Value)
                {
                    OperationName = (await _ownerCertificationRecordRepository.GetIncludeAsync(item.OwnerCertificationRecordId.ToString(), cancelToken))?.Owner.Name;
                }
                listOutput.Add(new GetComplaintOutput
                {
                    Id = item.Id.ToString(),
                    CreateTime = item.CreateOperationTime.Value,
                    Description = item.Description,
                    StatusName = item.StatusName,
                    StatusValue = item.StatusValue,
                    Url = (complaintAnnexList?.Where(x => x.ComplaintFollowUp.ComplaintId == item.Id)).FirstOrDefault()?.AnnexContent,
                    OperationName = OperationName,
                    OperationDepartmentName = item.OperationDepartmentName,
                    ComplaintTypeName = item.ComplaintType.Name
                });
            }

            return new ApiResult<GetAllComplaintForPropertyOutput>(APIResultCode.Success, new GetAllComplaintForPropertyOutput
            {
                List = listOutput,
                TotalCount = listCount
            });

        }

        /// <summary>
        /// 物业查看投诉
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("complaint/viewForProperty")]
        public async Task<ApiResult> ViewForProperty([FromBody]ViewForPropertyInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.ComplaintId))
            {
                throw new NotImplementedException("投诉Id信息为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }

            var complaintEntity = await _complaintRepository.GetAsync(input.ComplaintId, cancelToken);

            if (complaintEntity.StatusValue != ComplaintStatus.NotAccepted.Value)
            {
                return new ApiResult(APIResultCode.Success);
            }

            await _complaintRepository.ViewForPropertyAsync(new ComplaintDto
            {
                Id = input.ComplaintId,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            var complaintFollowUpEntity = await _complaintFollowUpRepository.AddAsync(new ComplaintFollowUpDto
            {
                ComplaintId = input.ComplaintId,
                OperationDepartmentName = Department.WuYe.Name,
                OperationDepartmentValue = Department.WuYe.Value,
                Description = "物业已查看正在处理！",
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
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

        /// <summary>
        /// 物业将投票置为无效
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("complaint/invalidProperty")]
        public async Task<ApiResult> InvalidProperty([FromBody]InvalidPropertyInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.ComplaintId))
            {
                throw new NotImplementedException("投诉Id信息为空！");
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }
            var complaintEntity = await _complaintRepository.GetAsync(input.ComplaintId, cancelToken);

            await _complaintRepository.InvalidAsync(new ComplaintDto
            {
                Id = input.ComplaintId,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            var complaintFollowUpEntity = await _complaintFollowUpRepository.AddAsync(new ComplaintFollowUpDto
            {
                ComplaintId = input.ComplaintId,
                OperationDepartmentName = Department.WuYe.Name,
                OperationDepartmentValue = Department.WuYe.Value,
                Description = "物业管理员将投诉置为无效，投诉关闭！",
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
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

        #endregion
    }
}
