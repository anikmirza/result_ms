using result_ms.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace result_ms.Models
{
    public class DBSContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Result> Results { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .Property(b => b.StudentId)
                .IsRequired();
            modelBuilder.Entity<Class>()
                .Property(b => b.ClassId)
                .IsRequired();
            modelBuilder.Entity<Subject>()
                .Property(b => b.SubjectId)
                .IsRequired();
            modelBuilder.Entity<Result>()
                .Property(b => b.ResultId)
                .IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var appSettingsJson = AppSettingsJson.GetAppSettings();
            optionsBuilder.UseSqlServer(appSettingsJson["DefaultConnectionStrings"]);
        }
    }
}
