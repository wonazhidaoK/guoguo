namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addShopCommodityShoppingTrolleysPropertyCompany : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PropertyCompanies",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        Phone = c.String(),
                        Address = c.String(),
                        Description = c.String(),
                        LogoImageUrl = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ShopCommodities",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        TypeId = c.Guid(nullable: false),
                        BarCode = c.String(),
                        Name = c.String(),
                        ImageUrl = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountPrice = c.Decimal(precision: 18, scale: 2),
                        Description = c.String(),
                        CommodityStocks = c.Int(nullable: false),
                        Sort = c.Int(nullable: false),
                        SalesTypeName = c.String(),
                        SalesTypeValue = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GoodsTypes", t => t.TypeId, cascadeDelete: true)
                .Index(t => t.TypeId);
            
            CreateTable(
                "dbo.ShoppingTrolleys",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ShopCommodityId = c.Guid(nullable: false),
                        OwnerCertificationRecordId = c.Guid(nullable: false),
                        CommodityCount = c.Int(nullable: false),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OwnerCertificationRecords", t => t.OwnerCertificationRecordId, cascadeDelete: true)
                .ForeignKey("dbo.ShopCommodities", t => t.ShopCommodityId, cascadeDelete: true)
                .Index(t => t.ShopCommodityId)
                .Index(t => t.OwnerCertificationRecordId);
            
            AddColumn("dbo.SmallDistricts", "PropertyCompanyId", c => c.Guid());
            CreateIndex("dbo.SmallDistricts", "PropertyCompanyId");
            AddForeignKey("dbo.SmallDistricts", "PropertyCompanyId", "dbo.PropertyCompanies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShoppingTrolleys", "ShopCommodityId", "dbo.ShopCommodities");
            DropForeignKey("dbo.ShoppingTrolleys", "OwnerCertificationRecordId", "dbo.OwnerCertificationRecords");
            DropForeignKey("dbo.ShopCommodities", "TypeId", "dbo.GoodsTypes");
            DropForeignKey("dbo.SmallDistricts", "PropertyCompanyId", "dbo.PropertyCompanies");
            DropIndex("dbo.ShoppingTrolleys", new[] { "OwnerCertificationRecordId" });
            DropIndex("dbo.ShoppingTrolleys", new[] { "ShopCommodityId" });
            DropIndex("dbo.ShopCommodities", new[] { "TypeId" });
            DropIndex("dbo.SmallDistricts", new[] { "PropertyCompanyId" });
            DropColumn("dbo.SmallDistricts", "PropertyCompanyId");
            DropTable("dbo.ShoppingTrolleys");
            DropTable("dbo.ShopCommodities");
            DropTable("dbo.PropertyCompanies");
        }
    }
}
