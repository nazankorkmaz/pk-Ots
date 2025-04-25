using AutoMapper;
 using MediatR;
 using Microsoft.EntityFrameworkCore;
 using Ots.Api.Domain;
 using Ots.Api.Impl.Cqrs;
 using Ots.Base;
 using Ots.Schema;
 
 namespace Ots.Api.Impl.Query;
 
 public class AccountTransactionQueryHandler :
 IRequestHandler<GetAccountTransactionByParametersQuery, ApiResponse<List<AccountTransactionResponse>>>,
 IRequestHandler<GetAccountTransactionByIdQuery, ApiResponse<AccountTransactionResponse>>
 {
     private readonly OtsMsSqlDbContext context;
     private readonly IMapper mapper;
     public AccountTransactionQueryHandler(OtsMsSqlDbContext context, IMapper mapper)
     {
         this.context = context;
         this.mapper = mapper;
     }
     public async Task<ApiResponse<List<AccountTransactionResponse>>> Handle(GetAccountTransactionByParametersQuery request, CancellationToken cancellationToken)
     {
         var AccountTransaction = await context.Set<AccountTransaction>()
         .Include(x => x.Account).ThenInclude(x => x.Customer)
         .ToListAsync(cancellationToken);
 
         var mapped = mapper.Map<List<AccountTransactionResponse>>(AccountTransaction);
         return new ApiResponse<List<AccountTransactionResponse>>(mapped);
     }
 
     public async Task<ApiResponse<AccountTransactionResponse>> Handle(GetAccountTransactionByIdQuery request, CancellationToken cancellationToken)
     {
         var AccountTransaction = await context.Set<AccountTransaction>()
         .Include(x => x.Account).ThenInclude(x => x.Customer)
         .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
 
         var mapped = mapper.Map<AccountTransactionResponse>(AccountTransaction);
         return new ApiResponse<AccountTransactionResponse>(mapped);
     }
 }