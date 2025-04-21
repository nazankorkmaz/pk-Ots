/*
using MediatR;
using Ots.Api.Domain;

namespace Ots.Api.Impl.Cqrs;

public record GetAllCustomersQuery : IRequest<List<Customer>>;
public record GetCustomerByIdQuery(int Id) : IRequest<Customer>;
public record CreateCustomerCommand(Customer customer) : IRequest<Customer>;
public record UpdateCustomerCommand(int Id, Customer customer) : IRequest<Customer>;
public record DeleteCustomerCommand(int Id) : IRequest<Customer>;

*/

using MediatR;
using Ots.Api.Domain;
using Ots.Base;
using Ots.Schema;

namespace Ots.Api.Impl.Cqrs;


public record GetAllCustomersQuery : IRequest<ApiResponse<List<CustomerResponse>>>;
public record GetCustomerByIdQuery(int Id) : IRequest<ApiResponse<CustomerResponse>>;
public record CreateCustomerCommand(CustomerRequest customer) : IRequest<ApiResponse<CustomerResponse>>;
public record UpdateCustomerCommand(int Id, CustomerRequest customer) : IRequest<ApiResponse>;
public record DeleteCustomerCommand(int Id) : IRequest<ApiResponse>;

public record GetCustomerByParametersQuery(string? FirstName, string? LastName, string? Email) : IRequest<ApiResponse<List<CustomerResponse>>>;
