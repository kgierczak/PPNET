using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using PPNET_2.Models;

namespace PPNET_2.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Category> Categories { get; set; }

        public AppDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "meals.db");
            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }
    }
}