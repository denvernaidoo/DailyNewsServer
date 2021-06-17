using DailyNewsServer.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DailyNewsServer.Data
{
    public class DailyNewsDbContext : DbContext
    {
        public DailyNewsDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
             * If these were not configured below then Entity Frameworwork would assume the table names 
             * would be the same as DbSet names eg Courses instead of Course
             */
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Role>().ToTable("Role");
        }
    }
}
