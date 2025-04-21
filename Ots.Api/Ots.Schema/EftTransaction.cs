using Ots.Base;

namespace Ots.Schema;


public class EftTransactionRequest : BaseRequest
{
    public long AccountId { get; set; }
    public string ReveiverIban { get; set; }
    public string ReceiverName { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public string? PaymentCategoryCode { get; set; }
}

public class EftTransactionResponse : BaseResponse
{
    public long AccountId { get; set; }
    public string AccountName { get; set; }    
    public string AccountIban { get; set; }
    public int AccountNumber { get; set; }
    public long CustomerId { get; set; }
    public string CustomerName { get; set; }
    public int CustomerNumber { get; set; }
    public string ReveiverIban { get; set; }
    public string ReceiverName { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public decimal? FeeAmount { get; set; }
    public DateTime TransactionDate { get; set; }
    public string ReferenceNumber { get; set; }
    public string? PaymentCategoryCode { get; set; }
}