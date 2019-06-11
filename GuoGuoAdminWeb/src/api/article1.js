import request from '@/utils/request'

// 获取省
export function getState() {
  return request({
    url: '/city/getState',
    method: 'get'
  })
}

// 获取市
export function getCity(stateName) {
  return request({
    url: '/city/getCity',
    method: 'get',
    params: { stateName }
  })
}

// 获取区
export function getRegion(stateName, cityName) {
  return request({
    url: '/city/getRegion',
    method: 'get',
    params: { stateName, cityName }
  })
}

// 新建街道办
export function createJdb(data) {
  return request({
    url: '/streetOffice/add',
    method: 'post',
    data
  })
}

// 获取街道办列表
export function fetchList(query) {
  return request({
    url: '/streetOffice/getAll',
    method: 'get',
    params: query
  })
}

// 删除街道办
export function delJdbRequest(Id) {
  return request({
    url: '/streetOffice/delete',
    params: { Id }
  })
}

// 编辑街道办
export function updateJdb(data) {
  return request({
    url: '/streetOffice/update',
    method: 'post',
    data
  })
}

export function fetchArticle(id) {
  return request({
    url: '/article/detail',
    method: 'get',
    params: { id }
  })
}

export function fetchPv(pv) {
  return request({
    url: '/article/pv',
    method: 'get',
    params: { pv }
  })
}
