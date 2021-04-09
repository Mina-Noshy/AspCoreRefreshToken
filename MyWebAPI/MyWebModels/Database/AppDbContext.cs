using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyWebModels.Seeding;
using MyWebModels.Models;
using MyWebModels.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyWebModels.ViewModels;

namespace MyWebModels.Database
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>().HasMany<Chat>().WithOne(m => m.GetUser_1).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<AppUser>().HasMany<Chat>().WithOne(m => m.GetUser_2).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Chat>().HasOne<AppUser>().WithMany(m => m.GetChates_1).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Chat>().HasOne<AppUser>().WithMany(m => m.GetChates_2).OnDelete(DeleteBehavior.Restrict);

            RoleSeed.Seed(builder);
            UserSeed.Seed(builder);
            UserRoleSeed.Seed(builder);
            CountrySeed.Seed(builder);
            CitySeed.Seed(builder);
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        public DbSet<Country> Countries{ get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }

    }
}
