namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPlatformCommodities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlatformCommodities",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        BarCode = c.String(),
                        Name = c.String(),
                        ImageUrl = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PlatformCommodities");
        }
    }
}
