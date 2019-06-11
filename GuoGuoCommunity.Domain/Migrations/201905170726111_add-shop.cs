namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addshop : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Shops",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        Name = c.String(),
                        Address = c.String(),
                        MerchantCategoryName = c.String(),
                        MerchantCategoryValue = c.String(),
                        Description = c.String(),
                        QualificationImageUrl = c.String(),
                        LogoImageUrl = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Shops", "UserId", "dbo.Users");
            DropIndex("dbo.Shops", new[] { "UserId" });
            DropTable("dbo.Shops");
        }
    }
}
