namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderaddSmallDistrictShopId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "SmallDistrictShopId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Orders", "SmallDistrictShopId");
            AddForeignKey("dbo.Orders", "SmallDistrictShopId", "dbo.SmallDistrictShops", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "SmallDistrictShopId", "dbo.SmallDistrictShops");
            DropIndex("dbo.Orders", new[] { "SmallDistrictShopId" });
            DropColumn("dbo.Orders", "SmallDistrictShopId");
        }
    }
}
