using MediatR;
using Ots.Base;
using Ots.Schema;

namespace Ots.Api.Impl.Cqrs;

public record GetAllCustomerAddressQuery : IRequest<ApiResponse<List<CustomerAddressResponse>>>;
public record GetCustomerAddressByIdQuery(int Id) : IRequest<ApiResponse<CustomerAddressResponse>>;
public record CreateCustomerAddressCommand(int CustomerId,CustomerAddressRequest CustomerAddress) : IRequest<ApiResponse<CustomerAddressResponse>>;
public record UpdateCustomerAddressCommand(int Id, CustomerAddressRequest CustomerAddress) : IRequest<ApiResponse>;
public record DeleteCustomerAddressCommand(int Id) : IRequest<ApiResponse>;