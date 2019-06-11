namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateShopPrinterName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shops", "PrinterName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shops", "PrinterName");
        }
    }
}
