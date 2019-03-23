namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatestationLetter : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.StationLetters", "DepartmentName");
            DropColumn("dbo.StationLetters", "DepartmentValue");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StationLetters", "DepartmentValue", c => c.String());
            AddColumn("dbo.StationLetters", "DepartmentName", c => c.String());
        }
    }
}
