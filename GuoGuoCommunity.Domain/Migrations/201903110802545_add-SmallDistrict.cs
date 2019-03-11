namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSmallDistrict : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SmallDistricts",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        State = c.String(nullable: false),
                        City = c.String(nullable: false),
                        Region = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        StreetOfficeId = c.String(nullable: false),
                        StreetOfficeName = c.String(nullable: false),
                        CommunityId = c.String(nullable: false),
                        CommunityName = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SmallDistricts");
        }
    }
}
