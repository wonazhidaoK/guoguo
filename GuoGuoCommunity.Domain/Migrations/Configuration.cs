using GuoGuoCommunity.Domain.Models;
using GuoGuoCommunity.Domain.Models.Enum;
using System;
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
            try
            {
                //This method will be called after migrating to the latest version.

                //  You can use the DbSet<T>.AddOrUpdate() helper extension method
                //  to avoid creating duplicate seed data.

                context.Tests.AddOrUpdate(x => x.Name,
                    new Test() { Id = 1, Name = "Jane Austen" },
                    new Test() { Id = 2, Name = "Charles Dickens" }
                    );
                context.Menus.AddOrUpdate(x => x.Name,
                    new Menu() { Name = "物业后台-业户信息", Key = "wy_house_info", DepartmentValue = Department.WuYe.Value, DepartmentName = Department.WuYe.Name },
                    //new Menu() { Name = "物业后台-认证中心", Key = "wy_rz" },
                    //new Menu() { Name = "物业后台-业主认证", Key = "wy_rz_yz" },
                    new Menu() { Name = "物业后台-公告", Key = "wy_notice", DepartmentValue = Department.WuYe.Value, DepartmentName = Department.WuYe.Name },
                    new Menu() { Name = "物业后台-投票", Key = "wy_vote", DepartmentValue = Department.WuYe.Value, DepartmentName = Department.WuYe.Name },
                    new Menu() { Name = "物业后台-投诉", Key = "wy_complaint", DepartmentValue = Department.WuYe.Value, DepartmentName = Department.WuYe.Name },
                    new Menu() { Name = "物业后台-站内信", Key = "wy_letter", DepartmentValue = Department.WuYe.Value, DepartmentName = Department.WuYe.Name },
                    new Menu() { Name = "街道办后台-公告", Key = "jdb_notice", DepartmentValue = Department.JieDaoBan.Value, DepartmentName = Department.JieDaoBan.Name },
                    new Menu() { Name = "街道办-高级认证", Key = "jdb_rz_gj", DepartmentValue = Department.JieDaoBan.Value, DepartmentName = Department.JieDaoBan.Name },
                    new Menu() { Name = "街道办后台-投票", Key = "jdb_vote", DepartmentValue = Department.JieDaoBan.Value, DepartmentName = Department.JieDaoBan.Name },
                    new Menu() { Name = "街道办后台-投诉", Key = "jdb_complaint", DepartmentValue = Department.JieDaoBan.Value, DepartmentName = Department.JieDaoBan.Name },
                    new Menu() { Name = "街道办后台-站内信", Key = "jdb_letter", DepartmentValue = Department.JieDaoBan.Value, DepartmentName = Department.JieDaoBan.Name },
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
                        Region = "道里区",
                        Account = "admin"
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
                context.ComplaintTypes.AddOrUpdate(x =>new { x.Name ,x.InitiatingDepartmentValue},
                    new ComplaintType()
                    {
                        Name = "房屋质量问题",
                        ComplaintPeriod = 15,
                        Description = "渗水、开裂、下沉、楼板厚度不够、铝合金门窗歪斜、地面不平整、安装施工质量差",
                        InitiatingDepartmentName = "业主",
                        InitiatingDepartmentValue = "Owner",
                        Level = "8",
                        ProcessingPeriod = 7,
                        // Id = Guid.Parse("11e99bb9-505b-e911-80e8-9e79ab1b7f85")
                    }, new ComplaintType()
                    {
                        Name = "公共空间、公共场地问题",
                        ComplaintPeriod = 15,
                        Description = "防栓向安装影响业主通行、楼道过于狭窄、残章通道不通、地下室至楼顶的步梯设计不合理、下水道堵塞",
                        InitiatingDepartmentName = "业主",
                        InitiatingDepartmentValue = "Owner",
                        Level = "7",
                        ProcessingPeriod = 7,
                        //Id = Guid.Parse("21e99bb9-505b-e911-80e8-9e79ab1b7f85")
                    }, new ComplaintType()
                    {
                        Name = "设备设施、共用设施问题",
                        ComplaintPeriod = 15,
                        Description = "电梯沉降严重、电梯、困人、停车收费道闸安装在斜坡上、人行道狭窄、行车道好人行道不分开、小区外围开放式",
                        InitiatingDepartmentName = "业主",
                        InitiatingDepartmentValue = "Owner",
                        Level = "6",
                        ProcessingPeriod = 7,
                        //Id = Guid.Parse("31e99bb9-505b-e911-80e8-9e79ab1b7f85")
                    }, new ComplaintType()
                    {
                        Name = "景观设计、绿化问题",
                        ComplaintPeriod = 15,
                        Description = "红土裸露、植物长势较差、地面排水不畅、绿地无围挡导致后期管理难度增加、景观水池无提示标志、水池深度超过1.8米、景观树种影响人行道、乔木绿篱的选择不耐寒",
                        InitiatingDepartmentName = "业主",
                        InitiatingDepartmentValue = "Owner",
                        Level = "5",
                        ProcessingPeriod = 7,
                        // Id = Guid.Parse("41e99bb9-505b-e911-80e8-9e79ab1b7f85")
                    }, new ComplaintType()
                    {
                        Name = "小区四周的噪音污染",
                        ComplaintPeriod = 15,
                        Description = "如施工、装修、渣土清运、业主唱歌、狗患、宠物声响",
                        InitiatingDepartmentName = "业主",
                        InitiatingDepartmentValue = "Owner",
                        Level = "4",
                        ProcessingPeriod = 7,
                        //Id = Guid.Parse("51e99bb9-505b-e911-80e8-9e79ab1b7f85")
                    }, new ComplaintType()
                    {
                        Name = "物业管理费及其他费用问题",
                        ComplaintPeriod = 15,
                        Description = "认为费用和享受不对等、电梯费和二次加压费过高、热水费过高、装修各类费用高",
                        InitiatingDepartmentName = "业主委员会",
                        InitiatingDepartmentValue = "VipOwner",
                        Level = "3",
                        ProcessingPeriod = 7,
                        // Id = Guid.Parse("61e99bb9-505b-e911-80e8-9e79ab1b7f85")
                    }, new ComplaintType()
                    {
                        Name = "物业服务不到位",
                        ComplaintPeriod = 15,
                        Description = "礼貌礼节缺乏、不耐心倾听、打断业主说话、态度恶劣、着装不整、骂人、打骂业主、岗位睡觉、岗位上吹牛聊天、不作为、推诿扯皮",
                        InitiatingDepartmentName = "业主委员会",
                        InitiatingDepartmentValue = "VipOwner",
                        Level = "2",
                        ProcessingPeriod = 7,
                        // Id = Guid.Parse("71e99bb9-505b-e911-80e8-9e79ab1b7f85")
                    }, new ComplaintType()
                    {
                        Name = "物业管理不透明",
                        ComplaintPeriod = 15,
                        Description = "费用、流程、标准、承诺不公开，经营糊涂账，开支不明示，暗箱操作",
                        InitiatingDepartmentName = "业主委员会",
                        InitiatingDepartmentValue = "VipOwner",
                        Level = "1",
                        ProcessingPeriod = 7,
                        // Id = Guid.Parse("81e99bb9-505b-e911-80e8-9e79ab1b7f85")
                    });
            }
            catch (Exception e)
            {
                throw new NotImplementedException(e.Message);
            }
        }
    }
}
