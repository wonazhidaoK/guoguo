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

// 物业公司
export function fetchWyList() {
  return request({
    url: '/propertyCompany/getList',
    method: 'get'
  })
}

// 角色权限街道办
export function fetRoleListJdb() {
  return request({
    url: '/role/getAllForStreetOffice',
    method: 'get'
  })
}
// 角色权限物业
export function fetRoleListWy() {
  return request({
    url: '/role/getAllForProperty',
    method: 'get'
  })
}

// 角色权限
export function fetRoleList() {
  return request({
    url: '/role/getAll',
    method: 'get'
  })
}

// 账户管理 物业
export function createUserWy(data) {
  return request({
    url: '/user/addPropertyUser',
    method: 'post',
    data
  })
}

export function fetchUserWyList(query) {
  return request({
    url: '/user/GetAllPropertyUser',
    method: 'get',
    params: query
  })
}

export function updateUserWy(data) {
  return request({
    url: '/user/updatePropertyUser',
    method: 'post',
    data
  })
}

// 账户管理 街道办用户
export function fetUserJdbList(query) {
  return request({
    url: '/user/GetAllStreetOfficeUser',
    method: 'get',
    params: query
  })
}

export function createUserJdb(data) {
  return request({
    url: '/user/addStreetOfficeUser',
    method: 'post',
    data
  })
}

export function updateUserJdb(data) {
  return request({
    url: '/user/updateStreetOfficeUser',
    method: 'post',
    data
  })
}

export function delUserRequest(Id) {
  return request({
    url: '/user/delete',
    params: { Id }
  })
}

// 获取业委会 列表 业委会模块用
export function fetchYwhList(query) {
  return request({
    url: '/vipOwner/getAll',
    method: 'get',
    params: query
  })
}

// 创建也维护createYwh
export function createYwh(data) {
  return request({
    url: '/vipOwner/add',
    method: 'post',
    data
  })
}

// updateYwh 编辑业委会
export function updateYwh(data) {
  return request({
    url: '/vipOwner/update',
    method: 'post',
    data
  })
}

// delYwhRequest
export function delYwhRequest(Id) {
  return request({
    url: '/vipOwner/delete',
    params: { Id }
  })
}

// 业委会置为无效
export function wuxiaoRequest(Id) {
  return request({
    url: '/vipOwner/Invalid',
    params: { Id }
  })
}

// 新疆单元 createDy
// 新建楼宇 楼宇模块用POST /building/add
export function createDy(data) {
  // console.log(data)
  return request({
    url: '/buildingUnit/add',
    method: 'post',
    data
  })
}

// 编辑单元
export function updateDy(data) {
  return request({
    url: '/buildingUnit/update',
    method: 'post',
    data
  })
}

// 删除楼宇
export function delDyRequest(Id) {
  return request({
    url: '/buildingUnit/delete',
    params: { Id }
  })
}

//  fetchLyMesList// 获取楼宇单元 列表 楼宇模块用
export function fetchLyMesList(query) {
  return request({
    url: '/buildingUnit/getAll',
    method: 'get',
    params: query
  })
}

// 获取小区（新建 查询 时用的lsit ）楼宇模块用
export function getXiaoquList(communityId) {
  return request({
    url: '/smallDistrict/getList',
    method: 'get',
    params: { communityId }
  })
}

// 新建楼宇 楼宇模块用POST /building/add
export function createLy(data) {
  return request({
    url: '/building/add',
    method: 'post',
    data
  })
}

// 删除楼宇
export function delLyRequest(Id) {
  return request({
    url: '/building/delete',
    params: { Id }
  })
}

// 编辑楼宇
export function updateLy(data) {
  return request({
    url: '/building/update',
    method: 'post',
    data
  })
}

// 获取楼宇列表 楼宇模块用
export function fetchLyList(query) {
  return request({
    url: '/building/getAll',
    method: 'get',
    params: query
  })
}

// 获取社区（新建 查询 时用的lsit ）小区模块用
export function getShequList(streetOfficeId) {
  return request({
    url: '/community/getList',
    method: 'get',
    params: { streetOfficeId }
  })
}

// 新建小区 小区模块用
export function createXiaoqu(data) {
  return request({
    url: '/smallDistrict/add',
    method: 'post',
    data
  })
}

// 获取小区列表 小区模块用
export function fetchXiaoquList(query) {
  return request({
    url: '/smallDistrict/getAll',
    method: 'get',
    params: query
  })
}

// 删除小区
export function delXqRequest(Id) {
  return request({
    url: '/smallDistrict/delete',
    params: { Id }
  })
}

// 编辑小区
export function updateXiaoqu(data) {
  return request({
    url: '/smallDistrict/update',
    method: 'post',
    data
  })
}

// 获取街道办（新建 查询 时用的lsit ））社区模块用
export function getJbdList(state, city, region) {
  return request({
    url: '/streetOffice/getList',
    method: 'get',
    params: { state, city, region }
  })
}

// 新建社区 社区模块用
export function createShequ(data) {
  return request({
    url: '/community/add',
    method: 'post',
    data
  })
}

// 获取社区列表 社区模块用
export function fetchShequList(query) {
  return request({
    url: '/community/getAll',
    method: 'get',
    params: query
  })
}

// 删除社区
export function delSqRequest(Id) {
  return request({
    url: '/community/delete',
    params: { Id }
  })
}

// 编辑社区
export function updateShequ(data) {
  return request({
    url: '/community/update',
    method: 'post',
    data
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
