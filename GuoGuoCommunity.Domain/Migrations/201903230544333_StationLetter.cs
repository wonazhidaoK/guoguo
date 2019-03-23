namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StationLetter : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StationLetterAnnexes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        StationLetterId = c.String(),
                        AnnexContent = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StationLetterBrowseRecords",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        StationLetterId = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StationLetters",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Title = c.String(),
                        Summary = c.String(),
                        Content = c.String(),
                        SmallDistrictArray = c.String(),
                        DepartmentName = c.String(),
                        DepartmentValue = c.String(),
                        StreetOfficeId = c.String(),
                        StreetOfficeName = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StationLetters");
            DropTable("dbo.StationLetterBrowseRecords");
            DropTable("dbo.StationLetterAnnexes");
        }
    }
}
