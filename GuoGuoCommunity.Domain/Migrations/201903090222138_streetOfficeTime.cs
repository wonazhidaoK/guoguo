namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class streetOfficeTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StreetOffices", "LastOperationTime", c => c.DateTimeOffset(precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StreetOffices", "LastOperationTime", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
    }
}
