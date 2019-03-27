namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVoteResultRecord : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VoteResultRecords",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        VoteId = c.String(),
                        CalculationMethodValue = c.String(),
                        CalculationMethodName = c.String(),
                        ResultValue = c.String(),
                        ResultName = c.String(),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.VipOwnerApplicationRecords", "OwnerCertificationId", c => c.String());
            AddColumn("dbo.Votes", "CalculationMethodValue", c => c.String());
            AddColumn("dbo.Votes", "CalculationMethodName", c => c.String());
            DropColumn("dbo.Votes", "CalculationMethod");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Votes", "CalculationMethod", c => c.String());
            DropColumn("dbo.Votes", "CalculationMethodName");
            DropColumn("dbo.Votes", "CalculationMethodValue");
            DropColumn("dbo.VipOwnerApplicationRecords", "OwnerCertificationId");
            DropTable("dbo.VoteResultRecords");
        }
    }
}
