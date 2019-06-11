namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateOrderItem : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.OrdeItems", newName: "OrderItems");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.OrderItems", newName: "OrdeItems");
        }
    }
}
