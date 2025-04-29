/*
using AutoMapper;
using MediatR;
using Ots.Api.Domain;
using Ots.Api.Impl.Cqrs;
using Ots.Base;
using Ots.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ots.Api.Impl.Query;

public class CustomerPhoneQueryHandler :
IRequestHandler<GetAllCustomerPhonesQuery, ApiResponse<List<CustomerPhoneResponse>>>,
IRequestHandler<GetCustomerPhoneByIdQuery, ApiResponse<CustomerPhoneResponse>>
{
     private readonly OtsMsSqlDbContext context;
    private readonly IMapper mapper;

    public CustomerPhoneQueryHandler(OtsMsSqlDbContext context, IMapper mapper)
    {
            this.context = context;
         this.mapper = mapper;
    }

    public async Task<ApiResponse<List<CustomerPhoneResponse>>> Handle(GetAllCustomerPhonesQuery request, CancellationToken cancellationToken)
    {
         var customerPhones = await context.Set<CustomerPhone>().Include(x => x.Customer).ToListAsync(cancellationToken);
 
        var mapped = mapper.Map<List<CustomerPhoneResponse>>(customerPhones);
        return new ApiResponse<List<CustomerPhoneResponse>>(mapped);
    }

    public async Task<ApiResponse<CustomerPhoneResponse>> Handle(GetCustomerPhoneByIdQuery request, CancellationToken cancellationToken)
    {
        var customerPhone = await context.Set<CustomerPhone>()
             .Include(x => x.Customer)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
 
        var mapped = mapper.Map<CustomerPhoneResponse>(customerPhone);
        return new ApiResponse<CustomerPhoneResponse>(mapped);
    }
}

*/


using AutoMapper;
using MediatR;
using Ots.Api.Domain;
using Ots.Api.Impl.Cqrs;
using Ots.Base;
using Ots.Schema;

namespace Ots.Api.Impl.Query;

public class CustomerPhoneQueryHandler :
IRequestHandler<GetAllCustomerPhonesQuery, ApiResponse<List<CustomerPhoneResponse>>>,
IRequestHandler<GetCustomerPhoneByIdQuery, ApiResponse<CustomerPhoneResponse>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public CustomerPhoneQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<List<CustomerPhoneResponse>>> Handle(GetAllCustomerPhonesQuery request, CancellationToken cancellationToken)
    {
        var customerPhones = await unitOfWork.CustomerPhoneRepository.GetAllAsync("Customer");
        var mapped = mapper.Map<List<CustomerPhoneResponse>>(customerPhones);
        return new ApiResponse<List<CustomerPhoneResponse>>(mapped);
    }

    public async Task<ApiResponse<CustomerPhoneResponse>> Handle(GetCustomerPhoneByIdQuery request, CancellationToken cancellationToken)
    {
        var customerPhone = await unitOfWork.CustomerPhoneRepository.GetByIdAsync(request.Id, "Customer");
        var mapped = mapper.Map<CustomerPhoneResponse>(customerPhone);
        return new ApiResponse<CustomerPhoneResponse>(mapped);
    }
}