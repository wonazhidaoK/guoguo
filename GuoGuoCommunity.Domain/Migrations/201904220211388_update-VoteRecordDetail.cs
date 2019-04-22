namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateVoteRecordDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VoteRecordDetails", "OwnerCertificationId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.VoteRecordDetails", "OwnerCertificationId");
        }
    }
}
