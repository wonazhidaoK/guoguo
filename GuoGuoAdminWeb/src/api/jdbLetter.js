
import request from '@/utils/request'

// 小区
export function getSmallList(StreetOfficeId) {
  return request({
    url: '/smallDistrict/getAllForStreetOfficeId',
    params: { StreetOfficeId }
  })
}

// 新建站内信POSTPOST /stationLetter/add
export function createvLetter(data) {
  return request({
    url: '/stationLetter/add',
    method: 'post',
    data
  })
}

// 获取 街道办站内信列表GET /stationLetter/getAllStreetOfficeStationLetter
export function fetchListVote(query) {
  return request({
    url: '/stationLetter/getAllStreetOfficeStationLetter',
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

// GET /vote/get
export function detailVote(Id) {
  return request({
    url: '/stationLetter/get',
    params: { Id }
  })
}
