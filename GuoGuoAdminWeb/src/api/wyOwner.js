
import request from '@/utils/request'

// 获取列表GETGET GET /vipOwnerCertificationRecord/getAllForProperty
export function fetchList(query) {
  return request({
    url: '/vipOwnerCertificationRecord/getAllForProperty',
    method: 'get',
    params: query
  })
}

// GET /vipOwner/getListForProperty
export function getTypeList() {
  return request({
    url: '/vipOwner/getListForProperty',
    method: 'get'
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
