namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class upadteVote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Votes", "VoteTypeValue", c => c.String());
            AddColumn("dbo.Votes", "VoteTypeName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Votes", "VoteTypeName");
            DropColumn("dbo.Votes", "VoteTypeValue");
        }
    }
}
