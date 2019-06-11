
import request from '@/utils/request'

// 获取公告列表GETGET GET /vote/getAllForProperty
export function fetchListLetter(query) {
  return request({
    url: '/stationLetter/getAllPropertyStationLetter',
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

// GET /vote/getGET /stationLetter/getPropertyStationLetter
export function detailVote(Id) {
  return request({
    url: '/stationLetter/getPropertyStationLetter',
    params: { Id }
  })
}
