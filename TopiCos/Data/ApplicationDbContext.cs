using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using TopiCos.Models;

namespace TopiCos.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed Countries Master Data
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MemberType>().HasData(
                new MemberType { Id=1, Name="Admin" },
                new MemberType { Id = 2, Name = "Co-Admin" },
                new MemberType { Id = 3, Name = "General-User" }
            );

        }



        public DbSet<Room> Rooms { get; set; }

        public DbSet<MemberType> MemberTypes { get; set; }

        public DbSet<RoomDetails> RoomDetails { get; set; }

        public DbSet<Topic> Topics { get; set; }

        public DbSet<Comment> Comments { get; set; }
    }
}
