using Executor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;

namespace Executor
{
    public class ApplicationContext : DbContext
    {
        public DbSet<GeneratorDb> Generators { get; set; } = null!;
        public DbSet<TaskDb> Tasks { get; set; } = null!;

        public ApplicationContext()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=my-postgres;Port=5432;Database=messagesync;Username=postgres;Password=postgres");
        }
    }
}
