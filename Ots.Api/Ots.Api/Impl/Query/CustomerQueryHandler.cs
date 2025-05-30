/*
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ots.Api.Domain;
using Ots.Api.Impl.Cqrs;

namespace Ots.Api.Impl.Query;

/* IRequestHandler MediatRden geliyor */
/*
public class CustomerQueryHandler :
IRequestHandler<GetAllCustomersQuery, List<Customer>>,
IRequestHandler<GetCustomerByIdQuery, Customer>
{
    private readonly OtsMsSqlDbContext context;

    public CustomerQueryHandler(OtsMsSqlDbContext context)
    {
        this.context = context;
    }
    public async Task<List<Customer>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await context.Set<Customer>().ToListAsync(cancellationToken);
        return customers;
    }

    public async Task<Customer> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await context.Set<Customer>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        return customer;
    }
}

*/


/*
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ots.Api.Domain;
using Ots.Api.Impl.Cqrs;
using Ots.Base;
using Ots.Schema;

namespace Ots.Api.Impl.Query;

public class CustomerQueryHandler :
IRequestHandler<GetAllCustomersQuery, ApiResponse<List<CustomerResponse>>>,
IRequestHandler<GetCustomerByIdQuery, ApiResponse<CustomerResponse>>
{
    private readonly  OtsMsSqlDbContext context;
    private readonly IMapper mapper;
    public CustomerQueryHandler( OtsMsSqlDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }
    public async Task<ApiResponse<List<CustomerResponse>>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await context.Set<Customer>().Include(x => x.CustomerAddresses).Include(x =>x.CustomerPhones ).ToListAsync(cancellationToken);
        var mapped = mapper.Map<List<CustomerResponse>>(customers);
        return new ApiResponse<List<CustomerResponse>>(mapped);
    }

    public async Task<ApiResponse<CustomerResponse>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await context.Set<Customer>().Include(x => x.CustomerAddresses).Include(x => x.CustomerPhones).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        // Include ile yeni ekledigimiz baglantilari da gostermesini sagladik.
        var mapped = mapper.Map<CustomerResponse>(customer);
        return new ApiResponse<CustomerResponse>(mapped);
    }
}

*/


using AutoMapper;
using LinqKit;
using MediatR;
using Ots.Api.Domain;
using Ots.Api.Impl.Cqrs;
using Ots.Base;
using Ots.Schema;

namespace Ots.Api.Impl.Query;

public class CustomerQueryHandler :
IRequestHandler<GetAllCustomersQuery, ApiResponse<List<CustomerResponse>>>,
IRequestHandler<GetCustomerByIdQuery, ApiResponse<CustomerResponse>>,
IRequestHandler<GetCustomerByParametersQuery, ApiResponse<List<CustomerResponse>>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public CustomerQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<List<CustomerResponse>>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await unitOfWork.CustomerRepository.GetAllAsync("CustomerAddresses", "CustomerPhones", "Accounts");
        var mapped = mapper.Map<List<CustomerResponse>>(customers);
        return new ApiResponse<List<CustomerResponse>>(mapped);
    }

    public async Task<ApiResponse<CustomerResponse>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await unitOfWork.CustomerRepository.GetByIdAsync(request.Id, "CustomerAddresses", "CustomerPhones", "Accounts");
        if (customer == null)
        {
            return new ApiResponse<CustomerResponse>("Customer not found");
        }

        var mapped = mapper.Map<CustomerResponse>(customer);
        return new ApiResponse<CustomerResponse>(mapped);
    }

    public async Task<ApiResponse<List<CustomerResponse>>> Handle(GetCustomerByParametersQuery request, CancellationToken cancellationToken)
    {
        var customers1 = await unitOfWork.CustomerRepository.Where(
            x => (string.IsNullOrEmpty(request.FirstName) || x.FirstName == request.FirstName) &&
             (string.IsNullOrEmpty(request.LastName) || x.LastName == request.LastName) &&
             ((x.Email == request.Email) || x.CustomerPhones.Any(y => y.PhoneNumber == "905545545454")),
            "CustomerAddresses", "CustomerPhones", "Accounts");

        var predicate = PredicateBuilder.New<Customer>(true);
        if (!string.IsNullOrEmpty(request.FirstName))
            predicate = predicate.And(x => x.FirstName.ToLower() == request.FirstName.ToLower());
        if (!string.IsNullOrEmpty(request.LastName))
            predicate = predicate.And(x => x.LastName == request.LastName);
        if (!string.IsNullOrEmpty(request.Email))
            predicate = predicate.And(x => x.Email == request.Email || x.CustomerPhones.Any(y => y.PhoneNumber == "905545545454"));

        var customers2 = await unitOfWork.CustomerRepository.Where(
            predicate, 
            "CustomerAddresses", "CustomerPhones", "Accounts");

        var mapped = mapper.Map<List<CustomerResponse>>(customers2);
        return new ApiResponse<List<CustomerResponse>>(mapped);
    }
}