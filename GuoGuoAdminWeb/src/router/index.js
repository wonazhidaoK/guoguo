import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)

/* Layout */
import Layout from '@/views/layout/Layout'

/* Router Modules */
// import componentsRouter from './modules/components'
// import chartsRouter from './modules/charts'
// import tableRouter from './modules/table'

/** note: sub-menu only appear when children.length>=1
 *  detail see  https://panjiachen.github.io/vue-element-admin-site/guide/essentials/router-and-nav.html
 **/

/**
* hidden: true                   if `hidden:true` will not show in the sidebar(default is false)
* alwaysShow: true               if set true, will always show the root menu, whatever its child routes length
*                                if not set alwaysShow, only more than one route under the children
*                                it will becomes nested mode, otherwise not show the root menu
* redirect: noredirect           if `redirect:noredirect` will no redirect in the breadcrumb
* name:'router-name'             the name is used by <keep-alive> (must set!!!)
* meta : {
    roles: ['admin','editor']    will control the page roles (you can set multiple roles)
    title: 'title'               the name show in sub-menu and breadcrumb (recommend set)
    icon: 'svg-name'             the icon show in the sidebar
    noCache: true                if true, the page will no be cached(default is false)
    breadcrumb: false            if false, the item will hidden in breadcrumb(default is true)
    affix: true                  if true, the tag will affix in the tags-view
  }
**/

export const constantRouterMap = [
  {
    path: '/redirect',
    component: Layout,
    hidden: true,
    children: [
      {
        path: '/redirect/:path*',
        component: () => import('@/views/redirect/index')
      }
    ]
  },
  {
    path: '/login',
    component: () => import('@/views/login/index'),
    hidden: true
  },
  {
    path: '/auth-redirect',
    component: () => import('@/views/login/authredirect'),
    hidden: true
  },
  {
    path: '/404',
    component: () => import('@/views/errorPage/404'),
    hidden: true
  },
  {
    path: '/401',
    component: () => import('@/views/errorPage/401'),
    hidden: true
  },
  {
    path: '',
    component: Layout,
    redirect: 'dashboard',
    children: [
      {
        path: 'dashboard',
        component: () => import('@/views/dashboard/index'),
        name: 'Dashboard',
        meta: { title: 'dashboard', icon: 'dashboard', noCache: true, affix: true }
      }
    ]
  }
]

export default new Router({
  // mode: 'history', // require service support
  scrollBehavior: () => ({ y: 0 }),
  routes: constantRouterMap
})

