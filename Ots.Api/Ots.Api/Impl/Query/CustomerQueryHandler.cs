using MediatR;
using Microsoft.EntityFrameworkCore;
using Ots.Api.Domain;
using Ots.Api.Impl.Cqrs;

namespace Ots.Api.Impl.Query;

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