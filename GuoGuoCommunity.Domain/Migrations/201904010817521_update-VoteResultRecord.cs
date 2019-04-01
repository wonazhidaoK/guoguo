namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateVoteResultRecord : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VoteResultRecords", "ShouldParticipateCount", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.VoteResultRecords", "ShouldParticipateCount");
        }
    }
}
