import request from '@/utils/request'

// 获取楼宇列表
export function getListLy(smallDistrictId) {
  return request({
    url: '/building/getList',
    params: { smallDistrictId }
  })
}

// 获取 该业户下 业主信息GET POST /owner/add
export function fetchUserMesDetail(id) {
  return request({
    url: '/owner/get',
    method: 'get',
    params: { id }
  })
}

// 获取 该业户下 业主 列表 GET POST /owner/add
export function fetchUserMesList(query) {
  return request({
    url: '/owner/getAll',
    method: 'get',
    params: query
  })
}

// 获取楼宇列表GET /buildingUnit/getList
export function getListDy(buildingId) {
  return request({
    url: '/buildingUnit/getList',
    params: { buildingId }
  })
}

// 新建业户信息/industry/add
export function createYehuUser(data) {
  return request({
    url: '/industry/add',
    method: 'post',
    data
  })
}

// 新建业主信息/industry/add
export function createYezhuUser(data) {
  return request({
    url: '/owner/add',
    method: 'post',
    data
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

// 获取业户列表GET /industry/getAll
export function fetchListYehu(query) {
  return request({
    url: '/industry/getAll',
    method: 'get',
    params: query
  })
}

// 编辑业户
export function updateLy(data) {
  return request({
    url: '/industry/update',
    method: 'post',
    data
  })
}

// 编辑业主
export function updateUser(data) {
  return request({
    url: '/owner/update',
    method: 'post',
    data
  })
}

// 删除业户
export function delYehu(Id) {
  return request({
    url: '/industry/delete',
    params: { Id }
  })
}

// 删除业主
export function delUser(Id) {
  return request({
    url: '/owner/delete',
    params: { Id }
  })
}
