using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ots.Base;

namespace Ots.Api.Domain;

// amac raporlamak
// mesela para b,i yerden bir yere gonderildigi zaman buraya 1 tane islem gelicek 
// ama AccountTransactionda alan ve veren icin toplam 2 tane islem olucak

[Table("MoneyTransfer", Schema = "dbo")]
public class MoneyTransfer : BaseEntity
{
    public long FromAccountId { get; set; }
    public long ToAccountId { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public decimal? FeeAmount { get; set; }
    public DateTime TransactionDate { get; set; }
    public string ReferenceNumber { get; set; }
}

public class MoneyTransferConfiguration : IEntityTypeConfiguration<MoneyTransfer>
{
    public void Configure(EntityTypeBuilder<MoneyTransfer> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();

        builder.Property(x => x.InsertedDate).IsRequired(true);
        builder.Property(x => x.UpdatedDate).IsRequired(false);
        builder.Property(x => x.InsertedUser).IsRequired(true).HasMaxLength(250);
        builder.Property(x => x.UpdatedUser).IsRequired(false).HasMaxLength(250);
        builder.Property(x => x.IsActive).IsRequired(true).HasDefaultValue(true);

        builder.Property(x => x.FromAccountId).IsRequired(true);
        builder.Property(x => x.ToAccountId).IsRequired(true);
        builder.Property(x => x.Amount).IsRequired(true).HasPrecision(16, 4);
        builder.Property(x => x.FeeAmount).IsRequired(false).HasPrecision(16, 4);
        builder.Property(x => x.TransactionDate).IsRequired(true);
        builder.Property(x => x.Description).IsRequired(true).HasMaxLength(500);
        builder.Property(x => x.ReferenceNumber).IsRequired(true).HasMaxLength(50);
    }
}