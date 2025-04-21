
/*
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ots.Api.Domain;
using Ots.Api.Impl.Cqrs;

namespace Ots.Api.Impl.Query;

public class CustomerCommandHandler 
{
    private readonly OtsMsSqlDbContext context;

    public CustomerCommandHandler(OtsMsSqlDbContext context)
    {
        this.context = context;
    }  
}

*/

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ots.Api.Domain;
using Ots.Api.Impl.Cqrs;
using Ots.Base;
using Ots.Schema;
using System.Diagnostics;
using EFCore.BulkExtensions;

namespace Ots.Api.Impl.Query;

public class CustomerCommandHandler :
IRequestHandler<CreateCustomerCommand, ApiResponse<CustomerResponse>>,
IRequestHandler<UpdateCustomerCommand, ApiResponse>,
IRequestHandler<DeleteCustomerCommand, ApiResponse>
{
    private readonly  OtsMsSqlDbContext dbContext;
    private readonly IMapper mapper;
    

    public CustomerCommandHandler( OtsMsSqlDbContext dbContext, IMapper mapper) // buralarda scope olarak  dbcontexe erisim oluyor
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ApiResponse> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Set<Customer>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
            return new ApiResponse("Customer not found");

        if (!entity.IsActive)
            return new ApiResponse("Customer is not active");

        entity.IsActive = false;

        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public static int GenerateRandom11DigitNumber(Random random)
{
    // 11 basamaklı minimum ve maksimum değerler
    const long min = 10000000000;
    const long max = 99999999999;

    // long üretmek için iki int birleştiriyoruz
    byte[] buf = new byte[8];
    random.NextBytes(buf);
    long longRand = BitConverter.ToInt64(buf, 0);
    longRand = Math.Abs(longRand % (max - min)) + min;
    return (int)longRand;
}



    public async Task<ApiResponse> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Set<Customer>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
            return new ApiResponse("Customer not found");

        if (!entity.IsActive)
            return new ApiResponse("Customer is not active");

        entity.FirstName = request.customer.FirstName;
        entity.MiddleName = request.customer.MiddleName;
        entity.LastName = request.customer.LastName;
        entity.Email = request.customer.Email;
        entity.UpdatedDate = DateTime.Now;
        entity.UpdatedUser = null;

        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse<CustomerResponse>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<Customer>(request.customer);
        mapped.InsertedDate = DateTime.Now;
        mapped.OpenDate = DateTime.Now;
        mapped.InsertedUser = "test";
        mapped.CustomerNumber = new Random().Next(1000000, 999999999);
        mapped.IsActive = true;

        var entity = await dbContext.AddAsync(mapped, cancellationToken);

       // var entity1 = await dbContext.AddAsync(mapped, cancellationToken);
        //var entity2 = await dbContext.AddAsync(mapped, cancellationToken);

        // Burada Efcore savecanhanges denilince veritabına erişir.
        // efcore üzerinde dbcontext üzerinde bir changeTracker var. Bu veritabında dbContext üzerinde yapılan tüm değişiklikleri verir.
        // savechanges changeTrackera sorar bana tüm değişikleri ver diye ve bunu class seviyesinde tutar. veri update, delete falan ghangisi olacagını change tracker yakalayıp sql cümleciklerine göre veriyor.  Bu değişiklikleri bilgisayarın belleğinde saklıyor.

        await dbContext.SaveChangesAsync(cancellationToken);

/*
        foreach( var item in entity.Entity.CustomerAddresses){
            var entit4 = await dbContext.AddAsync(mapped,cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        var entit45 = await dbContext.AddAsync(mapped,cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
*/

        /*
        //method 1 ve 2
        var stopwatch = Stopwatch.StartNew();
        var random = new Random();
        for (int i = 0; i < 10000; i++)
        {
        var customer = new Customer
        {
            FirstName = $"Customer {i}",
            MiddleName = $"CustomerM {i}",
            LastName = $"CustomerL {i}",
            InsertedDate = DateTime.Now,
            InsertedUser = "test",
            OpenDate = DateTime.Now,
            IsActive = true,
            IdentityNumber = $"x0000000004",//random.Next(100000000, int.MaxValue),
            Email = $"customer{i}@example.com",
            CustomerNumber = new Random().Next(1000000, 999999999)

        };

            await dbContext.AddAsync(customer);
            //await dbContext.SaveChangesAsync(); // her bir insert sonrası veritabanına yazar
        }
        await dbContext.SaveChangesAsync(); // her bir insert sonrası veritabanına yazar

        stopwatch.Stop();
        Console.WriteLine($"Foreach içinde SaveChanges: {stopwatch.ElapsedMilliseconds} ms");  // Foreach içinde SaveChanges: 67850 ms
        // Foreach dışında SaveChanges: 8843 ms
        */
/*
        var customers = new List<Customer>();
        for (int i = 0; i < 10000; i++)
        {
            customers.Add(new Customer
            {
                FirstName = $"CustomerBulk {i}",
                MiddleName = $"CustomerMBulk {i}",
                LastName = $"CustomerLBulk {i}",
                InsertedDate = DateTime.Now,
                InsertedUser = "test",
                OpenDate = DateTime.Now,
                IsActive = true,
                IdentityNumber = $"x0000000005",//random.Next(100000000, int.MaxValue),
                Email = $"customerBulk{i}@example.com",
                CustomerNumber = new Random().Next(1000000, 999999999)
            });
        }

        var stopwatch = Stopwatch.StartNew();
        await dbContext.BulkInsertAsync(customers);
        stopwatch.Stop();
        Console.WriteLine($"BulkInsert: {stopwatch.ElapsedMilliseconds} ms"); // BulkInsert: 21797 ms
*/
        
        var response = mapper.Map<CustomerResponse>(entity.Entity);
        return new ApiResponse<CustomerResponse>(response);
    }
}