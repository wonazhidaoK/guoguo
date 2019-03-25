namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVote : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VoteAnnexes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        VoteId = c.String(),
                        AnnexContent = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VoteQuestionOptions",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        VoteId = c.String(),
                        VoteQuestionId = c.String(),
                        Describe = c.String(),
                        Votes = c.Int(nullable: false),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VoteQuestions",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        VoteId = c.String(),
                        Title = c.String(),
                        OptionMode = c.String(),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VoteRecordDetails",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        VoteId = c.String(),
                        VoteQuestionId = c.String(),
                        VoteQuestionOptionId = c.String(),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VoteRecords",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        VoteId = c.String(),
                        Feedback = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Votes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Title = c.String(),
                        Summary = c.String(),
                        Deadline = c.DateTimeOffset(nullable: false, precision: 7),
                        SmallDistrictArray = c.String(),
                        StreetOfficeId = c.String(),
                        StreetOfficeName = c.String(),
                        CommunityId = c.String(),
                        CommunityName = c.String(),
                        SmallDistrictId = c.String(),
                        SmallDistrictName = c.String(),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Votes");
            DropTable("dbo.VoteRecords");
            DropTable("dbo.VoteRecordDetails");
            DropTable("dbo.VoteQuestions");
            DropTable("dbo.VoteQuestionOptions");
            DropTable("dbo.VoteAnnexes");
        }
    }
}
