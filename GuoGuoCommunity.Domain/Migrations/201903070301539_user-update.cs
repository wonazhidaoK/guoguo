namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "PhoneNumber", c => c.String());
            AddColumn("dbo.Users", "RoleId", c => c.String());
            AddColumn("dbo.Users", "RoleName", c => c.String());
            DropColumn("dbo.Users", "RolesId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "RolesId", c => c.String());
            DropColumn("dbo.Users", "RoleName");
            DropColumn("dbo.Users", "RoleId");
            DropColumn("dbo.Users", "PhoneNumber");
        }
    }
}
