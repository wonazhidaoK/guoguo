namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatevipOwnerApplicationRecord : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VipOwnerApplicationRecords", "VoteId", c => c.String());
            AddColumn("dbo.VipOwnerApplicationRecords", "VoteQuestionId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.VipOwnerApplicationRecords", "VoteQuestionId");
            DropColumn("dbo.VipOwnerApplicationRecords", "VoteId");
        }
    }
}
