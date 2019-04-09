using GuoGuoCommunity.Domain.Models;
using System.Data.Entity.Migrations;
namespace GuoGuoCommunity.Domain.Migrations
{

    internal sealed class Configuration : DbMigrationsConfiguration<GuoGuoCommunityContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(GuoGuoCommunityContext context)
        {
            //This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            context.Tests.AddOrUpdate(x => x.Name,
                new Test() { Id = 1, Name = "Jane Austen" },
                new Test() { Id = 2, Name = "Charles Dickens" }
                );
            context.Menus.AddOrUpdate(x => x.Name,
                new Menu() { Name = "物业后台-业户信息", Key = "wy_house_info" },
                //new Menu() { Name = "物业后台-认证中心", Key = "wy_rz" },
                //new Menu() { Name = "物业后台-业主认证", Key = "wy_rz_yz" },
                new Menu() { Name = "物业后台-公告", Key = "wy_notice" },
                
                new Menu() { Name = "物业后台-投票", Key = "wy_vote" },
                new Menu() { Name = "物业后台-投诉", Key = "wy_complaint" },
                new Menu() { Name = "物业后台-站内信", Key = "wy_letter" },
                new Menu() { Name = "街道办后台-公告", Key = "jdb_notice" },
                new Menu() { Name = "街道办-高级认证", Key = "jdb_rz_gj" },
                new Menu() { Name = "街道办后台-投票", Key = "jdb_vote" },
                new Menu() { Name = "街道办后台-投诉", Key = "jdb_complaint" },
                new Menu() { Name = "街道办后台-站内信", Key = "jdb_letter" },
                new Menu() { Name = "系统管理员", Key = "authorityMax" }
                );
            context.Users.AddOrUpdate(x => x.Name,
                new User()
                {
                    Name = "admin",
                    Password = "123456",
                    PhoneNumber = "13888888888",
                    State = "黑龙江",
                    City = "哈尔滨",
                    Region = "道里区"
                });
            context.VipOwnerCertificationConditions.AddOrUpdate(x => x.Title,
                new VipOwnerCertificationCondition()
                {
                    Title = "无犯罪证明",
                    Description = "辖区内派出所出具无犯罪证明",
                    TypeName = "图片",
                    TypeValue = "Image",
                },
                new VipOwnerCertificationCondition()
                {
                    Title = "单位介绍信",
                    Description = "业主提供现所在单位介绍信",
                    TypeName = "图片",
                    TypeValue = "Image",
                },
                new VipOwnerCertificationCondition()
                {
                    Title = "物业缴费票据",
                    Description = "提供业主三年的物业费缴费票据",
                    TypeName = "图片",
                    TypeValue = "Image",
                });
        }
    }
}
