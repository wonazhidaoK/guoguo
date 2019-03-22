namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Announcement : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnnouncementAnnexes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        AnnouncementId = c.String(),
                        AnnexContent = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Announcements",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Title = c.String(),
                        Summary = c.String(),
                        Content = c.String(),
                        SmallDistrictArray = c.String(),
                        DepartmentName = c.String(),
                        DepartmentValue = c.String(),
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
            DropTable("dbo.Announcements");
            DropTable("dbo.AnnouncementAnnexes");
        }
    }
}
