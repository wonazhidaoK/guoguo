using GuoGuoCommunity.Domain.Models;
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
    }
}
