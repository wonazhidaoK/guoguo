(window.webpackJsonp=window.webpackJsonp||[]).push([["chunk-2a6a"],{Lcw6:function(e,t,a){"use strict";var i=a("qULk");a.n(i).a},Mz3J:function(e,t,a){"use strict";Math.easeInOutQuad=function(e,t,a,i){return(e/=i/2)<1?a/2*e*e+t:-a/2*(--e*(e-2)-1)+t};var i=window.requestAnimationFrame||window.webkitRequestAnimationFrame||window.mozRequestAnimationFrame||function(e){window.setTimeout(e,1e3/60)};function n(e,t,a){var n=document.documentElement.scrollTop||document.body.parentNode.scrollTop||document.body.scrollTop,l=e-n,o=0;t=void 0===t?500:t;!function e(){o+=20,function(e){document.documentElement.scrollTop=e,document.body.parentNode.scrollTop=e,document.body.scrollTop=e}(Math.easeInOutQuad(o,n,l,t)),o<t?i(e):a&&"function"==typeof a&&a()}()}var l={name:"Pagination",props:{total:{required:!0,type:Number},page:{type:Number,default:1},limit:{type:Number,default:20},pageSizes:{type:Array,default:function(){return[10,20,30,50]}},layout:{type:String,default:"total, sizes, prev, pager, next, jumper"},background:{type:Boolean,default:!0},autoScroll:{type:Boolean,default:!0},hidden:{type:Boolean,default:!1}},computed:{currentPage:{get:function(){return this.page},set:function(e){this.$emit("update:page",e)}},pageSize:{get:function(){return this.limit},set:function(e){this.$emit("update:limit",e)}}},methods:{handleSizeChange:function(e){this.$emit("pagination",{page:this.currentPage,limit:e}),this.autoScroll&&n(0,800)},handleCurrentChange:function(e){this.$emit("pagination",{page:e,limit:this.pageSize}),this.autoScroll&&n(0,800)}}},o=(a("Lcw6"),a("KHd+")),s=Object(o.a)(l,function(){var e=this,t=e.$createElement,a=e._self._c||t;return a("div",{staticClass:"pagination-container",class:{hidden:e.hidden}},[a("el-pagination",e._b({attrs:{background:e.background,"current-page":e.currentPage,"page-size":e.pageSize,layout:e.layout,"page-sizes":e.pageSizes,total:e.total},on:{"update:currentPage":function(t){e.currentPage=t},"update:pageSize":function(t){e.pageSize=t},"size-change":e.handleSizeChange,"current-change":e.handleCurrentChange}},"el-pagination",e.$attrs,!1))],1)},[],!1,null,"331ed7d4",null);s.options.__file="index.vue";t.a=s.exports},ZySA:function(e,t,a){"use strict";var i=a("P2sY"),n=a.n(i),l=(a("jUE0"),{bind:function(e,t){e.addEventListener("click",function(a){var i=n()({},t.value),l=n()({ele:e,type:"hit",color:"rgba(0, 0, 0, 0.15)"},i),o=l.ele;if(o){o.style.position="relative",o.style.overflow="hidden";var s=o.getBoundingClientRect(),r=o.querySelector(".waves-ripple");switch(r?r.className="waves-ripple":((r=document.createElement("span")).className="waves-ripple",r.style.height=r.style.width=Math.max(s.width,s.height)+"px",o.appendChild(r)),l.type){case"center":r.style.top=s.height/2-r.offsetHeight/2+"px",r.style.left=s.width/2-r.offsetWidth/2+"px";break;default:r.style.top=(a.pageY-s.top-r.offsetHeight/2-document.documentElement.scrollTop||document.body.scrollTop)+"px",r.style.left=(a.pageX-s.left-r.offsetWidth/2-document.documentElement.scrollLeft||document.body.scrollLeft)+"px"}return r.style.backgroundColor=l.color,r.className="waves-ripple z-active",!1}},!1)}}),o=function(e){e.directive("waves",l)};window.Vue&&(window.waves=l,Vue.use(o)),l.install=o;t.a=l},"gDS+":function(e,t,a){e.exports={default:a("oh+g"),__esModule:!0}},jUE0:function(e,t,a){},"oh+g":function(e,t,a){var i=a("WEpk"),n=i.JSON||(i.JSON={stringify:JSON.stringify});e.exports=function(e){return n.stringify.apply(n,arguments)}},pDsk:function(e,t,a){"use strict";a.r(t);var i=a("P2sY"),n=a.n(i),l=a("gDS+"),o=a.n(l),s=a("t3Un"),r=function(e){return JSON.parse(e)};var u=a("ZySA"),c=a("Mz3J"),d=[{key:"CN",display_name:"China"},{key:"US",display_name:"USA"},{key:"JP",display_name:"Japan"},{key:"EU",display_name:"Eurozone"}],p=d.reduce(function(e,t){return e[t.key]=t.display_name,e},{}),m={name:"ComplexTable",components:{Pagination:c.a},directives:{waves:u.a},filters:{statusFilter:function(e){return{published:"success",draft:"info",deleted:"danger"}[e]},typeFilter:function(e){return p[e]},object2String:function(e){return o()(e)}},data:function(){return{tableKey:0,list:null,total:0,listLoading:!0,listQuery:{pageIndex:1,pageSize:20},importanceOptions:[1,2,3],calendarTypeOptions:d,sortOptions:[{label:"ID Ascending",key:"+id"},{label:"ID Descending",key:"-id"}],statusOptions:["published","draft","deleted"],showReviewer:!1,temp:{Name:"",Level:"",Description:"",InitiatingDepartmentValue:""},levelList:[1,2,3,4,5,6,7,8],dialogFormVisible:!1,dialogStatus:"",textMap:{update:"编辑投诉类型",create:"新建投诉类型"},dialogPvVisible:!1,pvData:[],rules:{Name:[{required:!0,message:"投诉类型名称是必填的",trigger:"blur"}],Level:[{required:!0,message:"投诉级别是必选的",trigger:"change"}],Description:[{required:!0,message:"描述是必填的",trigger:"blur"}],InitiatingDepartmentValue:[{required:!0,message:"部门是必选的",trigger:"change"}]},bmlist:[],createLoading:!1,num:0}},created:function(){this.getList(),this.getBumen()},methods:{getList:function(){var e=this;this.listLoading=!0,function(e){return Object(s.a)({url:"/complaintType/getAll",method:"get",params:e})}(this.listQuery).then(function(t){e.list=t.data.data.List,e.total=t.data.data.TotalCount,setTimeout(function(){e.listLoading=!1},1500)})},getBumen:function(){var e=this;Object(s.a)({url:"/department/getForComplaintAll",method:"get"}).then(function(t){e.bmlist=t.data.data})},handleFilter:function(){this.listQuery.pageIndex=1,this.getList()},handleModifyStatus:function(e,t){this.$message({message:"操作成功",type:"success"}),e.status=t},delBtn:function(e,t){var a=this;this.$confirm("确定要注销吗?","提示",{confirmButtonText:"确定",cancelButtonText:"取消",type:"delete"}).then(function(){(function(e){return Object(s.a)({url:"/complaintType/delete",params:{Id:e}})})(t).then(function(e){a.getList(),a.$notify({title:"成功",message:"注销成功",type:"success",duration:2e3})})}).catch(function(){})},resetTemp:function(){this.temp={Name:"",Level:"",Description:"",InitiatingDepartmentValue:""}},handleCreate:function(){var e=this;this.resetTemp(),this.dialogStatus="create",this.dialogFormVisible=!0,this.createLoading=!1,this.$nextTick(function(){e.$refs.dataForm.clearValidate()})},createData:function(){var e=this;this.$refs.dataForm.validate(function(t){t&&(e.num=e.num+1,e.num<2&&(e.createLoading=!0,function(e){return Object(s.a)({url:"/complaintType/add",method:"post",data:e})}(e.temp).then(function(t){e.getList(),e.$notify({title:"成功",message:"创建成功",type:"success",duration:2e3}),e.dialogFormVisible=!1,e.createLoading=!1,setTimeout(function(){e.num=0},2e3)}).catch(function(t){e.createLoading=!1,setTimeout(function(){e.num=0},2e3)})))})},handleUpdate:function(e){var t=this,a={Name:e.InitiatingDepartmentValue,Value:e.InitiatingDepartmentValue};e.Bumen=o()(a),this.temp=n()({},e),this.dialogStatus="update",this.dialogFormVisible=!0,this.$nextTick(function(){t.$refs.dataForm.clearValidate()})},updateData:function(){var e=this;this.$refs.dataForm.validate(function(t){if(t){var a=n()({},e.temp);e.num=e.num+1,e.num<2&&(e.createLoading=!0,function(e){var t={Id:e.Id,Name:e.Name,Description:e.Description,Level:e.Level,InitiatingDepartmentName:r(e.Bumen).Name,InitiatingDepartmentValue:r(e.Bumen).Value};return Object(s.a)({url:"/complaintType/update",method:"post",data:t})}(a).then(function(t){e.getList(),e.$notify({title:"成功",message:"编辑成功",type:"success",duration:2e3}),e.dialogFormVisible=!1,e.createLoading=!1,setTimeout(function(){e.num=0},2e3)}).catch(function(t){e.createLoading=!1,setTimeout(function(){e.num=0},2e3)}))}})}}},f=a("KHd+"),g=Object(f.a)(m,function(){var e=this,t=e.$createElement,a=e._self._c||t;return a("div",{staticClass:"app-container"},[a("div",{staticClass:"filter-container"},[a("el-input",{staticClass:"filter-item",staticStyle:{width:"200px"},attrs:{placeholder:"投诉名称"},nativeOn:{keyup:function(t){return"button"in t||!e._k(t.keyCode,"enter",13,t.key,"Enter")?e.handleFilter(t):null}},model:{value:e.listQuery.Name,callback:function(t){e.$set(e.listQuery,"Name",t)},expression:"listQuery.Name"}}),e._v(" "),a("el-input",{staticClass:"filter-item",staticStyle:{width:"200px"},attrs:{placeholder:"投诉描述"},nativeOn:{keyup:function(t){return"button"in t||!e._k(t.keyCode,"enter",13,t.key,"Enter")?e.handleFilter(t):null}},model:{value:e.listQuery.Description,callback:function(t){e.$set(e.listQuery,"Description",t)},expression:"listQuery.Description"}}),e._v(" "),a("el-select",{staticClass:"filter-item",staticStyle:{width:"150px"},attrs:{placeholder:"发起部门",clearable:""},on:{change:function(t){e.handleFilter()}},model:{value:e.listQuery.initiatingDepartmentValue,callback:function(t){e.$set(e.listQuery,"initiatingDepartmentValue",t)},expression:"listQuery.initiatingDepartmentValue"}},e._l(e.bmlist,function(e){return a("el-option",{key:e.Name,attrs:{label:e.Name,value:e.Value}})})),e._v(" "),a("el-button",{directives:[{name:"waves",rawName:"v-waves"}],staticClass:"filter-item",attrs:{type:"primary",icon:"el-icon-search"},on:{click:e.handleFilter}},[e._v(e._s(e.$t("table.search")))]),e._v(" "),a("el-button",{staticClass:"filter-item",staticStyle:{"margin-left":"10px"},attrs:{type:"primary",icon:"el-icon-edit"},on:{click:e.handleCreate}},[e._v(e._s(e.$t("table.register")))])],1),e._v(" "),a("el-table",{directives:[{name:"loading",rawName:"v-loading",value:e.listLoading,expression:"listLoading"}],key:e.tableKey,staticStyle:{width:"100%"},attrs:{data:e.list,border:"",fit:"","highlight-current-row":""}},[a("el-table-column",{attrs:{label:"投诉类型名称",align:"center"},scopedSlots:e._u([{key:"default",fn:function(t){return[a("span",[e._v(e._s(t.row.Name))])]}}])}),e._v(" "),a("el-table-column",{attrs:{label:"投诉级别",align:"center"},scopedSlots:e._u([{key:"default",fn:function(t){return[a("span",[e._v(e._s(t.row.Level))])]}}])}),e._v(" "),a("el-table-column",{attrs:{label:"发起部门",align:"center"},scopedSlots:e._u([{key:"default",fn:function(t){return[a("span",[e._v(e._s(t.row.InitiatingDepartmentName))])]}}])}),e._v(" "),a("el-table-column",{attrs:{label:"描述",align:"center"},scopedSlots:e._u([{key:"default",fn:function(t){return[a("span",[e._v(e._s(t.row.Description))])]}}])}),e._v(" "),a("el-table-column",{attrs:{label:e.$t("table.actions"),align:"center","class-name":"small-padding fixed-width"},scopedSlots:e._u([{key:"default",fn:function(t){return[a("el-button",{attrs:{type:"primary",size:"mini"},on:{click:function(a){e.handleUpdate(t.row)}}},[e._v(e._s(e.$t("table.edit")))]),e._v(" "),"deleted"!=t.row.status?a("el-button",{attrs:{size:"mini",type:"danger"},on:{click:function(a){e.delBtn(t.row,t.row.Id)}}},[e._v(e._s(e.$t("table.logoutBtn"))+"\n        ")]):e._e()]}}])})],1),e._v(" "),a("pagination",{directives:[{name:"show",rawName:"v-show",value:e.total>0,expression:"total>0"}],attrs:{total:e.total,page:e.listQuery.pageIndex,limit:e.listQuery.pageSize},on:{"update:page":function(t){e.$set(e.listQuery,"pageIndex",t)},"update:limit":function(t){e.$set(e.listQuery,"pageSize",t)},pagination:e.getList}}),e._v(" "),a("el-dialog",{attrs:{title:e.textMap[e.dialogStatus],visible:e.dialogFormVisible},on:{"update:visible":function(t){e.dialogFormVisible=t}}},[a("el-form",{ref:"dataForm",staticStyle:{width:"100%","box-sizing":"border-box",padding:"0 50px"},attrs:{rules:e.rules,model:e.temp,"label-position":"left","label-width":"120px"}},[a("el-form-item",{attrs:{label:"投诉类型",prop:"Name"}},[a("el-input",{attrs:{placeholder:"请输入投诉类型名称"},model:{value:e.temp.Name,callback:function(t){e.$set(e.temp,"Name",t)},expression:"temp.Name"}})],1),e._v(" "),a("el-form-item",{attrs:{label:"投诉描述",prop:"Description"}},[a("el-input",{attrs:{placeholder:"请输入投诉描述"},model:{value:e.temp.Description,callback:function(t){e.$set(e.temp,"Description",t)},expression:"temp.Description"}})],1),e._v(" "),a("el-form-item",{attrs:{label:"投诉级别",prop:"Level"}},[a("el-select",{staticClass:"filter-item",attrs:{placeholder:"请选择"},model:{value:e.temp.Level,callback:function(t){e.$set(e.temp,"Level",t)},expression:"temp.Level"}},e._l(e.levelList,function(e){return a("el-option",{key:e,attrs:{label:e,value:e}})}))],1),e._v(" "),a("el-form-item",{directives:[{name:"show",rawName:"v-show",value:"create"===e.dialogStatus,expression:"dialogStatus==='create'"}],attrs:{label:e.$t("table.Bumen"),prop:"InitiatingDepartmentValue"}},[a("el-select",{staticClass:"filter-item",attrs:{placeholder:"发起部门"},model:{value:e.temp.InitiatingDepartmentValue,callback:function(t){e.$set(e.temp,"InitiatingDepartmentValue",t)},expression:"temp.InitiatingDepartmentValue"}},e._l(e.bmlist,function(e){return a("el-option",{key:e.Name,attrs:{label:e.Name,value:e.Value}})}))],1)],1),e._v(" "),a("div",{staticClass:"dialog-footer",attrs:{slot:"footer"},slot:"footer"},[a("el-button",{on:{click:function(t){e.dialogFormVisible=!1}}},[e._v(e._s(e.$t("table.cancel")))]),e._v(" "),a("el-button",{directives:[{name:"loading",rawName:"v-loading",value:e.createLoading,expression:"createLoading"}],attrs:{type:"primary"},on:{click:function(t){"create"===e.dialogStatus?e.createData():e.updateData()}}},[e._v(e._s(e.$t("table.confirm")))])],1)],1),e._v(" "),a("el-dialog",{attrs:{visible:e.dialogPvVisible,title:"Reading statistics"},on:{"update:visible":function(t){e.dialogPvVisible=t}}},[a("el-table",{staticStyle:{width:"100%"},attrs:{data:e.pvData,border:"",fit:"","highlight-current-row":""}},[a("el-table-column",{attrs:{prop:"key",label:"Channel"}}),e._v(" "),a("el-table-column",{attrs:{prop:"pv",label:"Pv"}})],1),e._v(" "),a("span",{staticClass:"dialog-footer",attrs:{slot:"footer"},slot:"footer"},[a("el-button",{attrs:{type:"primary"},on:{click:function(t){e.dialogPvVisible=!1}}},[e._v(e._s(e.$t("table.confirm")))])],1)],1)],1)},[],!1,null,null,null);g.options.__file="index.vue";t.default=g.exports},qULk:function(e,t,a){}}]);