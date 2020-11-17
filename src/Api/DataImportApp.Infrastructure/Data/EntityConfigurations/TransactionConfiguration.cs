using DataImportApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataImportApp.Infrastructure.Data.EntityConfigurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");

            builder.HasKey(tr => tr.Id);
            builder.Property(tr => tr.Id).HasMaxLength(50);
            builder.Property(tr => tr.CurrencyCode).HasMaxLength(3).IsRequired();
            builder.Property(tr => tr.Status).HasConversion<string>().HasMaxLength(20);
        }
    }
}
