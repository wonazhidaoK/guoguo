namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateOrderuser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "OwnerCertificationRecordId", "dbo.OwnerCertificationRecords");
            DropIndex("dbo.Orders", new[] { "OwnerCertificationRecordId" });
            AddColumn("dbo.Orders", "IndustryId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Orders", "CreateOperationUserId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Orders", "IndustryId");
            CreateIndex("dbo.Orders", "CreateOperationUserId");
            AddForeignKey("dbo.Orders", "IndustryId", "dbo.Industries", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Orders", "CreateOperationUserId", "dbo.Users", "Id", cascadeDelete: true);
            DropColumn("dbo.Orders", "OwnerCertificationRecordId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "OwnerCertificationRecordId", c => c.Guid(nullable: false));
            DropForeignKey("dbo.Orders", "CreateOperationUserId", "dbo.Users");
            DropForeignKey("dbo.Orders", "IndustryId", "dbo.Industries");
            DropIndex("dbo.Orders", new[] { "CreateOperationUserId" });
            DropIndex("dbo.Orders", new[] { "IndustryId" });
            AlterColumn("dbo.Orders", "CreateOperationUserId", c => c.String());
            DropColumn("dbo.Orders", "IndustryId");
            CreateIndex("dbo.Orders", "OwnerCertificationRecordId");
            AddForeignKey("dbo.Orders", "OwnerCertificationRecordId", "dbo.OwnerCertificationRecords", "Id", cascadeDelete: true);
        }
    }
}