export const asyncRouterMap = [
  // 商家后台
  {
    path: '/shop_info',
    component: Layout,
    redirect: '/shop_info/index',
    meta: {
      roles: ['shop_info']
    },
    children: [
      {
        path: 'index',
        component: () => import('@/views/shop_info/index'),
        name: 'shop_info',
        meta: { title: '商铺中心', icon: 'user', noCache: true }
      }
    ]
  },
  {
    path: '/shop_type',
    component: Layout,
    redirect: '/shop_type/index',
    meta: {
      roles: ['shop_type']
    },
    children: [
      {
        path: 'index',
        component: () => import('@/views/shop_type/index'),
        name: 'shop_type',
        meta: { title: '商品类别', icon: 'type', noCache: true }
      }
    ]
  },
  {
    path: '/shop_goods',
    component: Layout,
    redirect: '/shop_goods/index',
    meta: {
      roles: ['shop_goods']
    },
    children: [
      {
        path: 'index',
        component: () => import('@/views/shop_goods/index'),
        name: 'shop_goods',
        meta: { title: '商品管理', icon: 'spgl', noCache: true }
      }
    ]
  },
  {
    path: '/shop_order',
    component: Layout,
    redirect: '/shop_order/index',
    meta: {
      roles: ['shop_order']
    },
    children: [
      {
        path: 'index',
        component: () => import('@/views/shop_order/index'),
        name: 'shop_order',
        meta: { title: '订单管理', icon: 'ddgl', noCache: true }
      }
    ]
  },
  // {
  //   path: '/shop_order_num',
  //   component: Layout,
  //   redirect: '/shop_order_num/index',
  //   meta: {
  //     roles: ['shop_order_num']
  //   },
  //   children: [
  //     {
  //       path: 'index',
  //       component: () => import('@/views/shop_order_num/index'),
  //       name: 'shop_order_num',
  //       meta: { title: '订单统计', icon: 'tj', noCache: true }
  //     }
  //   ]
  // },
  // 物业后台
  // 业务工作 业户信息
  {
    path: '/wy_house_info',
    component: Layout,
    meta: {
      roles: ['wy_house_info']
    },
    children: [
      {
        path: 'wy_house_info',
        component: () => import('@/views/wy_household_information/index'),
        name: 'wy_house_info',
        meta: { title: '业户信息', icon: 'xx' }
      }
    ]
  },
  // 物业后台 公告
  {
    path: '/wy_notice',
    component: Layout,
    meta: {
      roles: ['wy_notice']
    },
    children: [
      {
        path: 'wy_notice',
        component: () => import('@/views/wy_notice/index'),
        name: 'wy_notice',
        meta: { title: '公告', icon: 'gg' }
      }
    ]
  },
  // 物业后台 投票
  {
    path: '/wy_vote',
    component: Layout,
    meta: {
      roles: ['wy_vote']
    },
    children: [
      {
        path: 'wy_vote',
        component: () => import('@/views/wy_vote/index'),
        name: 'wy_vote',
        meta: { title: '投票', icon: 'tp' }
      }
    ]
  },
  // 物业后台 投诉
  {
    path: '/wy_complaint',
    component: Layout,
    meta: {
      roles: ['wy_complaint']
    },
    children: [
      {
        path: 'wy_complaint',
        component: () => import('@/views/wy_complaint/index'),
        name: 'wy_complaint',
        meta: { title: '投诉', icon: 'ts' }
      }
    ]
  },
  // 物业后台 站内信
  {
    path: '/wy_letter',
    component: Layout,
    meta: {
      roles: ['wy_letter']
    },
    children: [
      {
        path: 'wy_letter',
        component: () => import('@/views/wy_letter/index'),
        name: 'wy_letter',
        meta: { title: '站内信', icon: 'znx' }
      }
    ]
  },
  {
    path: '/wy_owner',
    component: Layout,
    meta: {
      roles: ['wy_owner']
    },
    children: [
      {
        path: 'wy_owner',
        component: () => import('@/views/wy_owner/index'),
        name: 'wy_owner',
        meta: { title: '业委会成员', icon: 'znx' }
      }
    ]
  },
  {
    path: '/wy_shop',
    component: Layout,
    redirect: '/wy_shop/index',
    meta: {
      roles: ['wy_shop']
    },
    children: [
      {
        path: 'index',
        component: () => import('@/views/wy_shop/index'),
        name: 'wy_shop',
        meta: { title: '关联商家', icon: 'glsj', noCache: true }
      }
    ]
  },
  {
    path: '/wy_order',
    component: Layout,
    redirect: '/wy_order/index',
    meta: {
      roles: ['wy_order']
    },
    children: [
      {
        path: 'index',
        component: () => import('@/views/wy_order/index'),
        name: 'wy_order',
        meta: { title: '订单管理', icon: 'order', noCache: true }
      }
    ]
  },
  {
    path: '/wy_info',
    component: Layout,
    redirect: '/wy_info/index',
    meta: {
      roles: ['wy_info']
    },
    children: [
      {
        path: 'index',
        component: () => import('@/views/wy_info/index'),
        name: 'wy_info',
        meta: { title: '基本信息', icon: 'jbxx', noCache: true }
      }
    ]
  },
  {
    path: '/wy_user_info',
    component: Layout,
    redirect: '/wy_user_info/index',
    meta: {
      roles: ['wy_user_info']
    },
    children: [
      {
        path: 'index',
        component: () => import('@/views/wy_user_info/index'),
        name: 'wy_user_info',
        meta: { title: '个人信息', icon: 'user', noCache: true }
      }
    ]
  },
  // 街道办后台
  {
    path: '/jdb_rz',
    component: Layout,
    redirect: '/jdb_rz_gj',
    alwaysShow: true, // will always show the root menu
    meta: {
      title: '认证中心',
      icon: 'rz',
      roles: ['jdb_rz_gj', 'jdb_rz_role'] // you can set roles in root nav
    },
    name: '认证中心',
    children: [
      // 高级认证
      {
        path: '/jdb_rz_gj',
        component: () => import('@/views/jdb_gj/index'),
        name: 'jdb_rz_gj',
        meta: {
          title: '高级认证',
          roles: ['jdb_rz_gj']
        }
      },
      // 申请条件管理
      {
        path: '/jsb_rz_tj',
        component: () => import('@/views/jsb_rz_tj/index'),
        name: 'jsb_rz_tj',
        meta: {
          title: '申请条件管理',
          roles: ['jdb_rz_role']
        }
      },
      // 认证记录
      {
        path: '/jsb_rz_owner',
        component: () => import('@/views/jsb_rz_owner/index'),
        name: 'jsb_rz_owner',
        meta: {
          title: '认证记录',
          roles: ['jdb_rz_owner']
        }
      }
    ]
  },
  {
    path: '/jdb_notice',
    component: Layout,
    meta: {
      roles: ['jdb_notice']
    },
    children: [
      {
        path: 'jdb_notice',
        component: () => import('@/views/jdb_notice/index'),
        name: 'jdb_notice',
        meta: { title: '公告', icon: 'gg' }
      }
    ]
  },
  {
    path: '/jdb_vote',
    component: Layout,
    meta: {
      roles: ['jdb_vote']
    },
    children: [
      {
        path: 'jdb_vote',
        component: () => import('@/views/jdb_vote/index'),
        name: 'jdb_vote',
        meta: { title: '投票', icon: 'tp' }
      }
    ]
  },
  {
    path: '/jdb_complaint',
    component: Layout,
    meta: {
      roles: ['jdb_complaint']
    },
    children: [
      {
        path: 'jdb_complaint',
        component: () => import('@/views/jdb_complaint/index'),
        name: 'jdb_complaint',
        meta: { title: '投诉', icon: 'ts' }
      }
    ]
  },
  {
    path: '/jdb_letter',
    component: Layout,
    meta: {
      roles: ['jdb_letter']
    },
    children: [
      {
        path: 'jdb_letter',
        component: () => import('@/views/jdb_letter/index'),
        name: 'jdb_letter',
        meta: { title: '站内信', icon: 'znx' }
      }
    ]
  },
  {
    path: '/permission',
    component: Layout,
    redirect: '/permission/index',
    alwaysShow: true, // will always show the root menu
    meta: {
      title: 'permission',
      icon: 'lock',
      roles: ['admin', 'editor'] // you can set roles in root nav
    },
    children: [
      {
        path: 'page',
        component: () => import('@/views/permission/page'),
        name: 'PagePermission',
        meta: {
          title: 'pagePermission',
          roles: ['admin'] // or you can only set roles in sub nav
        }
      },
      {
        path: 'directive',
        component: () => import('@/views/permission/directive'),
        name: 'DirectivePermission',
        meta: {
          title: 'directivePermission'
          // if do not set roles, means: this page does not require permission
        }
      }
    ]
  },
  // 呙呙社区
  {
    path: '/ww_jdb',
    component: Layout,
    meta: {
      roles: ['authorityMax']
    },
    children: [
      {
        path: 'ww_jdb',
        component: () => import('@/views/ww_jdb/index'),
        name: 'ww_jdb',
        meta: { title: '街道办', icon: 'jd' }
      }
    ]
  },
  {
    path: '/ww_sq',
    component: Layout,
    meta: {
      roles: ['authorityMax']
    },
    children: [
      {
        path: 'ww_sq',
        component: () => import('@/views/ww_sq/index'),
        name: 'ww_sq',
        meta: { title: '社区', icon: 'sq' }
      }
    ]
  },
  {
    path: '/ww_xq',
    component: Layout,
    meta: {
      roles: ['authorityMax']
    },
    children: [
      {
        path: 'ww_xq',
        component: () => import('@/views/ww_xq/index'),
        name: 'ww_xq',
        meta: { title: '小区', icon: 'xq' }
      }
    ]
  },
  {
    path: '/ww_ly',
    component: Layout,
    meta: {
      roles: ['authorityMax']
    },
    children: [
      {
        path: 'ww_ly',
        component: () => import('@/views/ww_ly/index'),
        name: 'ww_ly',
        meta: { title: '楼宇', icon: 'chart' }
      }
    ]
  },
  {
    path: '/ww_ywh',
    component: Layout,
    meta: {
      roles: ['authorityMax']
    },
    children: [
      {
        path: 'ww_ywh',
        component: () => import('@/views/ww_ywh/index'),
        name: 'ww_ywh',
        meta: { title: '业委会', icon: 'ywh' }
      }
    ]
  },
  {
    path: '/ww_ywhjg',
    component: Layout,
    meta: {
      roles: ['authorityMax']
    },
    children: [
      {
        path: 'ww_ywhjg',
        component: () => import('@/views/ww_ywhjg/index'),
        name: 'ww_ywhjg',
        meta: { title: '业委会架构', icon: 'jg' }
      }
    ]
  },
  {
    path: '/ww_tslx',
    component: Layout,
    meta: {
      roles: ['authorityMax']
    },
    children: [
      {
        path: 'ww_tslx',
        component: () => import('@/views/ww_tslx/index'),
        name: 'ww_tslx',
        meta: { title: '投诉类型', icon: 'ts' }
      }
    ]
  },
  {
    path: '/ww_wy',
    component: Layout,
    meta: {
      roles: ['authorityMax']
    },
    children: [
      {
        path: 'ww_wy',
        component: () => import('@/views/ww_wy/index'),
        name: 'ww_wy',
        meta: { title: '物业公司', icon: 'wygs' }
      }
    ]
  },
  {
    path: '/ww_shop',
    component: Layout,
    meta: {
      roles: ['authorityMax']
    },
    children: [
      {
        path: 'ww_shop',
        component: () => import('@/views/ww_shop/index'),
        name: 'ww_shop',
        meta: { title: '商铺管理', icon: 'shophome' }
      }
    ]
  },
  {
    path: '/ww_goods',
    component: Layout,
    meta: {
      roles: ['authorityMax']
    },
    children: [
      {
        path: 'ww_goods',
        component: () => import('@/views/ww_goods/index'),
        name: 'ww_goods',
        meta: { title: '商品管理', icon: 'goods' }
      }
    ]
  },
  // wowo后台 账户管理
  {
    path: '/wowoAccount',
    component: Layout,
    redirect: '/ww_jdbAc',
    alwaysShow: true, // will always show the root menu
    meta: {
      title: '账户管理',
      icon: 'zh',
      roles: ['authorityMax'] // you can set roles in root nav
    },
    name: '账户管理',
    children: [
      // 街道办后台
      {
        path: '/ww_jdbAc',
        component: () => import('@/views/ww_jdb_account/index'),
        name: 'ww_jdbAc',
        meta: { title: '街道办' }
      },
      // 物业后台
      {
        path: '/ww_wyAc',
        component: () => import('@/views/ww_wy_account/index'),
        name: 'ww_wyAc',
        meta: { title: '物业' }
      },
      // 商铺后台
      {
        path: '/ww_shop_account',
        component: () => import('@/views/ww_shop_account/index'),
        name: 'ww_shop_account',
        meta: { title: '商铺' }
      },
      // 角色管理
      {
        path: '/ww_role',
        component: () => import('@/views/ww_role/index'),
        name: 'ww_role',
        meta: { title: '角色管理' }
      },
      {
        path: '/ww_menu',
        component: () => import('@/views/ww_menu/index'),
        name: 'ww_menu',
        meta: { title: '菜单管理', roles: ['authorityMaxauthorityMax'] }
      }
    ]
  },
  { path: '*', redirect: '/404', hidden: true }
  // { path: '*', redirect: '/401', hidden: true }
]
