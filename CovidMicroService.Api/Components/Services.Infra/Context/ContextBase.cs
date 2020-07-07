using Microsoft.EntityFrameworkCore;
using Services.Domain.Entities;
using Services.Infra.Map;
using System;

namespace Services.Infra.Context
{
    public class ContextBase : DbContext
    {
        public ContextBase() { }

        public ContextBase(DbContextOptions<ContextBase> options) : base(options) { }
        public DbSet<Covid19> Covid19 { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new Covid19Configurations());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                   .UseSqlServer(this.StringConnectionConfig());
            }
        }

        private string StringConnectionConfig()
        {
            var conn = Environment.GetEnvironmentVariable("LocalConnection");
            return conn;
        }
    }

}
