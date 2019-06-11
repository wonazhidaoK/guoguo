namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSmallDistrictShop : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SmallDistrictShops",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        SmallDistrictId = c.Guid(nullable: false),
                        ShopId = c.Guid(nullable: false),
                        Sort = c.Int(nullable: false),
                        Postage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shops", t => t.ShopId, cascadeDelete: true)
                .ForeignKey("dbo.SmallDistricts", t => t.SmallDistrictId, cascadeDelete: true)
                .Index(t => t.SmallDistrictId)
                .Index(t => t.ShopId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SmallDistrictShops", "SmallDistrictId", "dbo.SmallDistricts");
            DropForeignKey("dbo.SmallDistrictShops", "ShopId", "dbo.Shops");
            DropIndex("dbo.SmallDistrictShops", new[] { "ShopId" });
            DropIndex("dbo.SmallDistrictShops", new[] { "SmallDistrictId" });
            DropTable("dbo.SmallDistrictShops");
        }
    }
}
