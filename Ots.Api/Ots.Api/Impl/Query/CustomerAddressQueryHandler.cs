using AutoMapper;
using MediatR;
using Ots.Api.Domain;
using Ots.Api.Impl.Cqrs;
using Ots.Base;
using Ots.Schema;
using Microsoft.EntityFrameworkCore;
namespace Ots.Api.Impl.Query;

public class CustomerAddressQueryHandler :
IRequestHandler<GetAllCustomerAddressQuery, ApiResponse<List<CustomerAddressResponse>>>,
IRequestHandler<GetCustomerAddressByIdQuery, ApiResponse<CustomerAddressResponse>>
{
     private readonly OtsMsSqlDbContext context;
    private readonly IMapper mapper;

    public CustomerAddressQueryHandler(OtsMsSqlDbContext context, IMapper mapper)    {
 this.context = context;
         this.mapper = mapper;
    }

    public async Task<ApiResponse<List<CustomerAddressResponse>>> Handle(GetAllCustomerAddressQuery request, CancellationToken cancellationToken)
    {
        var customerAddress = await context.Set<CustomerAddress>()
         .Include(x => x.Customer).ToListAsync(cancellationToken);
 
         var mapped = mapper.Map<List<CustomerAddressResponse>>(customerAddress);
        return new ApiResponse<List<CustomerAddressResponse>>(mapped);
    }

    public async Task<ApiResponse<CustomerAddressResponse>> Handle(GetCustomerAddressByIdQuery request, CancellationToken cancellationToken)
    {
         var customerAddress = await context.Set<CustomerAddress>()
            .Include(x => x.Customer)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
 
        var mapped = mapper.Map<CustomerAddressResponse>(customerAddress);
        return new ApiResponse<CustomerAddressResponse>(mapped);
    }
}