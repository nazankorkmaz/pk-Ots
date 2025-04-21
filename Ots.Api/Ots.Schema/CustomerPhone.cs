using Ots.Base;

namespace Ots.Schema;

public class CustomerPhoneRequest : BaseRequest
{
    public long CustomerId { get; set; }
    public string CountryCode { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsDefault { get; set; }
}
public class CustomerPhoneResponse : BaseResponse
{
    public long CustomerId { get; set; }
    public string CustomerName { get; set; }
    public int CustomerNumber { get; set; }
    public string CountryCode { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsDefault { get; set; }
}