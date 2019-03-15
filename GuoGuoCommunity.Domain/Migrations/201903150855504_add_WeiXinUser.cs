namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_WeiXinUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Menus", "CreateOperationUserId", c => c.String());
            AddColumn("dbo.Menus", "CreateOperationTime", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.Menus", "LastOperationUserId", c => c.String());
            AddColumn("dbo.Menus", "LastOperationTime", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.Menus", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Menus", "DeletedTime", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.OwnerCertificationRecords", "CertificationStatusName", c => c.String());
            AddColumn("dbo.OwnerCertificationRecords", "CertificationStatusValue", c => c.String());
            AddColumn("dbo.Role_Menu", "CreateOperationUserId", c => c.String());
            AddColumn("dbo.Role_Menu", "CreateOperationTime", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.Role_Menu", "LastOperationUserId", c => c.String());
            AddColumn("dbo.Role_Menu", "LastOperationTime", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.Role_Menu", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Role_Menu", "DeletedTime", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.User_Role", "CreateOperationUserId", c => c.String());
            AddColumn("dbo.User_Role", "CreateOperationTime", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.User_Role", "LastOperationUserId", c => c.String());
            AddColumn("dbo.User_Role", "LastOperationTime", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.User_Role", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.User_Role", "DeletedTime", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.Users", "CreateOperationUserId", c => c.String());
            AddColumn("dbo.Users", "CreateOperationTime", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.Users", "LastOperationUserId", c => c.String());
            AddColumn("dbo.Users", "LastOperationTime", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.Users", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "DeletedTime", c => c.DateTimeOffset(precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "DeletedTime");
            DropColumn("dbo.Users", "IsDeleted");
            DropColumn("dbo.Users", "LastOperationTime");
            DropColumn("dbo.Users", "LastOperationUserId");
            DropColumn("dbo.Users", "CreateOperationTime");
            DropColumn("dbo.Users", "CreateOperationUserId");
            DropColumn("dbo.User_Role", "DeletedTime");
            DropColumn("dbo.User_Role", "IsDeleted");
            DropColumn("dbo.User_Role", "LastOperationTime");
            DropColumn("dbo.User_Role", "LastOperationUserId");
            DropColumn("dbo.User_Role", "CreateOperationTime");
            DropColumn("dbo.User_Role", "CreateOperationUserId");
            DropColumn("dbo.Role_Menu", "DeletedTime");
            DropColumn("dbo.Role_Menu", "IsDeleted");
            DropColumn("dbo.Role_Menu", "LastOperationTime");
            DropColumn("dbo.Role_Menu", "LastOperationUserId");
            DropColumn("dbo.Role_Menu", "CreateOperationTime");
            DropColumn("dbo.Role_Menu", "CreateOperationUserId");
            DropColumn("dbo.OwnerCertificationRecords", "CertificationStatusValue");
            DropColumn("dbo.OwnerCertificationRecords", "CertificationStatusName");
            DropColumn("dbo.Menus", "DeletedTime");
            DropColumn("dbo.Menus", "IsDeleted");
            DropColumn("dbo.Menus", "LastOperationTime");
            DropColumn("dbo.Menus", "LastOperationUserId");
            DropColumn("dbo.Menus", "CreateOperationTime");
            DropColumn("dbo.Menus", "CreateOperationUserId");
        }
    }
}
