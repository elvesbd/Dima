using Dima.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dima.Api.Data.Mappings;

public class TransactionMapping : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transaction");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(80)
            .HasColumnType("NVARCHAR");
        builder.Property(x => x.Type)
            .IsRequired()
            .HasColumnType("SMALLINT");
        builder.Property(x => x.Amount)
            .IsRequired()
            .HasColumnType("MONEY");
        builder.Property(x => x.CreatedAt)
            .IsRequired();
        builder.Property(x => x.PaidOrReceived)
            .IsRequired(false)
            .HasMaxLength(255)
            .HasColumnType("NVARCHAR");
        builder.Property(x => x.UserId)
            .IsRequired()
            .HasMaxLength(160)
            .HasColumnType("VARCHAR");
    }
}