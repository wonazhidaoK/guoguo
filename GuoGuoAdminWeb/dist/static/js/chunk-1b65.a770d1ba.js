(window.webpackJsonp=window.webpackJsonp||[]).push([["chunk-1b65"],{"2kEW":function(e,t,i){"use strict";var a=i("JruU");i.n(a).a},"9TPh":function(e,t,i){"use strict";var a=i("eeMe"),n=i.n(a),s=(i("e8E5"),i("X4fA"));n.a.autoDiscover=!1;var l={props:{id:{type:String,required:!0},url:{type:String,required:!0},clickable:{type:Boolean,default:!0},initbol:{type:Boolean,default:!0},defaultMsg:{type:String,default:"上传附件"},acceptedFiles:{type:String,default:""},thumbnailHeight:{type:Number,default:200},thumbnailWidth:{type:Number,default:200},showRemoveLink:{type:Boolean,default:!0},maxFilesize:{type:Number,default:2},maxFiles:{type:Number,default:1},autoProcessQueue:{type:Boolean,default:!0},useCustomDropzoneOptions:{type:Boolean,default:!1},defaultImg:{default:"",type:[String,Array]},couldPaste:{type:Boolean,default:!1}},data:function(){return{dropzone:"",initOnce:!0}},watch:{defaultImg:function(e){0!==e.length?this.initOnce&&(this.initImages(e),this.initOnce=!1):this.initOnce=!1}},mounted:function(){var e=this,t=document.getElementById(this.id),i=this;this.dropzone=new n.a(t,{headers:{Authorization:Object(s.a)()},clickable:this.clickable,thumbnailWidth:this.thumbnailWidth,thumbnailHeight:this.thumbnailHeight,maxFiles:this.maxFiles,maxFilesize:this.maxFilesize,dictRemoveFile:"删除",addRemoveLinks:this.showRemoveLink,acceptedFiles:this.acceptedFiles,autoProcessQueue:this.autoProcessQueue,dictFileTooBig:"该文件大小为({{filesize}}M).最大不能超过: {{maxFilesize}}M.",dictDefaultMessage:'<i style="margin-top: 3em;display: inline-block" class="material-icons">'+this.defaultMsg+"</i><br>",dictMaxFilesExceeded:"目前只支持上传一个附件",previewTemplate:'<div class="dz-preview dz-file-preview">  <div class="dz-image" style="width:'+this.thumbnailWidth+"px;height:"+this.thumbnailHeight+'px" ><img style="width:'+this.thumbnailWidth+"px;height:"+this.thumbnailHeight+'px" data-dz-thumbnail /></div>  <div class="dz-details"><div class="dz-size"><span data-dz-size></span></div> <div class="dz-progress"><span class="dz-upload" data-dz-uploadprogress></span></div>  <div class="dz-error-message"><span data-dz-errormessage></span></div>  <div class="dz-success-mark"> <i class="material-icons">done</i> </div>  <div class="dz-error-mark"><i class="material-icons">失败</i></div></div>',init:function(){var e=this,t=i.defaultImg;if(t)if(Array.isArray(t)){if(0===t.length)return;t.map(function(t,a){var n={name:"name"+a,size:12345,url:t};return e.options.addedfile.call(e,n),e.options.thumbnail.call(e,n,t),n.previewElement.classList.add("dz-success"),n.previewElement.classList.add("dz-complete"),i.initOnce=!1,!0})}else{var a={name:"name",size:12345,url:t};this.options.addedfile.call(this,a),this.options.thumbnail.call(this,a,t),a.previewElement.classList.add("dz-success"),a.previewElement.classList.add("dz-complete"),i.initOnce=!1}},accept:function(e,t){t()},sending:function(e,t,a){i.initOnce=!1}}),this.couldPaste&&document.addEventListener("paste",this.pasteImg),this.dropzone.on("success",function(t){i.$emit("dropzone-success",t,i.dropzone.element,e.dropzone)}),this.dropzone.on("addedfile",function(e){i.$emit("dropzone-fileAdded",e)}),this.dropzone.on("removedfile",function(e){i.$emit("dropzone-removedFile",e)}),this.dropzone.on("error",function(e,t,a){i.$emit("dropzone-error",e,t,a)}),this.dropzone.on("successmultiple",function(e,t,a){i.$emit("dropzone-successmultiple",e,t,a)})},destroyed:function(){document.removeEventListener("paste",this.pasteImg),this.dropzone.destroy()},methods:{removeAllFiles:function(){this.dropzone.removeAllFiles(!0)},processQueue:function(){this.dropzone.processQueue()},pasteImg:function(e){var t=(e.clipboardData||e.originalEvent.clipboardData).items;"file"===t[0].kind&&this.dropzone.addFile(t[0].getAsFile())},initImages:function(e){var t=this;if(e)if(Array.isArray(e))e.map(function(e,i){var a={name:"name"+i,size:12345,url:e};return t.dropzone.options.addedfile.call(t.dropzone,a),t.dropzone.options.thumbnail.call(t.dropzone,a,e),a.previewElement.classList.add("dz-success"),a.previewElement.classList.add("dz-complete"),!0});else{var i={name:"name",size:12345,url:e};this.dropzone.options.addedfile.call(this.dropzone,i),this.dropzone.options.thumbnail.call(this.dropzone,i,e),i.previewElement.classList.add("dz-success"),i.previewElement.classList.add("dz-complete")}}}},o=(i("2kEW"),i("KHd+")),r=Object(o.a)(l,function(){var e=this.$createElement,t=this._self._c||e;return t("div",{ref:this.id,staticClass:"dropzone",attrs:{action:this.url,id:this.id}},[t("input",{attrs:{type:"file",name:"file"}})])},[],!1,null,"3c886344",null);r.options.__file="index.vue";t.a=r.exports},JruU:function(e,t,i){},Lcw6:function(e,t,i){"use strict";var a=i("qULk");i.n(a).a},Mz3J:function(e,t,i){"use strict";Math.easeInOutQuad=function(e,t,i,a){return(e/=a/2)<1?i/2*e*e+t:-i/2*(--e*(e-2)-1)+t};var a=window.requestAnimationFrame||window.webkitRequestAnimationFrame||window.mozRequestAnimationFrame||function(e){window.setTimeout(e,1e3/60)};function n(e,t,i){var n=document.documentElement.scrollTop||document.body.parentNode.scrollTop||document.body.scrollTop,s=e-n,l=0;t=void 0===t?500:t;!function e(){l+=20,function(e){document.documentElement.scrollTop=e,document.body.parentNode.scrollTop=e,document.body.scrollTop=e}(Math.easeInOutQuad(l,n,s,t)),l<t?a(e):i&&"function"==typeof i&&i()}()}var s={name:"Pagination",props:{total:{required:!0,type:Number},page:{type:Number,default:1},limit:{type:Number,default:20},pageSizes:{type:Array,default:function(){return[10,20,30,50]}},layout:{type:String,default:"total, sizes, prev, pager, next, jumper"},background:{type:Boolean,default:!0},autoScroll:{type:Boolean,default:!0},hidden:{type:Boolean,default:!1}},computed:{currentPage:{get:function(){return this.page},set:function(e){this.$emit("update:page",e)}},pageSize:{get:function(){return this.limit},set:function(e){this.$emit("update:limit",e)}}},methods:{handleSizeChange:function(e){this.$emit("pagination",{page:this.currentPage,limit:e}),this.autoScroll&&n(0,800)},handleCurrentChange:function(e){this.$emit("pagination",{page:e,limit:this.pageSize}),this.autoScroll&&n(0,800)}}},l=(i("Lcw6"),i("KHd+")),o=Object(l.a)(s,function(){var e=this,t=e.$createElement,i=e._self._c||t;return i("div",{staticClass:"pagination-container",class:{hidden:e.hidden}},[i("el-pagination",e._b({attrs:{background:e.background,"current-page":e.currentPage,"page-size":e.pageSize,layout:e.layout,"page-sizes":e.pageSizes,total:e.total},on:{"update:currentPage":function(t){e.currentPage=t},"update:pageSize":function(t){e.pageSize=t},"size-change":e.handleSizeChange,"current-change":e.handleCurrentChange}},"el-pagination",e.$attrs,!1))],1)},[],!1,null,"331ed7d4",null);o.options.__file="index.vue";t.a=o.exports},Z6sn:function(e,t,i){"use strict";i.r(t);var a=i("P2sY"),n=i.n(a),s=i("QbLZ"),l=i.n(s),o=i("L2JU"),r=i("t3Un");var d=i("ZySA"),c=i("Mz3J"),u=i("9TPh"),p=i("MkI2"),m={name:"ComplexTable",components:{Pagination:c.a,Dropzone:u.a,imgPreview:p.a},directives:{waves:d.a},filters:{statusFilter:function(e){return{published:"success",draft:"info",deleted:"danger"}[e]}},data:function(){return{dialogImg:!1,nowimg:"",imageList:["https://www.guoguoshequ.com//Upload/Announcement/95-16060G40F5.jpg","https://www.guoguoshequ.com//Upload/Announcement/95-16060G40F5.jpg","https://www.guoguoshequ.com//Upload/Announcement/95-16060G40F5.jpg"],activeNames:["1"],tableKey:0,list:null,total:0,typeList:[{VoteTypeValue:"RecallProperty",Name:"发起倡议"},{VoteTypeValue:"Ordinary",Name:"普通投票"},{VoteTypeValue:"VipOwnerElection",Name:"业委会重组"}],listLoading:!0,listQuery:{pageIndex:1,pageSize:20},temp:{Summary:"",Deadline:"",SmallDistrictId:"",VipOwnerId:""},dialogFormVisible:!1,dialogStatus:"",textMap:{update:"编辑业委会选举",create:"新建业委会选举"},rules:{Summary:[{required:!0,message:"摘要是必填的",trigger:"blur"}],Deadline:[{required:!0,message:"时间是必选的",trigger:"change"}],SmallDistrictId:[{required:!0,message:"小区是必填的",trigger:"change"}],VipOwnerId:[{required:!0,message:"业委会是必填的",trigger:"change"}],electionNumber:[{required:!0,trigger:"blur",validator:function(e,t,i){if(!t)return i(new Error("竞选人数不能为空"));if(!/^[0-9]+$/.test(t))return i(new Error("竞选人数必须是数字"));i()}}]},voteDetailCon:"",dialogPvVisible:!1,smalllist:"",ywhlist:[],createLoading:!1,num:0}},computed:l()({},Object(o.b)(["loginuser"])),created:function(){this.getSmall(),this.rootUrl="http://192.168.2.133:2223"},methods:{handleChange:function(e){},$imgPreview:function(e){this.dialogImg=!0,this.nowimg=e},addfile:function(e){this.createLoading=!0},imgError:function(e){this.createLoading=!1},dropzoneS:function(e){var t=JSON.parse(e.xhr.response);"200"===t.code?(this.temp.AnnexId=t.data.Id,this.$message({message:"上传成功",type:"success"})):this.$message({message:"上传失败",type:"error"}),this.createLoading=!1},dropzoneR:function(e){this.temp.AnnexId=""},getList:function(e){var t=this;"first"===e?this.listLoading=!1:(this.listLoading=!0,function(e){return Object(r.a)({url:"/vipOwnerCertification/getAll",method:"get",params:e})}(this.listQuery).then(function(e){t.list=e.data.data.List,t.total=e.data.data.TotalCount,setTimeout(function(){t.listLoading=!1},1500)}))},getSmall:function(e){var t=this;(function(e){return Object(r.a)({url:"/smallDistrict/getAllForStreetOfficeId",params:{StreetOfficeId:e}})})(this.$store.getters.loginuser.StreetOfficeId).then(function(e){var i=e.data.data.List;i=i.filter(function(e){return e.check=!1,e}),t.smalllist=i,t.getList("first")})},handleFilter:function(){this.listQuery.pageIndex=1,this.getList()},changeSmall:function(){var e=this;this.temp.VipOwnerId="",this.ywhlist="",function(e){return Object(r.a)({url:"/vipOwner/getList",params:{smallDistrictId:e}})}(this.temp.SmallDistrictId).then(function(t){e.ywhlist=t.data.data})},successBtn:function(e){var t=this;(function(e){return Object(r.a)({url:"/vipOwnerCertification/adopt",method:"post",data:{Id:e}})})(e.Id).then(function(e){t.getList(),t.$notify({title:"成功",message:"通过成功",type:"success",duration:2e3})})},handleModifyStatus:function(e,t){this.$message({message:"操作成功",type:"success"}),e.status=t},delVoteBtn:function(e,t){var i=this;(function(e){return Object(r.a)({url:"//delete",params:{Id:e}})})(t).then(function(e){i.getList(),i.$notify({title:"成功",message:"删除成功",type:"success",duration:2e3})})},voteDetail:function(e,t){var i=this;(function(e){return Object(r.a)({url:"/vipOwnerCertification/get",params:{Id:e}})})(t).then(function(e){i.dialogPvVisible=!0,i.activeNames=["1"],i.voteDetailCon=e.data.data})},resetTemp:function(){this.temp={Summary:"",Deadline:"",SmallDistrictId:"",VipOwnerId:""}},handleCreate:function(){var e=this;this.resetTemp(),this.dialogStatus="create",this.dialogFormVisible=!0,this.$nextTick(function(){e.$refs.dataForm.clearValidate()})},createData:function(){var e=this;this.$refs.dataForm.validate(function(t){t&&(e.num=e.num+1,e.num<2&&(e.createLoading=!0,function(e){return Object(r.a)({url:"/vote/addVoteForVipOwnerElection",method:"post",data:e})}(e.temp).then(function(t){e.$notify({title:"成功",message:"创建成功",type:"success",duration:2e3}),e.dialogFormVisible=!1,e.createLoading=!1,setTimeout(function(){e.num=0},2e3)}).catch(function(t){e.createLoading=!1,setTimeout(function(){e.num=0},2e3)})))})},handleUpdate:function(e){var t=this;this.temp=n()({},e),this.dialogStatus="update",this.dialogFormVisible=!0,this.$nextTick(function(){t.$refs.dataForm.clearValidate()})},updateData:function(){var e=this;this.$refs.dataForm.validate(function(t){t&&function(e){return Object(r.a)({url:"//update",method:"post",data:e})}(n()({},e.temp)).then(function(){e.getList(),e.dialogFormVisible=!1,e.$notify({title:"成功",message:"编辑成功",type:"success",duration:2e3})})})}}},f=i("KHd+"),h=Object(f.a)(m,function(){var e=this,t=e.$createElement,i=e._self._c||t;return i("div",{staticClass:"app-container"},[i("div",{staticClass:"filter-container"},[i("el-select",{staticClass:"filter-item",attrs:{placeholder:"请选择小区"},on:{change:e.handleFilter},model:{value:e.listQuery.smallDistrictId,callback:function(t){e.$set(e.listQuery,"smallDistrictId",t)},expression:"listQuery.smallDistrictId"}},e._l(e.smalllist,function(e){return i("el-option",{key:e.Name,attrs:{label:e.Name,value:e.Id}})})),e._v(" "),i("el-button",{directives:[{name:"waves",rawName:"v-waves"}],staticClass:"filter-item",attrs:{type:"primary",icon:"el-icon-search"},on:{click:e.handleFilter}},[e._v(e._s(e.$t("table.search")))]),e._v(" "),i("el-button",{staticClass:"filter-item",staticStyle:{"margin-left":"10px",width:"150px"},attrs:{type:"primary",icon:"el-icon-edit"},on:{click:e.handleCreate}},[e._v("业委会重组")])],1),e._v(" "),i("el-table",{directives:[{name:"loading",rawName:"v-loading",value:e.listLoading,expression:"listLoading"}],key:e.tableKey,staticStyle:{width:"100%"},attrs:{data:e.list,border:"",fit:"","highlight-current-row":""}},[i("el-table-column",{attrs:{label:"姓名",align:"center"},scopedSlots:e._u([{key:"default",fn:function(t){return[i("span",[e._v(e._s(t.row.Name))])]}}])}),e._v(" "),i("el-table-column",{attrs:{label:"申请理由",align:"center"},scopedSlots:e._u([{key:"default",fn:function(t){return[i("span",[e._v(e._s(t.row.Reason))])]}}])}),e._v(" "),i("el-table-column",{attrs:{label:"小区名称",align:"center"},scopedSlots:e._u([{key:"default",fn:function(t){return[i("span",[e._v(e._s(t.row.SmallDistrictName))])]}}])}),e._v(" "),i("el-table-column",{attrs:{label:"职能名称",align:"center"},scopedSlots:e._u([{key:"default",fn:function(t){return[i("span",[e._v(e._s(t.row.StructureName))])]}}])}),e._v(" "),i("el-table-column",{attrs:{label:"通过",align:"center"},scopedSlots:e._u([{key:"default",fn:function(t){return[i("span",[e._v(e._s(t.row.IsAdopt?"是":"否"))])]}}])}),e._v(" "),i("el-table-column",{attrs:{label:e.$t("table.actions"),align:"center","class-name":"small-padding fixed-width"},scopedSlots:e._u([{key:"default",fn:function(t){return[i("el-button",{directives:[{name:"show",rawName:"v-show",value:!t.row.IsAdopt,expression:"!scope.row.IsAdopt"}],attrs:{type:"danger",size:"mini"},on:{click:function(i){e.successBtn(t.row)}}},[e._v("通过")]),e._v(" "),i("el-button",{attrs:{size:"mini",type:"primary"},on:{click:function(i){e.voteDetail(t.row,t.row.Id)}}},[e._v("详情")])]}}])})],1),e._v(" "),i("pagination",{directives:[{name:"show",rawName:"v-show",value:e.total>0,expression:"total>0"}],attrs:{total:e.total,page:e.listQuery.pageIndex,limit:e.listQuery.pageSize},on:{"update:page":function(t){e.$set(e.listQuery,"pageIndex",t)},"update:limit":function(t){e.$set(e.listQuery,"pageSize",t)},pagination:e.getList}}),e._v(" "),i("el-dialog",{attrs:{title:e.textMap[e.dialogStatus],visible:e.dialogFormVisible},on:{"update:visible":function(t){e.dialogFormVisible=t}}},[i("el-form",{ref:"dataForm",staticStyle:{width:"100%","box-sizing":"border-box",padding:"0 50px"},attrs:{rules:e.rules,model:e.temp,"label-position":"left","label-width":"120px"}},[i("el-form-item",{attrs:{label:"内容",prop:"Summary"}},[i("el-input",{attrs:{disabled:""!=e.temp.Id&&null!=e.temp.Id},model:{value:e.temp.Summary,callback:function(t){e.$set(e.temp,"Summary",t)},expression:"temp.Summary"}})],1),e._v(" "),i("el-form-item",{attrs:{label:"竞选人数",prop:"electionNumber"}},[i("el-input",{attrs:{disabled:""!=e.temp.Id&&null!=e.temp.Id},model:{value:e.temp.electionNumber,callback:function(t){e.$set(e.temp,"electionNumber",t)},expression:"temp.electionNumber"}})],1),e._v(" "),i("el-form-item",{attrs:{label:"结束时间",prop:"Deadline"}},[i("el-date-picker",{staticClass:"filter-item",attrs:{type:"date",format:"yyyy-MM-dd","value-format":"yyyy-MM-dd",placeholder:"选择结束时间"},model:{value:e.temp.Deadline,callback:function(t){e.$set(e.temp,"Deadline",t)},expression:"temp.Deadline"}})],1),e._v(" "),i("el-form-item",{attrs:{label:"小区",prop:"SmallDistrictId"}},[i("el-select",{staticClass:"filter-item",attrs:{placeholder:"小区"},on:{change:e.changeSmall},model:{value:e.temp.SmallDistrictId,callback:function(t){e.$set(e.temp,"SmallDistrictId",t)},expression:"temp.SmallDistrictId"}},e._l(e.smalllist,function(t){return i("el-option",{key:t.Name,attrs:{label:t.Name,value:t.Id,disabled:""!=e.temp.Id&&null!=e.temp.Id}})}))],1),e._v(" "),i("el-form-item",{attrs:{label:"业委会",prop:"VipOwnerId"}},[i("el-select",{staticClass:"filter-item",attrs:{placeholder:"业委会"},model:{value:e.temp.VipOwnerId,callback:function(t){e.$set(e.temp,"VipOwnerId",t)},expression:"temp.VipOwnerId"}},e._l(e.ywhlist,function(t){return i("el-option",{key:t.Name,attrs:{label:t.Name,value:t.Id,disabled:""!=e.temp.Id&&null!=e.temp.Id}})}))],1),e._v(" "),i("div",{staticClass:"editor-container"},[e.dialogFormVisible?i("dropzone",{attrs:{id:"myVueDropzone",url:e.rootUrl+"/api/uploadVote"},on:{"dropzone-fileAdded":e.addfile,"dropzone-error":e.imgError,"dropzone-removedFile":e.dropzoneR,"dropzone-success":e.dropzoneS}}):e._e()],1)],1),e._v(" "),i("div",{staticClass:"dialog-footer",attrs:{slot:"footer"},slot:"footer"},[i("el-button",{on:{click:function(t){e.dialogFormVisible=!1}}},[e._v(e._s(e.$t("table.cancel")))]),e._v(" "),i("el-button",{directives:[{name:"loading",rawName:"v-loading",value:e.createLoading,expression:"createLoading"}],attrs:{type:"primary"},on:{click:function(t){"create"===e.dialogStatus?e.createData():e.updateData()}}},[e._v(e._s(e.$t("table.confirm")))])],1)],1),e._v(" "),i("el-dialog",{attrs:{visible:e.dialogPvVisible,title:"详情"},on:{"update:visible":function(t){e.dialogPvVisible=t}}},[i("h4",[e._v("姓名： "),i("small",[e._v(e._s(e.voteDetailCon.Name))])]),e._v(" "),i("h4",[e._v("申请理由："),i("small",[e._v(e._s(e.voteDetailCon.Reason))])]),e._v(" "),i("h4",[e._v("小区名称："),i("small",[e._v(e._s(e.voteDetailCon.SmallDistrictName))])]),e._v(" "),i("h4",[e._v("附件")]),e._v(" "),i("ul",e._l(e.voteDetailCon.AnnexModels,function(t,a){return i("li",{key:a},[i("div",{on:{click:function(i){e.$imgPreview(t.Url)}}},[e._v(e._s(t.CertificationConditionName))]),e._v(" "),i("div",[i("img",{staticStyle:{width:"100px"},attrs:{src:t.Url,alt:""},on:{click:function(i){e.$imgPreview(t.Url)}}})])])})),e._v(" "),i("span",{staticClass:"dialog-footer",attrs:{slot:"footer"},slot:"footer"},[i("el-button",{attrs:{type:"primary"},on:{click:function(t){e.dialogPvVisible=!1}}},[e._v(e._s(e.$t("table.confirm")))])],1)]),e._v(" "),i("el-dialog",{staticStyle:{padding:"0",margin:"0"},attrs:{visible:e.dialogImg,title:"图片预览"},on:{"update:visible":function(t){e.dialogImg=t}}},[i("div",{staticClass:"components-container",staticStyle:{padding:"0",margin:"0"}},[i("div",{staticClass:"editor-container",staticStyle:{padding:"0",margin:"0"}},[i("img-preview",{attrs:{nowimg:e.nowimg}})],1)])])],1)},[],!1,null,null,null);h.options.__file="index.vue";t.default=h.exports},ZySA:function(e,t,i){"use strict";var a=i("P2sY"),n=i.n(a),s=(i("jUE0"),{bind:function(e,t){e.addEventListener("click",function(i){var a=n()({},t.value),s=n()({ele:e,type:"hit",color:"rgba(0, 0, 0, 0.15)"},a),l=s.ele;if(l){l.style.position="relative",l.style.overflow="hidden";var o=l.getBoundingClientRect(),r=l.querySelector(".waves-ripple");switch(r?r.className="waves-ripple":((r=document.createElement("span")).className="waves-ripple",r.style.height=r.style.width=Math.max(o.width,o.height)+"px",l.appendChild(r)),s.type){case"center":r.style.top=o.height/2-r.offsetHeight/2+"px",r.style.left=o.width/2-r.offsetWidth/2+"px";break;default:r.style.top=(i.pageY-o.top-r.offsetHeight/2-document.documentElement.scrollTop||document.body.scrollTop)+"px",r.style.left=(i.pageX-o.left-r.offsetWidth/2-document.documentElement.scrollLeft||document.body.scrollLeft)+"px"}return r.style.backgroundColor=s.color,r.className="waves-ripple z-active",!1}},!1)}}),l=function(e){e.directive("waves",s)};window.Vue&&(window.waves=s,Vue.use(l)),s.install=l;t.a=s},jUE0:function(e,t,i){},qULk:function(e,t,i){}}]);