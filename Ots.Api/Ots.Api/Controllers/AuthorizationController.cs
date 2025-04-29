using MediatR;
 using Microsoft.AspNetCore.Mvc;
 using Ots.Api.Domain;
 using Ots.Api.Impl.Cqrs;
 using Ots.Base;
 using Ots.Schema;
 
 using Ots.Api.Filter;

 namespace Ots.Api.Controllers;
 
 
 [ApiController]
 [Route("api/[controller]")]
 public class AuthorizationController : ControllerBase
 {
     private readonly IMediator mediator;
     public AuthorizationController(IMediator mediator)
     {
         this.mediator = mediator;
     }
 
     [HttpPost("Token")]
     [TypeFilter(typeof(LogResourceFilter))]
     [TypeFilter(typeof(LogActionFilter))]
     [TypeFilter(typeof(LogAuthorizationFilter))]
     [TypeFilter(typeof(LogResultFilter))]
     [TypeFilter(typeof(LogExceptionFilter))]
     public async Task<ApiResponse<AuthorizationResponse>> Post([FromBody] AuthorizationRequest request)
     {
         var operation = new CreateAuthorizationTokenCommand(request);
         var result = await mediator.Send(operation);
         return result;
     }
 
 }