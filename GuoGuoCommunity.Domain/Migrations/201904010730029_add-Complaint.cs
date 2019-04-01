namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addComplaint : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ComplaintAnnexes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ComplaintId = c.String(),
                        ComplaintFollowUpId = c.String(),
                        AnnexContent = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ComplaintFollowUps",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ComplaintId = c.String(),
                        Description = c.String(),
                        OperationDepartmentName = c.String(),
                        OperationDepartmentValue = c.String(),
                        Aappeal = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Complaints",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Description = c.String(),
                        ComplaintTypeId = c.String(),
                        DepartmentName = c.String(),
                        DepartmentValue = c.String(),
                        OwnerCertificationId = c.String(),
                        ClosedTime = c.DateTimeOffset(precision: 7),
                        ExpiredTime = c.DateTimeOffset(precision: 7),
                        ProcessUpTime = c.DateTimeOffset(precision: 7),
                        IsInvalid = c.String(),
                        StatusName = c.String(),
                        StatusValue = c.String(),
                        StreetOfficeId = c.String(),
                        StreetOfficeName = c.String(),
                        CommunityId = c.String(),
                        CommunityName = c.String(),
                        SmallDistrictId = c.String(),
                        SmallDistrictName = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ComplaintStatusChangeRecordings",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        OldStatus = c.String(),
                        NewStatus = c.String(),
                        ComplaintFollowUpId = c.String(),
                        ComplaintId = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ComplaintStatusChangeRecordings");
            DropTable("dbo.Complaints");
            DropTable("dbo.ComplaintFollowUps");
            DropTable("dbo.ComplaintAnnexes");
        }
    }
}
