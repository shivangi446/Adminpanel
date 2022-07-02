using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project_Infrastructure.EntityModels
{
    public class ApplicationEntityDbContext : DbContext
    {
        public ApplicationEntityDbContext(DbContextOptions options)
        : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
        public DbSet<TestTable> TestTable { get; set; }
    }
}
