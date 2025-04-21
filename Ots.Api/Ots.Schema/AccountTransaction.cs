using Ots.Base;

namespace Ots.Schema;


public class AccountTransactionResponse : BaseResponse
{
    public long AccountId { get; set; }
    public string AccountName { get; set; }
    public int AccountNumber { get; set; }
    public long CustomerId { get; set; }
    public string CustomerName { get; set; }
    public int CustomerNumber { get; set; }
    public string Description { get; set; }
    public decimal? DebitAmount { get; set; } // +++ 
    public decimal? CreditAmount { get; set; } // --- 
    public DateTime TransactionDate { get; set; }
    public string ReferenceNumber { get; set; }
    public string? TransferType { get; set; }
}