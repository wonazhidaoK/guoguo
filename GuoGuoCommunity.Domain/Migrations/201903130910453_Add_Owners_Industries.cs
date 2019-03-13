namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Owners_Industries : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Industries",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        NumberOfLayers = c.String(),
                        Acreage = c.String(),
                        Oriented = c.String(),
                        BuildingId = c.String(nullable: false),
                        BuildingName = c.String(),
                        BuildingUnitId = c.String(),
                        BuildingUnitName = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Owners",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        Birthday = c.String(),
                        Gender = c.String(),
                        PhoneNumber = c.String(),
                        IDCard = c.String(),
                        IndustryId = c.String(),
                        IndustryName = c.String(),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Owners");
            DropTable("dbo.Industries");
        }
    }
}
