
import request from '@/utils/request'

// 投诉 详情GET /complaintFollowUp/get
export function detailTs(id) {
  return request({
    url: '/complaintFollowUp/get',
    params: { id }
  })
}

// 查看投诉POST
export function chakanTs(ComplaintId) {
  return request({
    url: '/complaint/viewForStreetOffice',
    method: 'post',
    data: { ComplaintId }
  })
}

// 投诉状态列表GETGET /department/getForComplaintAll
export function tsBmFn() {
  return request({
    url: '/department/getForComplaintAll',
    method: 'get'
  })
}

// 投诉类型列表GETGET /complaintType/getList
export function tsTypeFn(initiatingDepartmentValue) {
  return request({
    url: '/complaintType/getList',
    method: 'get',
    params: { initiatingDepartmentValue }
  })
}

// 投诉状态列表GET /complaintStatus/getAll
export function tsStatusFn() {
  return request({
    url: '/complaintStatus/getAll',
    method: 'get'
  })
}

// 新建公告POST /announcement/addPropertyAnnouncement
export function createNotice(data) {
  return request({
    url: '/announcement/addPropertyAnnouncement',
    method: 'post',
    data
  })
}

// 获取投诉列表 GET /complaint/getAllForStreetOffice
export function fetchList(query) {
  return request({
    url: '/complaint/getAllForStreetOffice',
    method: 'get',
    params: query
  })
}

// 处理投诉POST /complaintFollowUp/addForStreetOffice
export function handleTs(data) {
  return request({
    url: '/complaintFollowUp/addForStreetOffice',
    method: 'post',
    data
  })
}

// 置为无效 POST /complaint/invalidStreetOffice
export function delTs(ComplaintId) {
  return request({
    url: '/complaint/invalidStreetOffice',
    method: 'post',
    data: { ComplaintId }
  })
}
