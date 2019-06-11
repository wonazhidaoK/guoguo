
import request from '@/utils/request'

// 获取楼宇列表
export function getListLy(smallDistrictId) {
  return request({
    url: '/building/getList',
    params: { smallDistrictId }
  })
}

//  手动干预/vote/updateCalculationMethod
export function handFn(Id) {
  return request({
    url: '/vote/updateCalculationMethod',
    params: { Id }
  })
}

// 建议列表
export function fetchJyList(query) {
  return request({
    url: '/voteRecord/getFeedback',
    params: query
  })
}

// 获取楼宇列表GET /buildingUnit/getList
export function getListNotice(buildingId) {
  return request({
    url: '/buildingUnit/getList',
    params: { buildingId }
  })
}

// 新建投票
export function createvVote(p) {
  const data = {
    'Title': p.Title,
    'Summary': p.Summary,
    'Deadline': p.Deadline,
    'List': [
      {
        'Title': p.Question,
        'List': [
          {
            'Describe': '同意'
          }, {
            'Describe': '不同意'
          }
        ]
      }
    ],
    'AnnexId': p.AnnexId
  }
  return request({
    url: '/vote/addVoteForProperty',
    method: 'post',
    data
  })
}

// 获取公告列表GETGET GET /vote/getAllForProperty
export function fetchListVote(query) {
  return request({
    url: '/vote/getAllForProperty',
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
    url: '/vote/get',
    params: { Id }
  })
}
