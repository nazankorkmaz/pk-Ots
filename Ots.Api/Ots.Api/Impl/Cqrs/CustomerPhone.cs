using MediatR;
using Ots.Base;
using Ots.Schema;

namespace Ots.Api.Impl.Cqrs;

public record GetAllCustomerPhonesQuery : IRequest<ApiResponse<List<CustomerPhoneResponse>>>;
public record GetCustomerPhoneByIdQuery(int Id) : IRequest<ApiResponse<CustomerPhoneResponse>>;
public record CreateCustomerPhoneCommand(CustomerPhoneRequest CustomerPhone) : IRequest<ApiResponse<CustomerPhoneResponse>>;
public record UpdateCustomerPhoneCommand(int Id, CustomerPhoneRequest CustomerPhone) : IRequest<ApiResponse>;
public record DeleteCustomerPhoneCommand(int Id) : IRequest<ApiResponse>;