namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addShopUserAddresses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShopUserAddresses",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ApplicationRecordId = c.Guid(nullable: false),
                        ReceiverName = c.String(),
                        ReceiverPhone = c.String(),
                        IndustryId = c.Guid(nullable: false),
                        IsDefault = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Industries", t => t.IndustryId)
                .ForeignKey("dbo.OwnerCertificationRecords", t => t.ApplicationRecordId, cascadeDelete: true)
                .Index(t => t.ApplicationRecordId)
                .Index(t => t.IndustryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShopUserAddresses", "ApplicationRecordId", "dbo.OwnerCertificationRecords");
            DropForeignKey("dbo.ShopUserAddresses", "IndustryId", "dbo.Industries");
            DropIndex("dbo.ShopUserAddresses", new[] { "IndustryId" });
            DropIndex("dbo.ShopUserAddresses", new[] { "ApplicationRecordId" });
            DropTable("dbo.ShopUserAddresses");
        }
    }
}
