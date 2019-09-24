namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_ShopUserAddresses_User : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ShopUserAddresses", "ApplicationRecordId", "dbo.OwnerCertificationRecords");
            DropIndex("dbo.ShopUserAddresses", new[] { "ApplicationRecordId" });
            AlterColumn("dbo.ShopUserAddresses", "CreateOperationUserId", c => c.Guid(nullable: false));
            CreateIndex("dbo.ShopUserAddresses", "CreateOperationUserId");
            AddForeignKey("dbo.ShopUserAddresses", "CreateOperationUserId", "dbo.Users", "Id", cascadeDelete: true);
            DropColumn("dbo.ShopUserAddresses", "ApplicationRecordId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ShopUserAddresses", "ApplicationRecordId", c => c.Guid(nullable: false));
            DropForeignKey("dbo.ShopUserAddresses", "CreateOperationUserId", "dbo.Users");
            DropIndex("dbo.ShopUserAddresses", new[] { "CreateOperationUserId" });
            AlterColumn("dbo.ShopUserAddresses", "CreateOperationUserId", c => c.String());
            CreateIndex("dbo.ShopUserAddresses", "ApplicationRecordId");
            AddForeignKey("dbo.ShopUserAddresses", "ApplicationRecordId", "dbo.OwnerCertificationRecords", "Id", cascadeDelete: true);
        }
    }
}
