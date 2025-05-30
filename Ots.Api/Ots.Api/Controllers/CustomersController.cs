﻿
/*
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ots.Api.Domain;
using Ots.Api.Impl.Cqrs;

namespace Ots.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IMediator mediator;
    public CustomersController(IMediator mediator)
    {
        this.mediator = mediator;
    }


    [HttpGet("GetAll")]
    public async Task<List<Customer>> GetAll()
    {
        var operation = new GetAllCustomersQuery();
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("GetById/{id}")]
    public async Task<Customer> GetByIdAsync([FromRoute] int id)
    {
        var operation = new GetCustomerByIdQuery(id);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPost]
    public async Task<Customer> Post([FromBody] Customer customer)
    {
        var operation = new CreateCustomerCommand(customer);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPut("{id}")]
    public async Task<Customer> Put([FromRoute] int id, [FromBody] Customer customer)
    {
        var operation = new UpdateCustomerCommand(id,customer);
        var result = await mediator.Send(operation);
        return result;
    }
    [HttpDelete("{id}")]
    public async Task<Customer> Delete([FromRoute] int id)
    {
        var operation = new DeleteCustomerCommand(id);
        var result = await mediator.Send(operation);
        return result;
    }

} 
*/

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ots.Api.Domain;
using Ots.Api.Impl.Cqrs;
using Ots.Base;
using Ots.Schema;

namespace Ots.Api.Controllers;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class CustomersController : ControllerBase
{
    private readonly IMediator mediator;
    public CustomersController(IMediator mediator)
    {
        this.mediator = mediator;
    }


    [HttpGet("GetAll")]
    [Authorize(Roles = "admin,user")]
    public async Task<ApiResponse<List<CustomerResponse>>> GetAll()
    {
        var operation = new GetAllCustomersQuery();
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("GetById/{id}")]
    [Authorize(Roles = "admin,user")]
    public async Task<ApiResponse<CustomerResponse>> GetByIdAsync([FromRoute] int id)
    {
        var operation = new GetCustomerByIdQuery(id);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<CustomerResponse>> Post([FromBody] CustomerRequest customer)
    {
        var operation = new CreateCustomerCommand(customer);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse> Put([FromRoute] int id, [FromBody] CustomerRequest customer)
    {
        var operation = new UpdateCustomerCommand(id,customer);
        var result = await mediator.Send(operation);
        return result;
    }
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse> Delete([FromRoute] int id)
    {
        var operation = new DeleteCustomerCommand(id);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("ByParameters")]
    
     public async Task<ApiResponse<List<CustomerResponse>>> GetByParameters([FromQuery] string? firstName, [FromQuery] string? lastName, [FromQuery] string? email)
     {
         var operation = new GetCustomerByParametersQuery(firstName, lastName, email);
         var result = await mediator.Send(operation);
         return result;
     }

}