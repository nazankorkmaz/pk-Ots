using MediatR;
using Ots.Api.Domain;

namespace Ots.Api.Impl.Cqrs;

public record GetAllCustomersQuery : IRequest<List<Customer>>;
public record GetCustomerByIdQuery(int Id) : IRequest<Customer>;
public record CreateCustomerCommand(Customer customer) : IRequest<Customer>;
public record UpdateCustomerCommand(int Id, Customer customer) : IRequest<Customer>;
public record DeleteCustomerCommand(int Id) : IRequest<Customer>;