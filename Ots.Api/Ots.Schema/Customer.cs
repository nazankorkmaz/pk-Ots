using Ots.Base;

namespace Ots.Schema;

public class CustomerRequest : BaseRequest
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string LastName { get; set; }
    public string IdentityNumber { get; set; }
}

public class CustomerResponse : BaseResponse
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string LastName { get; set; }
    public string IdentityNumber { get; set; }
    public int CustomerNumber { get; set; }
    public DateTime OpenDate { get; set; }

    public virtual List<CustomerAddressResponse> CustomerAddresses { get; set; }
    public virtual List<CustomerPhoneResponse> CustomerPhones { get; set; }
    public virtual List<AccountResponse> Accounts { get; set; }
}