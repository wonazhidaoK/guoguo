namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateShopCommodity : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ShopCommodities", "DiscountPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ShopCommodities", "DiscountPrice", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
