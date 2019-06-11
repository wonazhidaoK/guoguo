namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addOrdeItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrdeItems",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        OrderId = c.Guid(nullable: false),
                        ShopCommodityId = c.Guid(nullable: false),
                        Name = c.String(),
                        ImageUrl = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountPrice = c.Decimal(precision: 18, scale: 2),
                        CommodityCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.ShopCommodities", t => t.ShopCommodityId)
                .Index(t => t.OrderId)
                .Index(t => t.ShopCommodityId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrdeItems", "ShopCommodityId", "dbo.ShopCommodities");
            DropForeignKey("dbo.OrdeItems", "OrderId", "dbo.Orders");
            DropIndex("dbo.OrdeItems", new[] { "ShopCommodityId" });
            DropIndex("dbo.OrdeItems", new[] { "OrderId" });
            DropTable("dbo.OrdeItems");
        }
    }
}
