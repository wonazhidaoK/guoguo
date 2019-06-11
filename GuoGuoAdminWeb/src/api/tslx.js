import request from '@/utils/request'

const string2Object = str => {
  return JSON.parse(str)
}

// 获取部门列表GET /department/getAlldepartment/getForComplaintAll
export function fetchListBm() {
  return request({
    url: '/department/getForComplaintAll',
    method: 'get'
  })
}

// 新建投诉
export function createZhineng(data) {
  // const data = {
  //   'Name': temp.Name,
  //   'Description': temp.Description,
  //   'Level': temp.Level,
  //   'InitiatingDepartmentName': string2Object(temp.Bumen).Name,
  //   'InitiatingDepartmentValue': string2Object(temp.Bumen).Value
  // }
  return request({
    url: '/complaintType/add',
    method: 'post',
    data
  })
}

// 获取投诉列表
export function fetchList(query) {
  return request({
    url: '/complaintType/getAll',
    method: 'get',
    params: query
  })
}

// 删除投诉
export function delRequest(Id) {
  return request({
    url: '/complaintType/delete',
    params: { Id }
  })
}

// 编辑投诉
export function update(temp) {
  const data = {
    'Id': temp.Id,
    'Name': temp.Name,
    'Description': temp.Description,
    'Level': temp.Level,
    'InitiatingDepartmentName': string2Object(temp.Bumen).Name,
    'InitiatingDepartmentValue': string2Object(temp.Bumen).Value
  }
  return request({
    url: '/complaintType/update',
    method: 'post',
    data
  })
}
