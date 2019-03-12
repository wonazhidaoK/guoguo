using GuoGuoCommunity.Domain.Models;
using System.Data.Entity;

namespace GuoGuoCommunity.Domain
{
    public class GuoGuoCommunityContext : DbContext
    {
        public GuoGuoCommunityContext() : base("name=GuoGuoCommunityContext")
        {
            Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }


        public DbSet<Test> Tests { get; set; }

        public DbSet<Role_Menu> Role_Menus { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<User_Role> User_Roles { get; set; }

        public DbSet<Menu> Menus { get; set; }

        public DbSet<StreetOffice> StreetOffices { get; set; }

        public DbSet<Community> Communities { get; set; }

        public DbSet<SmallDistrict> SmallDistricts { get; set; }

        public DbSet<VipOwnerStructure> VipOwnerStructures { get; set; }

        public DbSet<VipOwner> VipOwners { get; set; }

        public DbSet<ComplaintType> ComplaintTypes { get; set; }

        public DbSet<BuildingUnit> BuildingUnits { get; set; }

        public DbSet<Building> Buildings { get; set; }
    }
}
