
/*
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Ots.Base;

namespace Ots.Api.Domain;


using Microsoft.EntityFrameworkCore.Metadata.Builders;


[Table("Account", Schema = "dbo")] // bununl aaccount tablein adini istedigin gibi degistirebilirsin
public class Account : BaseEntity
{
    public long CustomerId { get; set; }
    public virtual Customer Customer { get; set; }
    
    public string Name { get; set; }
    public int AccountNumber { get; set; }
    public string IBAN { get; set; }
    public decimal Balance { get; set; }
    public string CurrencyCode { get; set; }
    public DateTime OpenDate { get; set; }
    public DateTime? CloseDate { get; set; }
    public virtual List<AccountTransaction> AccountTransactions { get; set; }
    public virtual List<EftTransaction> EftTransactions { get; set; }
}   


public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();

        //baseden gelen alanlar
        builder.Property(x=> x.InsertedDate).IsRequired(true);
        builder.Property(x=> x.UpdatedDate).IsRequired(false);
        builder.Property(x=> x.InsertedUser).IsRequired(true).HasMaxLength(250);
        builder.Property(x=> x.UpdatedUser).IsRequired(false).HasMaxLength(250);
        builder.Property(x=> x.IsActive).IsRequired(true).HasDefaultValue(true);
   
        builder.Property(x=> x.CustomerId).IsRequired(true);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.AccountNumber).IsRequired();
        builder.Property(x => x.IBAN).IsRequired().HasMaxLength(26);
        builder.Property(x => x.Balance).IsRequired().HasPrecision(18,2);
        builder.Property(x => x.CurrencyCode).IsRequired().HasMaxLength(3);
        builder.Property(x => x.OpenDate).IsRequired();
        builder.Property(x => x.CloseDate).IsRequired(true);

        builder.HasIndex(x => x.AccountNumber).IsUnique(true);

        builder.HasMany(x => x.AccountTransactions)
            .WithOne(x => x.Account)
            .HasForeignKey(x => x.AccountId).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
        // isRequired yani accountTransactionda accountId'si bos olan bir kayit olmasin

        
    }
}

*/

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ots.Base;

namespace Ots.Api.Domain;

[Table("Account", Schema = "dbo")]
public class Account : BaseEntity
{
    public long CustomerId { get; set; }
    public virtual Customer Customer { get; set; }
    
    public string Name { get; set; }
    public int AccountNumber { get; set; }
    public string IBAN { get; set; }
    public decimal Balance { get; set; }
    public string CurrencyCode { get; set; }
    public DateTime OpenDate { get; set; }
    public DateTime? CloseDate { get; set; }

    public virtual List<EftTransaction> EftTransactions { get; set; }
    public virtual List<AccountTransaction> AccountTransactions { get; set; }
}

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();

        builder.Property(x=> x.InsertedDate).IsRequired(true);
        builder.Property(x=> x.UpdatedDate).IsRequired(false);
        builder.Property(x=> x.InsertedUser).IsRequired(true).HasMaxLength(250);
        builder.Property(x=> x.UpdatedUser).IsRequired(false).HasMaxLength(250);
        builder.Property(x=> x.IsActive).IsRequired(true).HasDefaultValue(true);
        
        builder.Property(x=> x.CustomerId).IsRequired(true);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.AccountNumber).IsRequired();
        builder.Property(x => x.IBAN).IsRequired().HasMaxLength(26);
        builder.Property(x => x.Balance).IsRequired().HasPrecision(18,2);
        builder.Property(x => x.CurrencyCode).IsRequired().HasMaxLength(3);
        builder.Property(x => x.OpenDate).IsRequired();
        builder.Property(x => x.CloseDate).IsRequired(false);

        builder.HasMany(x => x.AccountTransactions)
            .WithOne(x => x.Account)
            .HasForeignKey(x => x.AccountId).IsRequired(true).OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.EftTransactions)
            .WithOne(x => x.Account)
            .HasForeignKey(x => x.AccountId).IsRequired(true).OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x=> x.AccountNumber).IsUnique(true);
    }
}