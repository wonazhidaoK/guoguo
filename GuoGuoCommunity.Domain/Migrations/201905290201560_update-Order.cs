namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Postage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Orders", "PaymentPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "PaymentPrice");
            DropColumn("dbo.Orders", "Postage");
        }
    }
}
