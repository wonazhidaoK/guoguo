
import request from '@/utils/request'

// 小区
export function getSmallList(StreetOfficeId) {
  return request({
    url: '/smallDistrict/getAllForStreetOfficeId',
    params: { StreetOfficeId }
  })
}

// getYwhListGET /vipOwner/getList
export function getYwhList(smallDistrictId) {
  return request({
    url: '/vipOwner/getList',
    params: { smallDistrictId }
  })
}

// 新建投票POST /vote/addVoteForStreetOfficePOST /vote/addVoteForVipOwnerElection
export function createvVote(data) {
  // const data = {
  //   'VipOwnerId': p.Title,
  //   'Summary': p.Summary,
  //   'Deadline': p.Deadline,
  //   'SmallDistrictId': p.SmallDistrict,
  //   'AnnexId': p.AnnexId
  // }
  return request({
    url: '/vote/addVoteForVipOwnerElection',
    method: 'post',
    data
  })
}

// 获取高级认证列表GET /vipOwnerCertification/getAll
export function fetchListVote(query) {
  return request({
    url: '/vipOwnerCertification/getAll',
    method: 'get',
    params: query
  })
}

// 编辑toupiao
export function updateVote(data) {
  return request({
    url: '//update',
    method: 'post',
    data
  })
}

// 删除投票GET /announcement/delete
export function delVote(Id) {
  return request({
    url: '//delete',
    params: { Id }
  })
}

// 通过认证POST /vipOwnerCertification/adopt
export function successRenZheng(Id) {
  return request({
    url: '/vipOwnerCertification/adopt',
    method: 'post',
    data: { Id }
  })
}

// 高级认证详情GET /vipOwnerCertification/get
export function detailRz(Id) {
  return request({
    url: '/vipOwnerCertification/get',
    params: { Id }
  })
}
