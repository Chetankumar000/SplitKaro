using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SplitKaro.Models;

namespace SplitKaro.Data
{
    public class AppDBContext: IdentityDbContext<AppUser>
    {
        public AppDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Define composite primary key for GroupMember
            builder.Entity<GroupMember>(x => x.HasKey(p => new { p.AppUserId, p.GroupId }));

            // Define relationship for GroupMember -> AppUser with cascade delete
            builder.Entity<GroupMember>()
                .HasOne(u => u.AppUser)
                .WithMany(u => u.GroupMembers)
                .HasForeignKey(u => u.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete for AppUser

            // Define relationship for GroupMember -> Group without cascade delete
            builder.Entity<GroupMember>()
                .HasOne(u => u.Group)
                .WithMany(u => u.GroupMembers)
                .HasForeignKey(u => u.GroupId)
                .OnDelete(DeleteBehavior.Restrict);  // No cascade delete for Group

            // Seeding roles
            List<IdentityRole> roles = new List<IdentityRole>
    {
        new IdentityRole
        {
            Id = Guid.NewGuid().ToString(),  // Ensure unique Id
            Name = "Admin",
            NormalizedName = "ADMIN"
        },
        new IdentityRole
        {
            Id = Guid.NewGuid().ToString(),  // Ensure unique Id
            Name = "User",
            NormalizedName = "USER"
        }
    };
            builder.Entity<IdentityRole>().HasData(roles);
        }


    }
}
