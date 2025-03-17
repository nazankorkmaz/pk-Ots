namespace Ots.Api.Domain;

using System.ComponentModel.DataAnnotations.Schema;
using Ots.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


[Table("Customer", Schema = "dbo")]
public class Customer: BaseEntity
{

    public string Name {get;set;}
    public String Email {get;set;}
    public String Phone {get;set;}

    public virtual List<CustomerAddress> CustomerAdresses {get;set;} // yani bir customer altında birden fazla adres olabilir birden coka iliski

    //public virtual List<CustomerPhone> CustomerPhones { get; set; }
    public virtual List<Account> Accounts { get; set; }
}

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>{

    public void Configure(EntityTypeBuilder<Customer> builder)
    {

        builder.HasKey(x => x.Id);


        builder.Property(x=> x.InsertedDate).IsRequired(true);
        builder.Property(x=> x.UpdatedDate).IsRequired(false);
        builder.Property(x=> x.InsertedUser).IsRequired(true).HasMaxLength(250);
        builder.Property(x=> x.UpdatedUser).IsRequired(false).HasMaxLength(250);
    
        builder.HasMany(x => x.CustomerAdresses )
        .WithOne(x => x.Customer)
        .HasForeignKey(x => x.CustomerId);

        builder.HasMany(x => x.Accounts)
            .WithOne(x => x.Customer)
            .HasForeignKey(x => x.CustomerId).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
    }
}

