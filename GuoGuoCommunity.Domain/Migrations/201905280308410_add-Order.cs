namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addOrder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        OrderStatusName = c.String(),
                        OrderStatusValue = c.String(),
                        DeliveryPhone = c.String(),
                        DeliveryName = c.String(),
                        Number = c.String(),
                        ShopId = c.Guid(nullable: false),
                        ShopCommodityCount = c.Int(nullable: false),
                        ShopCommodityPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentStatusName = c.String(),
                        PaymentStatusValue = c.String(),
                        PaymentTypeName = c.String(),
                        PaymentTypeValue = c.String(),
                        OwnerCertificationRecordId = c.Guid(nullable: false),
                        ReceiverName = c.String(),
                        ReceiverPhone = c.String(),
                        Address = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OwnerCertificationRecords", t => t.OwnerCertificationRecordId, cascadeDelete: true)
                .ForeignKey("dbo.Shops", t => t.ShopId, cascadeDelete: true)
                .Index(t => t.ShopId)
                .Index(t => t.OwnerCertificationRecordId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.Orders", "OwnerCertificationRecordId", "dbo.OwnerCertificationRecords");
            DropIndex("dbo.Orders", new[] { "OwnerCertificationRecordId" });
            DropIndex("dbo.Orders", new[] { "ShopId" });
            DropTable("dbo.Orders");
        }
    }
}
