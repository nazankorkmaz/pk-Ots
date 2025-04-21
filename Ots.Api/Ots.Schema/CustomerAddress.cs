using Ots.Base;

namespace Ots.Schema;

public class CustomerAddressRequest : BaseRequest
{
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? District { get; set; }
    public string? Street { get; set; }
    public string? ZipCode { get; set; }
    public bool IsDefault { get; set; }
}

public class CustomerAddressResponse : BaseResponse
{
    public long CustomerId { get; set; }
    public string CustomerName { get; set; }
    public int CustomerNumber { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? District { get; set; }
    public string? Street { get; set; }
    public string? ZipCode { get; set; }
    public bool IsDefault { get; set; }
}