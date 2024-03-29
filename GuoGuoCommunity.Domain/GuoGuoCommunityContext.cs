﻿using GuoGuoCommunity.Domain.Models;
using GuoGuoCommunity.Domain.Models.Store;
using System.Data.Entity;

namespace GuoGuoCommunity.Domain
{
    public class GuoGuoCommunityContext : DbContext
    {
        public GuoGuoCommunityContext() : base("name=GuoGuoCommunityContext")
        {
            Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        public DbSet<Test> Tests { get; set; }

        /// <summary>
        /// 角色菜单
        /// </summary>
        public DbSet<Role_Menu> Role_Menus { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public DbSet<User_Role> User_Roles { get; set; }

        /// <summary>
        /// 菜单
        /// </summary>
        public DbSet<Menu> Menus { get; set; }

        /// <summary>
        /// 街道办
        /// </summary>
        public DbSet<StreetOffice> StreetOffices { get; set; }

        /// <summary>
        /// 社区
        /// </summary>
        public DbSet<Community> Communities { get; set; }

        /// <summary>
        /// 小区
        /// </summary>
        public DbSet<SmallDistrict> SmallDistricts { get; set; }

        /// <summary>
        /// 业委会架构
        /// </summary>
        public DbSet<VipOwnerStructure> VipOwnerStructures { get; set; }

        /// <summary>
        /// 业委会
        /// </summary>
        public DbSet<VipOwner> VipOwners { get; set; }

        /// <summary>
        /// 投诉类型
        /// </summary>
        public DbSet<ComplaintType> ComplaintTypes { get; set; }

        /// <summary>
        /// 楼宇单元信息
        /// </summary>
        public DbSet<BuildingUnit> BuildingUnits { get; set; }

        /// <summary>
        /// 楼宇信息
        /// </summary>
        public DbSet<Building> Buildings { get; set; }

        /// <summary>
        /// 业户信息
        /// </summary>
        public DbSet<Industry> Industries { get; set; }

        /// <summary>
        /// 业主信息
        /// </summary>
        public DbSet<Owner> Owners { get; set; }

        /// <summary>
        /// 业主认证记录
        /// </summary>
        public DbSet<OwnerCertificationRecord> OwnerCertificationRecords { get; set; }

        /// <summary>
        /// 微信用户表
        /// </summary>
        public DbSet<WeiXinUser> WeiXinUsers { get; set; }

        /// <summary>
        /// 上传表
        /// </summary>
        public DbSet<Upload> Uploads { get; set; }

        /// <summary>
        /// 业委会成员申请表
        /// </summary>
        public DbSet<VipOwnerApplicationRecord> VipOwnerApplicationRecords { get; set; }

        /// <summary>
        /// 业委会认证附件表
        /// </summary>
        public DbSet<VipOwnerCertificationAnnex> VipOwnerCertificationAnnices { get; set; }

        /// <summary>
        /// 高级认证申请条件
        /// </summary>
        public DbSet<VipOwnerCertificationCondition> VipOwnerCertificationConditions { get; set; }

        /// <summary>
        /// 业委会成员认证记录
        /// </summary>
        public DbSet<VipOwnerCertificationRecord> VipOwnerCertificationRecords { get; set; }

        /// <summary>
        /// 业主认证附件表
        /// </summary>
        public DbSet<OwnerCertificationAnnex> OwnerCertificationAnnices { get; set; }

        /// <summary>
        /// 公告表
        /// </summary>
        public DbSet<Announcement> Announcements { get; set; }

        /// <summary>
        /// 公告附件表
        /// </summary>
        public DbSet<AnnouncementAnnex> AnnouncementAnnices { get; set; }

        /// <summary>
        /// 站内信
        /// </summary>
        public DbSet<StationLetter> StationLetters { get; set; }

        /// <summary>
        /// 站内信附件
        /// </summary>
        public DbSet<StationLetterAnnex> StationLetterAnnices { get; set; }

        /// <summary>
        /// 站内信浏览记录
        /// </summary>
        public DbSet<StationLetterBrowseRecord> StationLetterBrowseRecords { get; set; }

        /// <summary>
        /// 投票附件
        /// </summary>
        public DbSet<VoteAnnex> VoteAnnices { get; set; }

        /// <summary>
        /// 投票问题选项
        /// </summary>
        public DbSet<VoteQuestionOption> VoteQuestionOptions { get; set; }

        /// <summary>
        /// 投票问题
        /// </summary>
        public DbSet<VoteQuestion> VoteQuestions { get; set; }

        /// <summary>
        /// 投票记录详情
        /// </summary>
        public DbSet<VoteRecordDetail> VoteRecordDetails { get; set; }

        /// <summary>
        /// 投票记录
        /// </summary>
        public DbSet<VoteRecord> VoteRecords { get; set; }

        /// <summary>
        /// 投票管理
        /// </summary>
        public DbSet<Vote> Votes { get; set; }

        /// <summary>
        /// 投票结果记录
        /// </summary>
        public DbSet<VoteResultRecord> VoteResultRecords { get; set; }

        /// <summary>
        /// 投票业委会关联表
        /// </summary>
        public DbSet<VoteAssociationVipOwner> VoteAssociationVipOwners { get; set; }

        /// <summary>
        /// 投诉
        /// </summary>
        public DbSet<Complaint> Complaints { get; set; }

        /// <summary>
        /// 投诉跟进
        /// </summary>
        public DbSet<ComplaintFollowUp> ComplaintFollowUps { get; set; }

        /// <summary>
        /// 投诉附件
        /// </summary>
        public DbSet<ComplaintAnnex> ComplaintAnnices { get; set; }

        /// <summary>
        /// 投诉状态变更记录
        /// </summary>
        public DbSet<ComplaintStatusChangeRecording> StatusChangeRecordings { get; set; }

        /// <summary>
        /// 身份证照片读取记录
        /// </summary>
        public DbSet<IDCardPhotoRecord> IDCardPhotoRecords { get; set; }

        /// <summary>
        /// 物业公司信息
        /// </summary>
        public DbSet<PropertyCompany> PropertyCompanies { get; set; }

        #region 商超

        /// <summary>
        /// 平台商品
        /// </summary>
        public DbSet<PlatformCommodity> PlatformCommodities { get; set; }

        /// <summary>
        /// 店铺商品类别
        /// </summary>
        public DbSet<GoodsType> GoodsTypes { get; set; }

        /// <summary>
        /// 店铺商品
        /// </summary>
        public DbSet<ShopCommodity> ShopCommodities { get; set; }

        /// <summary>
        /// 店铺信息
        /// </summary>
        public DbSet<Shop> Shops { get; set; }

        /// <summary>
        /// 商家购物车
        /// </summary>
        public DbSet<ShoppingTrolley> ShoppingTrolleys { get; set; }

        /// <summary>
        /// 小区关联商户
        /// </summary>
        public DbSet<SmallDistrictShop>  SmallDistrictShops { get; set; }

        /// <summary>
        /// 商超用户地址管理
        /// </summary>
        public DbSet<ShopUserAddress> ShopUserAddresses { get; set; }

        /// <summary>
        /// 订单
        /// </summary>
        public DbSet<Order>  Orders { get; set; }

        /// <summary>
        /// 订单项
        /// </summary>
        public DbSet<OrderItem>  OrdeItems { get; set; }
        /// <summary>
        /// 活动
        /// </summary>
        public DbSet<Activity> Activities { get; set; }

        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            /*
             * ef code first 建外键引发异常 在这里编辑约束特性
             * 特定情况下的表设计才会引发的异常
             * 参考文章相见https://blog.csdn.net/anjingshen/article/details/73522353
             */
            var shopUserAddress = modelBuilder.Entity<ShopUserAddress>();
            shopUserAddress
                .HasRequired(t => t.Industry).WithMany().WillCascadeOnDelete(false);
            var ordeItem = modelBuilder.Entity<OrderItem>();
            ordeItem.HasRequired(t => t.ShopCommodity).WithMany().WillCascadeOnDelete(false);
            var order = modelBuilder.Entity<Order>();
            order.HasRequired(t => t.SmallDistrictShop).WithMany().WillCascadeOnDelete(false);
            base.OnModelCreating(modelBuilder);
        }
    }
}
