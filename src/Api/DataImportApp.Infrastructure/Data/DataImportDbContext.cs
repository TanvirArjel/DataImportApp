using System;
using DataImportApp.Infrastructure.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace DataImportApp.Infrastructure.Data
{
    public class DataImportDbContext : DbContext
    {
        public DataImportDbContext(DbContextOptions<DataImportDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TransactionConfiguration).Assembly);
        }
    }
}
