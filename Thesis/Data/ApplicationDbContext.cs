using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Thesis.Models;

namespace Thesis.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public new DbSet<User> Users { get; set; }
        public DbSet<Models.Object> Objects { get; set; }
        public DbSet<UserObject> UserObjects { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<SuggestedInfo> SuggestedInfo { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // использование Fluent API
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserObject>().HasKey(uo => new { uo.UserId, uo.ObjectId });
            modelBuilder.Entity<UserObject>().HasOne(uo => uo.User).WithMany(uo => uo.UserObjects).HasForeignKey(uo => uo.UserId);
            modelBuilder.Entity<UserObject>().HasOne(uo => uo.Object).WithMany(uo => uo.UserObjects).HasForeignKey(uo => uo.ObjectId);

            modelBuilder.Entity<Event>().HasKey(e => e.Id); //составной ключ из трех id?
            modelBuilder.Entity<Event>().HasOne(e => e.User).WithMany(e => e.Events).HasForeignKey(e => e.UserId);
            modelBuilder.Entity<Event>().HasOne(e => e.Object).WithMany(e => e.Events).HasForeignKey(e => e.ObjectId);
        }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=thesisdb;Trusted_Connection=True;");
        //}
    }
}
