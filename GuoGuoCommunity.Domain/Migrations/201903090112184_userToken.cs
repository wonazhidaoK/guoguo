namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userToken : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "RefreshToken", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "RefreshToken");
        }
    }
}
