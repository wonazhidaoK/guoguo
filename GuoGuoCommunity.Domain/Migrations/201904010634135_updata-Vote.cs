namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updataVote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Votes", "StatusValue", c => c.String());
            AddColumn("dbo.Votes", "StatusName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Votes", "StatusName");
            DropColumn("dbo.Votes", "StatusValue");
        }
    }
}
