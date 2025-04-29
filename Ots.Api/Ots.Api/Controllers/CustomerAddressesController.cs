using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ots.Api.Impl.Cqrs;
using Ots.Base;
using Ots.Schema;
using Microsoft.AspNetCore.Authorization;
using Ots.Api.Domain;
namespace Ots.Api.Controllers;

using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class CustomerAddresssController : ControllerBase
{
    private readonly IMediator mediator;
    public CustomerAddresssController(IMediator mediator)
    {
        this.mediator = mediator;
    }


    [HttpGet("GetAll")]
  [Authorize(Roles = "admin,user")]
    public async Task<ApiResponse<List<CustomerAddressResponse>>> GetAll()
    {
        var operation = new GetAllCustomerAddressQuery();
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("GetById/{id}")]
    [Authorize(Roles = "admin,user")]
    public async Task<ApiResponse<CustomerAddressResponse>> GetByIdAsync([FromRoute] int id)
    {
        var operation = new GetCustomerAddressByIdQuery(id);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPost("ByCustomerId/{CustomerId}")]

    public async Task<ApiResponse<CustomerAddressResponse>> Post(int CustomerId, [FromBody] CustomerAddressRequest CustomerAddress)
    {
        var operation = new CreateCustomerAddressCommand(CustomerId,CustomerAddress);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin,user")]
    public async Task<ApiResponse> Put([FromRoute] int id, [FromBody] CustomerAddressRequest CustomerAddress)
    {
        var operation = new UpdateCustomerAddressCommand(id,CustomerAddress);
        var result = await mediator.Send(operation);
        return result;
    }
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin,user")]
  
    public async Task<ApiResponse> Delete([FromRoute] int id)
    {
        var operation = new DeleteCustomerAddressCommand(id);
        var result = await mediator.Send(operation);
        return result;
    }

}