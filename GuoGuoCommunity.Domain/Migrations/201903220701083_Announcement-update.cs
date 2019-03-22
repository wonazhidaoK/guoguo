namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Announcementupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Announcements", "StreetOfficeId", c => c.String());
            AddColumn("dbo.Announcements", "StreetOfficeName", c => c.String());
            AddColumn("dbo.Announcements", "CommunityId", c => c.String());
            AddColumn("dbo.Announcements", "CommunityName", c => c.String());
            AddColumn("dbo.Announcements", "SmallDistrictId", c => c.String());
            AddColumn("dbo.Announcements", "SmallDistrictName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Announcements", "SmallDistrictName");
            DropColumn("dbo.Announcements", "SmallDistrictId");
            DropColumn("dbo.Announcements", "CommunityName");
            DropColumn("dbo.Announcements", "CommunityId");
            DropColumn("dbo.Announcements", "StreetOfficeName");
            DropColumn("dbo.Announcements", "StreetOfficeId");
        }
    }
}
