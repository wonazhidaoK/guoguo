namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class weiXinUser_owner : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OwnerCertificationRecords",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        UserId = c.String(),
                        OwnerId = c.String(),
                        CertificationTime = c.String(),
                        CertificationResult = c.String(),
                        IsValid = c.String(),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WeiXinUsers",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Subscribe_scene = c.String(),
                        Tagid_list = c.String(),
                        Groupid = c.String(),
                        Remark = c.String(),
                        Unionid = c.String(),
                        Subscribe_time = c.String(),
                        Headimgurl = c.String(),
                        Country = c.String(),
                        Province = c.String(),
                        City = c.String(),
                        Language = c.String(),
                        Sex = c.Int(nullable: false),
                        Nickname = c.String(),
                        Openid = c.String(),
                        Subscribe = c.Int(nullable: false),
                        Qr_scene = c.Int(nullable: false),
                        Qr_scene_str = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Users", "Openid", c => c.String());
            AddColumn("dbo.Users", "Unionid", c => c.String());
            AddColumn("dbo.Users", "IsOwner", c => c.String());
            AddColumn("dbo.Users", "IsVipOwner", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "IsVipOwner");
            DropColumn("dbo.Users", "IsOwner");
            DropColumn("dbo.Users", "Unionid");
            DropColumn("dbo.Users", "Openid");
            DropTable("dbo.WeiXinUsers");
            DropTable("dbo.OwnerCertificationRecords");
        }
    }
}
