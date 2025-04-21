// havale yapildiginda havale tablosuna bir kayit transactiona bir kayit atilir.
// karton tablo denirlir literaturde

using MediatR;
using Ots.Base;
using Ots.Schema;

namespace Ots.Api.Impl.Cqrs;

public record GetAccountTransactionByIdQuery(int Id) : IRequest<ApiResponse<AccountTransactionResponse>>;
public record GetAccountTransactionByParametersQuery() : IRequest<ApiResponse<List<AccountTransactionResponse>>>;