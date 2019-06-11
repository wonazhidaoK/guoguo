
import request from '@/utils/request'

// 获取列表GETGET vipOwnerApplicationRecord/getAllForHistory
export function fetchList(query) {
  return request({
    url: '/vipOwnerApplicationRecord/getAllForHistory',
    method: 'get',
    params: query
  })
}

// 小区
export function getxiaoquList(StreetOfficeId) {
  return request({
    url: '/smallDistrict/getAllForStreetOfficeId',
    params: { StreetOfficeId }
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

// 删除GET /announcement/delete
export function delVote(Id) {
  return request({
    url: '//delete',
    params: { Id }
  })
}

// GET /vote/getGET /stationLetter/getPropertyStationLetter
export function detailVote(Id) {
  return request({
    url: '/stationLetter/getPropertyStationLetter',
    params: { Id }
  })
}
