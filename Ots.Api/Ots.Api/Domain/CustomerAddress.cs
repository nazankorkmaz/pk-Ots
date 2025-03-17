using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ots.Api.Domain;
using Ots.Base;


[Table("CustomerAddress",Schema ="dbo")]
public class CustomerAddress:BaseEntity
{
	public long CustomerId {get;set;}
	public virtual Customer Customer {get;set;}
	public string? Country {get;set;}
	public string? City {get;set;}
	public string? Street {get;set;}
	public string? District {get;set;}
	public string? ZipCode {get;set;}
}

public class CustomerAddressConfiguration : IEntityTypeConfiguration<CustomerAddress>
{
    public void Configure(EntityTypeBuilder<CustomerAddress> builder)
    {
        builder.HasKey(ca => ca.Id);
        builder.Property(ca => ca.Id).UseIdentityColumn();


        builder.Property(x => x.CustomerId).IsRequired(true);
        builder.Property(ca => ca.Country).IsRequired().HasMaxLength(100);

        builder.Property(ca => ca.City).IsRequired().HasMaxLength(100);
        builder.Property(ca => ca.District).IsRequired().HasMaxLength(100);
        builder.Property(ca => ca.Street).IsRequired().HasMaxLength(100);
        builder.Property(ca => ca.ZipCode).IsRequired().HasMaxLength(10);

    }
}