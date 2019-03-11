namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class streetOffice : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StreetOffices",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        State = c.String(nullable: false),
                        City = c.String(nullable: false),
                        Region = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(nullable: false, precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StreetOffices");
        }
    }
}
