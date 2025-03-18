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