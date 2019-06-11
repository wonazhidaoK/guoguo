
import request from '@/utils/request'

// 公告详情
export function detailNotice(Id) {
  return request({
    url: '/vote/get',
    params: { Id }
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

// 获取公告列表GETGET /announcement/getListPropertyAnnouncement
export function fetchListNotice(query) {
  return request({
    url: '/announcement/getListPropertyAnnouncement',
    method: 'get',
    params: query
  })
}

// 编辑
export function updateLy(data) {
  return request({
    url: '/industry/update',
    method: 'post',
    data
  })
}

// 删除GET /announcement/delete
export function delNotice(Id) {
  return request({
    url: '/announcement/delete',
    params: { Id }
  })
}
