using MediatR;
using Ots.Base;
using Ots.Schema;

namespace Ots.Api.Impl.Cqrs;

public record GetAllAccountsQuery : IRequest<ApiResponse<List<AccountResponse>>>;
public record GetAccountByIdQuery(int Id) : IRequest<ApiResponse<AccountResponse>>;
public record CreateAccountCommand(AccountRequest Account) : IRequest<ApiResponse<AccountResponse>>;
public record UpdateAccountCommand(int Id, AccountRequest Account) : IRequest<ApiResponse>;
public record DeleteAccountCommand(int Id) : IRequest<ApiResponse>;